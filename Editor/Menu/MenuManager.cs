using System;
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
            var config = MenuConfigInitializer.MenuConfig;

            var scriptContent = GenerateMenuItemsScriptContent(config);
            if (string.IsNullOrWhiteSpace(scriptContent))
                return;

            var scriptPath = "Assets/CustomMenu/Editor/GeneratedMenuItems.cs";

            System.IO.File.WriteAllText(scriptPath, scriptContent);
            AssetDatabase.Refresh();
        }

        private static string GenerateMenuItemsScriptContent(MenuConfigBase config)
        {
            var content = @"using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace CustomMenu.CustomMenu.Editor
{
    internal static class GeneratedMenuItems
    {";

            // Generate Scene Menu Items
            foreach (var item in config.SceneMenuItems)
            {
                if (string.IsNullOrEmpty(item.MenuPath))
                {
                    Debug.LogError($"[MenuManager::GenerateMenuItemsScriptContent] Menu Path cannot be empty for {item.SceneName}");
                    return string.Empty;
                }

                var methodName = $"OpenScene{item.SceneName.Replace(" ", string.Empty)}";
                content += $@"
        [MenuItem(""{item.MenuPath}"", priority = {item.Priority})]
        private static void {methodName}()
        {{
            var scenePath = ""{item.ScenePath}"";
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        }}";
            }

            // Generate Asset Menu Items
            foreach (var item in config.AssetMenuItems.Where(x => x.Asset != null))
            {
                if (string.IsNullOrEmpty(item.MenuPath))
                {
                    Debug.LogError($"[MenuManager::GenerateMenuItemsScriptContent] Menu Path cannot be empty for {item.Asset.name}");
                    return string.Empty;
                }

                var methodName = $"SelectAsset{item.Asset.name.Replace(" ", "_")}";
                var assetPath = AssetDatabase.GetAssetPath(item.Asset);

                content += $@"

        [MenuItem(""{item.MenuPath}"", priority = {item.Priority})]
        private static void {methodName}()
        {{
            var asset = AssetDatabase.LoadAssetAtPath<Object>(""{assetPath}"");
            Selection.activeObject = asset;
        }}";
            }

            // Generate Custom Menu Items
            foreach (var item in config.MethodExecutionItems)
            {
                if (string.IsNullOrEmpty(item.MenuPath))
                {
                    Debug.LogError($"[MenuManager::GenerateMenuItemsScriptContent] Menu Path cannot be empty for {item.MethodExecutionType}");
                    return string.Empty;
                }

                content += $@"

        [MenuItem(""{item.MenuPath}"", priority = {item.Priority})]
        private static void {item.MethodExecutionType}()
        {{
            {GenerateCustomMethodContent(item.MethodExecutionType)}
        }}";
            }

            content += @"
    }
}";

            return content;
        }

        private static string GenerateCustomMethodContent(MethodExecutionType methodName) =>
            methodName switch
            {
                MethodExecutionType.DeleteAllPlayerPrefs => "PlayerPrefs.DeleteAll();",

                _ => throw new ArgumentOutOfRangeException(nameof(methodName), methodName, null)
            };
    }
}