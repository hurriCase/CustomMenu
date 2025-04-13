using System;

namespace CustomMenu.Editor.MenuItems.MenuItems
{
    [Serializable]
    internal sealed class ScriptingSymbolMenuItem : BaseMenuItem<string>
    {
        internal string GetPrefsKey() => $"ScriptingSymbol_{MenuTarget}";
    }
}