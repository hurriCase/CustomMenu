using CustomMenu.Editor.MenuItems;
using CustomMenu.Editor.MenuItems.MethodExecution;
using UnityEditor;
using UnityEngine;

namespace CustomMenu.Editor.Menu.MenuConfig
{
    /// <summary>
    /// Base class for menu configuration. Inherit from this class to create your own menu configuration.
    /// </summary>
    public abstract class MenuConfigBase : ScriptableObject
    {
        private static MenuConfigBase _menuConfig;

        internal static MenuConfigBase MenuConfig => _menuConfig = _menuConfig ? _menuConfig : InitializeConfig();

        [field: SerializeField] internal SceneAsset DefaultSceneAsset { get; private set; }
        [field: SerializeField] internal SceneMenuItem[] SceneMenuItems { get; private set; }
        [field: SerializeField] internal AssetMenuItem[] AssetMenuItems { get; private set; }
        [field: SerializeField] internal MethodExecutionItem[] MethodExecutionItems { get; private set; }

        private static MenuConfigBase InitializeConfig()
        {
            var guids = AssetDatabase.FindAssets("t:MenuConfigBase");
            switch (guids.Length)
            {
                case 0:
                    Debug.LogWarning($"[MenuConfigBase::InitializeConfig] No menu configuration found in the project");
                    return null;

                case > 1:
                    Debug.LogWarning(
                        "[MenuConfigBase::InitializeConfig] Multiple configurations found. Using the first one.");
                    break;
            }

            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            return AssetDatabase.LoadAssetAtPath<MenuConfigBase>(path);
        }
    }
}