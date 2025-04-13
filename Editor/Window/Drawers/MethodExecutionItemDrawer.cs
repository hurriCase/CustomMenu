using CustomMenu.Editor.MenuItems.MethodExecution;
using UnityEditor;
using UnityEngine;

namespace CustomMenu.Editor.Window.Drawers
{
    [CustomPropertyDrawer(typeof(MethodExecutionItem))]
    internal class MethodExecutionItemDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var menuPathProp = property.FindFieldRelative(nameof(MethodExecutionItem.MenuPath));
            var methodTypeProp
                = property.FindFieldRelative(nameof(MethodExecutionItem.MethodExecutionType));
            var priorityProp = property.FindFieldRelative(nameof(MethodExecutionItem.Priority));

            position.height = EditorGUIUtility.singleLineHeight;

            var methodTypeRect = new Rect(position);
            EditorGUI.PropertyField(methodTypeRect, methodTypeProp, new GUIContent("Method Type"));
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