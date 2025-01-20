using AssetLoader.Runtime;
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
        /// <summary>
        /// Gets a singleton instance of the specified menu config type.
        /// </summary>
        /// <typeparam name="T">The type of menu config to retrieve</typeparam>
        /// <returns>Instance of the menu config</returns>
        protected static T GetInstance<T>() where T : MenuConfigBase
            => ResourceLoader<T>.Load();

        [field: SerializeField] internal SceneMenuItem[] SceneMenuItems { get; private set; }
        [field: SerializeField] internal AssetMenuItem[] AssetMenuItems { get; private set; }
        [field: SerializeField] internal MethodExecutionItem[] MethodExecutionItems { get; private set; }
    }
}