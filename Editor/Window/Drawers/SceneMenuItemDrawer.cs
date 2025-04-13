using CustomMenu.Editor.MenuItems;
using UnityEditor;
using UnityEngine;

namespace CustomMenu.Editor.Window.Drawers
{
    [CustomPropertyDrawer(typeof(SceneMenuItem))]
    internal class SceneMenuItemDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var menuPathProp = property.FindFieldRelative(nameof(SceneMenuItem.MenuPath));
            var sceneProp = property.FindFieldRelative(nameof(SceneMenuItem.Scene));
            var priorityProp = property.FindFieldRelative(nameof(SceneMenuItem.Priority));

            position.height = EditorGUIUtility.singleLineHeight;

            var sceneRect = new Rect(position);
            EditorGUI.PropertyField(sceneRect, sceneProp, new GUIContent("Scene Asset"));
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            var menuPathRect = new Rect(position);
            EditorGUI.PropertyField(menuPathRect, menuPathProp, new GUIContent("Menu Path"));
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            var priorityRect = new Rect(position);
            EditorGUI.PropertyField(priorityRect, priorityProp, new GUIContent("Priority"));

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 3;
    }
}