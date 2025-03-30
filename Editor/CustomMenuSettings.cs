using System.IO;
using CustomMenu.Editor.MenuItems;
using CustomMenu.Editor.MenuItems.MethodExecution;
using CustomMenu.Editor.Window;
using UnityEditor;
using UnityEngine;

namespace CustomMenu.Editor
{
    internal sealed class CustomMenuSettings : ScriptableObject
    {
        [field: SerializeField] internal SceneAsset DefaultSceneAsset { get; private set; }
        [field: SerializeField] internal SceneMenuItem[] SceneMenuItems { get; private set; }
        [field: SerializeField] internal AssetMenuItem[] AssetMenuItems { get; private set; }
        [field: SerializeField] internal MethodExecutionItem[] MethodExecutionItems { get; private set; }

        private const string SettingsPath = "Assets/CustomMenu/Editor/Settings/CustomMenuSettings.asset";

        internal static CustomMenuSettings GetOrCreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<CustomMenuSettings>(SettingsPath);
            if (settings)
                return settings;

            settings = CreateInstance<CustomMenuSettings>();

            var directory = Path.GetDirectoryName(SettingsPath);
            if (string.IsNullOrEmpty(directory) is false && Directory.Exists(directory) is false)
                Directory.CreateDirectory(directory);

            AssetDatabase.CreateAsset(settings, SettingsPath);
            AssetDatabase.SaveAssets();

            CustomMenuWindow.ShowWindow();

            return settings;
        }

        internal static bool SettingsExist() => AssetDatabase.LoadAssetAtPath<CustomMenuSettings>(SettingsPath);
    }
}