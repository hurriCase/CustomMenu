# Custom Menu Package

A Unity editor extension that provides a flexible and maintainable way to create custom menu items through ScriptableObject configurations.

## Features

- Create menu items for opening scenes
- Create menu items for selecting assets
- Create menu items for executing custom methods
- Configure menu items through ScriptableObject
- Auto-generate menu item scripts

## Setup

1. Create a menu configuration asset by inheriting from `MenuConfigBase`:
```csharp
[CreateAssetMenu(fileName = "MenuConfig", menuName = "Config/MenuConfig")]
public class MenuConfig : MenuConfigBase
{
}
```

2. Create an instance of your menu configuration asset
3. Configure your menu items in the inspector:
   - Scene Menu Items
   - Asset Menu Items
   - Method Execution Items

4. Click "Generate Menu Items" to create the menu script

## Menu Item Types

### Scene Menu Items
- Opens specified scenes in the editor
- Configure scene asset, menu path, and priority
- Example path: "Project/Scenes/Main"

### Asset Menu Items
- Selects specified assets in the Project window
- Configure target asset, menu path, and priority
- Example path: "Project/Assets/GameConfig"

### Method Execution Items
- Executes predefined methods
- Configure method type, menu path, and priority
- Available methods:
   - DeleteAllPlayerPrefs
- Example path: "Project/Utils/ClearPrefs"

## Configuration Properties

Each menu item type supports:
- MenuPath: Path in Unity's menu bar
- Priority: Order within menu section (lower numbers appear first)
- Specific fields per type (Scene, Asset, or Method)

## Usage Example

```csharp
// Create menu config asset
[CreateAssetMenu(fileName = "GameMenuConfig", menuName = "Config/GameMenuConfig")]
public class GameMenuConfig : MenuConfigBase 
{
}

// Menu items will be generated as:
[MenuItem("Project/Scenes/Main", priority = 0)]
private static void OpenSceneMain()
{
    EditorSceneManager.OpenScene("Assets/Scenes/Main.unity", OpenSceneMode.Single);
}
```

## Notes

- Menu paths should be unique
- Generated script is created at "Assets/CustomMenu/Scripts/GeneratedMenuItems.cs"
- Regenerate menu items after configuration changes