using CustomMenu.CustomMenu.Editor.MenuItems;
using CustomMenu.Editor.MenuItems.MethodExecution;
using UnityEngine;

namespace CustomMenu.Editor.Menu.MenuConfig
{
    /// <summary>
    /// Base class for menu configuration. Inherit from this class to create your own menu configuration.
    /// </summary>
    public abstract class MenuConfigBase : ScriptableObject
    {
        internal static MenuConfigBase MenuConfig { get; private set; }

        private void OnEnable() => MenuConfig ??= this;

        [field: SerializeField] internal SceneMenuItem[] SceneMenuItems { get; private set; }
        [field: SerializeField] internal AssetMenuItem[] AssetMenuItems { get; private set; }
        [field: SerializeField] internal MethodExecutionItem[] MethodExecutionItems { get; private set; }
    }
}