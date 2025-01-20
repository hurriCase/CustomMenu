# Custom Menu Generator

A flexible Unity editor menu generation system that allows creating custom menu items through scriptable objects.

## Quick Start

1. **Create Menu Configuration**
```csharp
// Create custom menu config
[Resource(name: "CustomMenuConfig", resourcePath: "MyApp/Configs")]
internal sealed class CustomMenuConfig : MenuConfigBase 
{
    private static CustomMenuConfig _instance;
    internal static CustomMenuConfig Instance => _instance ??= GetInstance<CustomMenuConfig>();
}

// Initialize the config
MenuConfigInitializer.InitializeMenuConfig(CustomMenuConfig.Instance);
```

2. **Configure Menu Items**
```csharp
// Add items in the inspector:
- Scene menu items (open scenes)
- Asset menu items (select assets)  
- Method execution items (execute predefined methods)
```

3. **Generate Menus**
   Click "Generate Menu Items" in the inspector to create the menu system.

## Core Features

- Configurable menu items through ScriptableObject
- Different menu item types:
    - Scene opening
    - Asset selection
    - Method execution
- Custom menu paths and priorities
- Editor UI for configuration
- Automated menu script generation

## Essential Configuration

### Menu Config Base
```csharp
internal abstract class MenuConfigBase : ScriptableObject
{
    [SerializeField] internal SceneMenuItem[] SceneMenuItems;
    [SerializeField] internal AssetMenuItem[] AssetMenuItems; 
    [SerializeField] internal MethodExecutionItem[] MethodExecutionItems;
}
```

### Menu Items
```csharp
// Scene Menu Item
internal sealed class SceneMenuItem
{
    internal string MenuPath = "Project/Scenes";
    internal SceneAsset Scene;
    internal int Priority;
}

// Asset Menu Item  
internal sealed class AssetMenuItem
{
    internal string MenuPath = "Project/Assets";
    internal Object Asset;
    internal int Priority;  
}

// Method Execution Item
internal sealed class MethodExecutionItem 
{
    internal string MenuPath = "Project/MethodExecution";
    internal MethodExecutionType MethodExecutionType;
    internal int Priority;
}
```

## Best Practices

1. **Menu Organization**
    - Use clear menu path hierarchy
    - Group related items
    - Assign sensible priorities

2. **Configuration**
    - Create a single menu config per project
    - Initialize early in editor startup
    - Keep menu items organized

3. **Method Execution**
    - Add new method types to MethodExecutionType enum
    - Implement method logic in MenuManager
    - Use for editor-only functionality

## API Reference

### MenuConfigBase
```csharp
internal abstract class MenuConfigBase : ScriptableObject
{
    protected static T GetInstance<T>() where T : MenuConfigBase;
    internal SceneMenuItem[] SceneMenuItems { get; }
    internal AssetMenuItem[] AssetMenuItems { get; }
    internal MethodExecutionItem[] MethodExecutionItems { get; }
}
```

### MenuConfigInitializer
```csharp
internal static class MenuConfigInitializer
{
    internal static MenuConfigBase MenuConfig { get; }
    internal static void InitializeMenuConfig(MenuConfigBase menuConfig);
}
```

## Common Issues & Solutions

1. **Menus Not Generating**
    - Verify menu config is initialized
    - Check menu paths are valid
    - Ensure assets/scenes exist

2. **Invalid Menu Items**
    - Menu paths must not be empty
    - Scene/asset references required
    - Priority values should be unique

3. **Method Execution Errors**
    - Add new methods to enum
    - Implement method logic
    - Editor-only methods only

## Technical Details

### Menu Generation Process
1. Read configuration from ScriptableObject
2. Validate menu items and paths
3. Generate C# menu script
4. Refresh Unity asset database

### Menu Item Types
- Scene menus: Open Unity scenes
- Asset menus: Select project assets
- Method execution: Run editor functionality