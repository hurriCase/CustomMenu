using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CustomMenu.Editor.MenuItems.MethodExecution;
using UnityEditor;
using UnityEngine;

namespace CustomMenu.Editor
{
    internal static class MenuManager
    {
        internal static void GenerateMenuItemsScriptFromSettings(CustomMenuSettings settings)
        {
            var scriptContent = GenerateMenuItemsScriptContentFromSettings(settings);

            if (string.IsNullOrWhiteSpace(scriptContent))
                return;

            WriteScriptFile(scriptContent);
        }

        private static void WriteScriptFile(string scriptContent)
        {
            var scriptPath = $"Assets/CustomMenu/Scripts/Editor/GeneratedMenuItems.cs";
            var directory = Path.GetDirectoryName(scriptPath);
            if (string.IsNullOrEmpty(directory) is false && Directory.Exists(directory) is false)
                Directory.CreateDirectory(directory);

            File.WriteAllText(scriptPath, scriptContent);
            AssetDatabase.Refresh();

            Debug.Log("Generated menu items script successfully at: " + scriptPath);
        }

        private static string GenerateMenuItemsScriptContentFromSettings(CustomMenuSettings settings)
        {
            var content = @"using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using CustomMenu.Editor;
using CustomMenu.Editor.MenuItems.MethodExecution.Helpers;

namespace CustomMenu.Scripts.Editor
{
    internal static class GeneratedMenuItems
    {";

            var isFirstMenuItem = true;

            var usedMethodNames = new HashSet<string>();
            var usedMenuPaths = new HashSet<string>();

            // Generate Scene Menu Items
            if (settings.SceneMenuItems != null)
                foreach (var item in settings.SceneMenuItems)
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
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() is false)
                return;

            var scenePath = ""{item.ScenePath}"";
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        }}";
                }

            // Generate Asset Menu Items
            if (settings.AssetMenuItems != null)
                foreach (var item in settings.AssetMenuItems.Where(assetMenuItem => assetMenuItem.Asset))
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

            // Generate Custom Method Execution Menu Items
            if (settings.MethodExecutionItems != null)
                foreach (var item in settings.MethodExecutionItems)
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

                    // Generate method based on its type (Toggle or Regular)
                    if (IsToggleMethod(item.MethodExecutionType))
                    {
                        content += GenerateToggleMethodContent(item, usedMethodNames);
                    }
                    else
                    {
                        content += GenerateRegularMethodContent(item, usedMethodNames);
                    }
                }

            content += @"
    }
}";

            return content;
        }

        private static string GenerateRegularMethodContent(MethodExecutionItem item, HashSet<string> usedMethodNames)
        {
            var baseMethodName = item.MethodExecutionType.ToString();
            var methodName = GetUniqueMethodName(baseMethodName, usedMethodNames);

            return $@"
        [MenuItem(""{item.MenuPath}"", priority = {item.Priority})]
        private static void {methodName}()
        {{
            {GenerateMethodExecutionCode(item.MethodExecutionType)}
        }}";
        }

        private static string GenerateToggleMethodContent(MethodExecutionItem item, HashSet<string> usedMethodNames)
        {
            var baseMethodName = item.MethodExecutionType.ToString();
            var methodName = GetUniqueMethodName(baseMethodName, usedMethodNames);
            var validateMethodName = $"Validate{methodName}";
            var toggleCheckStateMethod = GetToggleCheckStateMethod(item.MethodExecutionType);

            return $@"
        [MenuItem(""{item.MenuPath}"", priority = {item.Priority})]
        private static void {methodName}()
        {{
            {GenerateMethodExecutionCode(item.MethodExecutionType)}
        }}

        [MenuItem(""{item.MenuPath}"", true)]
        private static bool {validateMethodName}()
        {{
            Menu.SetChecked(""{item.MenuPath}"", {toggleCheckStateMethod});
            return true;
        }}";
        }

        private static string GenerateMethodExecutionCode(MethodExecutionType methodType) =>
            methodType switch
            {
                MethodExecutionType.DeleteAllPlayerPrefs => "PlayerPrefs.DeleteAll();",
                MethodExecutionType.ToggleDefaultSceneAutoLoad => "DefaultSceneLoader.ToggleAutoLoad();",
                MethodExecutionType.ToggleDebugSymbol => "DebugSymbolHandler.ToggleDebugSymbol();",
                _ => throw new ArgumentOutOfRangeException(nameof(methodType), methodType, null)
            };

        private static string GetToggleCheckStateMethod(MethodExecutionType methodType) =>
            methodType switch
            {
                MethodExecutionType.ToggleDefaultSceneAutoLoad => "DefaultSceneLoader.IsDefaultSceneSet()",
                MethodExecutionType.ToggleDebugSymbol => "DebugSymbolHandler.IsDebugSymbolEnabled()",
                _ => throw new ArgumentOutOfRangeException(nameof(methodType), methodType, "Not a toggle method type")
            };

        private static bool IsToggleMethod(MethodExecutionType methodType)
        {
            return methodType switch
            {
                MethodExecutionType.ToggleDefaultSceneAutoLoad => true,
                MethodExecutionType.ToggleDebugSymbol => true,
                _ => false
            };
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
    }
}