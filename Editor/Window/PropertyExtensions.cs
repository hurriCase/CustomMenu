namespace CustomMenu.Editor.Window
{
    internal static class PropertyExtensions
    {
        internal static string ConvertToBackingField(this string propertyName)
            => $"<{propertyName}>k__BackingField";
    }
}