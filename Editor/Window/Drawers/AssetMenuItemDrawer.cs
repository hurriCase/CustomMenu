using CustomMenu.Editor.MenuItems;
using UnityEditor;
using UnityEngine;

namespace CustomMenu.Editor.Window.Drawers
{
    [CustomPropertyDrawer(typeof(AssetMenuItem))]
    internal class AssetMenuItemDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var menuPathProp = property
                .FindPropertyRelative(nameof(AssetMenuItem.MenuPath).ConvertToBackingField());
            var assetProp = property
                .FindPropertyRelative(nameof(AssetMenuItem.Asset).ConvertToBackingField());
            var priorityProp = property
                .FindPropertyRelative(nameof(AssetMenuItem.Priority).ConvertToBackingField());

            position.height = EditorGUIUtility.singleLineHeight;

            var assetRect = new Rect(position);
            EditorGUI.PropertyField(assetRect, assetProp, new GUIContent("Asset"));
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            var menuPathRect = new Rect(position);
            EditorGUI.PropertyField(menuPathRect, menuPathProp, new GUIContent("Menu Path"));
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            var priorityRect = new Rect(position);
            EditorGUI.PropertyField(priorityRect, priorityProp, new GUIContent("Priority"));

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 3;
    }
}