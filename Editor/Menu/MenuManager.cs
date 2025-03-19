using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CustomMenu.Editor.Menu.MenuConfig;
using CustomMenu.Editor.MenuItems.MethodExecution;
using UnityEditor;
using UnityEngine;

namespace CustomMenu.Editor.Menu
{
    internal static class MenuManager
    {
        public static void GenerateMenuItemsScript()
        {
            var config = MenuConfigBase.MenuConfig;

            var scriptContent = GenerateMenuItemsScriptContent(config);
            if (string.IsNullOrWhiteSpace(scriptContent))
                return;

            var scriptPath = $"Assets/CustomMenu/Scripts/Editor/GeneratedMenuItems.cs";
            var directory = Path.GetDirectoryName(scriptPath);
            if (string.IsNullOrEmpty(directory) is false && Directory.Exists(directory) is false)
                Directory.CreateDirectory(directory);

            File.WriteAllText(scriptPath, scriptContent);
            AssetDatabase.Refresh();
        }

        private static string GenerateMenuItemsScriptContent(MenuConfigBase config)
        {
            var content = @"using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using CustomMenu.Editor;

namespace CustomMenu.Scripts.Editor
{
    internal static class GeneratedMenuItems
    {";

            var isFirstMenuItem = true;

            var usedMethodNames = new HashSet<string>();
            var usedMenuPaths = new HashSet<string>();

            // Generate Scene Menu Items
            if (config.SceneMenuItems != null)
                foreach (var item in config.SceneMenuItems)
                {
                    if (!item.Scene)
                    {
                        Debug.LogError(
                            "[MenuManager::GenerateMenuItemsScriptContent] Scene Asset must be assigned to create Menu Items");
                        return string.Empty;
                    }

                    if (ValidateMenuPath(item.MenuPath) is false)
                        return string.Empty;

                    if (usedMenuPaths.Add(item.MenuPath) is false)
                    {
                        Debug.LogError($"[MenuManager] Duplicate menu path '{item.MenuPath}' for scene '{item.SceneName}'. Skipping this item.");
                        continue;
                    }

                    var baseMethodName = $"OpenScene{item.SceneName.Replace(" ", string.Empty)}";
                    var methodName = GetUniqueMethodName(baseMethodName, usedMethodNames);

                    if (isFirstMenuItem)
                        isFirstMenuItem = false;
                    else
                        content += "\n";

                    content += $@"
        [MenuItem(""{item.MenuPath}"", priority = {item.Priority})]
        private static void {methodName}()
        {{
            var scenePath = ""{item.ScenePath}"";
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        }}";
                }

            // Generate Asset Menu Items
            if (config.AssetMenuItems != null)
                foreach (var item in config.AssetMenuItems.Where(assetMenuItem => assetMenuItem.Asset))
                {
                    if (ValidateMenuPath(item.MenuPath) is false)
                        return string.Empty;

                    if (usedMenuPaths.Add(item.MenuPath) is false)
                    {
                        Debug.LogError($"[MenuManager] Duplicate menu path '{item.MenuPath}' for asset '{item.Asset.name}'. Skipping this item.");
                        continue;
                    }

                    var baseMethodName = $"SelectAsset{item.Asset.name.Replace(" ", "_")}";
                    var methodName = GetUniqueMethodName(baseMethodName, usedMethodNames);

                    var assetPath = AssetDatabase.GetAssetPath(item.Asset);

                    if (isFirstMenuItem)
                        isFirstMenuItem = false;
                    else
                        content += "\n";

                    content += $@"
        [MenuItem(""{item.MenuPath}"", priority = {item.Priority})]
        private static void {methodName}()
        {{
            var asset = AssetDatabase.LoadAssetAtPath<Object>(""{assetPath}"");
            Selection.activeObject = asset;
        }}";
                }

            // Generate Custom Menu Items
            if (config.MethodExecutionItems != null)
                foreach (var item in config.MethodExecutionItems)
                {
                    if (ValidateMenuPath(item.MenuPath) is false)
                        return string.Empty;

                    if (usedMenuPaths.Add(item.MenuPath) is false)
                    {
                        Debug.LogError($"[MenuManager] Duplicate menu path '{item.MenuPath}' for method '{item.MethodExecutionType}'. Skipping this item.");
                        continue;
                    }

                    if (isFirstMenuItem)
                        isFirstMenuItem = false;
                    else
                        content += "\n";

                    if (item.MethodExecutionType == MethodExecutionType.ToggleDefaultSceneAutoLoad)
                    {
                        var baseMethodName = item.MethodExecutionType.ToString();
                        var methodName = GetUniqueMethodName(baseMethodName, usedMethodNames);
                        var validateMethodName = $"Validate{methodName}";

                        content += $@"
        [MenuItem(""{item.MenuPath}"", priority = {item.Priority})]
        private static void {methodName}()
        {{
            DefaultSceneLoader.ToggleAutoLoad();
        }}

        [MenuItem(""{item.MenuPath}"", true)]
        private static bool {validateMethodName}()
        {{
            Menu.SetChecked(""{item.MenuPath}"", EditorPrefs.GetBool(DefaultSceneLoader.EnableSetPlayModeSceneKey, false));
            return true;
        }}";
                    }
                    else
                    {
                        var baseMethodName = item.MethodExecutionType.ToString();
                        var methodName = GetUniqueMethodName(baseMethodName, usedMethodNames);

                        content += $@"
        [MenuItem(""{item.MenuPath}"", priority = {item.Priority})]
        private static void {methodName}()
        {{
            {GenerateCustomMethodContent(item.MethodExecutionType)}
        }}";
                    }
                }

            content += @"
    }
}";

            return content;
        }

        private static string GetUniqueMethodName(string baseName, HashSet<string> usedNames)
        {
            var methodName = baseName;
            var suffix = 1;

            while (usedNames.Contains(methodName))
            {
                methodName = $"{baseName}_{suffix}";
                suffix++;
            }

            usedNames.Add(methodName);
            return methodName;
        }

        private static bool ValidateMenuPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("[MenuManager] Menu Path cannot be empty");
                return false;
            }

            if (path.Contains('/') is false)
            {
                Debug.LogError($"[MenuManager] Menu path '{path}' should contain a submenu " +
                               "(using forward slash to specify it, e.g. 'Tools/Custom')");
                return false;
            }

            if (path.EndsWith('/'))
            {
                Debug.LogError($"[MenuManager] Menu path '{path}' cannot end with a forward slash");
                return false;
            }

            if (path.StartsWith('/'))
            {
                Debug.LogError($"[MenuManager] Menu path '{path}' cannot start with a forward slash");
                return false;
            }

            if (path.Contains("//") is false)
                return true;

            Debug.LogError($"[MenuManager] Menu path '{path}' contains double slashes which would create empty menu items");
            return false;
        }

        private static string GenerateCustomMethodContent(MethodExecutionType methodName) =>
            methodName switch
            {
                MethodExecutionType.DeleteAllPlayerPrefs => "PlayerPrefs.DeleteAll();",
                MethodExecutionType.ToggleDefaultSceneAutoLoad => "DefaultSceneLoader.ToggleAutoLoad();",
                _ => throw new ArgumentOutOfRangeException(nameof(methodName), methodName, null)
            };
    }
}