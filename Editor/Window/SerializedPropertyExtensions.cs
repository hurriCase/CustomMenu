using UnityEditor;

namespace CustomMenu.Editor.Window
{
    internal static class SerializedPropertyExtensions
    {
        internal static SerializedProperty FindField(this SerializedObject serializedObject, string name) =>
            serializedObject.FindProperty(ConvertToBackingField(name));

        internal static SerializedProperty FindFieldRelative(this SerializedProperty serializedObject, string name) =>
            serializedObject.FindPropertyRelative(ConvertToBackingField(name));

        private static string ConvertToBackingField(this string propertyName)
            => $"<{propertyName}>k__BackingField";
    }
}