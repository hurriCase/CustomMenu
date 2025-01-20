using CustomMenu.Editor.Menu.MenuConfig;

namespace CustomMenu.Editor.Menu
{
    /// <summary>
    /// Initializes the menu configuration system
    /// </summary>
    internal static class MenuConfigInitializer
    {
        internal static MenuConfigBase MenuConfig { get; private set; }

        /// <summary>
        /// Initializes the menu system with a custom menu configuration
        /// </summary>
        /// <param name="menuConfig">The menu configuration to use</param>
        internal static void InitializeMenuConfig(MenuConfigBase menuConfig)
        {
            MenuConfig = menuConfig;
        }
    }
}