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

            var menuPathProp =
                property.FindPropertyRelative(nameof(MethodExecutionItem.MenuPath).ConvertToBackingField());
            var methodTypeProp =
                property.FindPropertyRelative(nameof(MethodExecutionItem.MethodExecutionType).ConvertToBackingField());
            var priorityProp =
                property.FindPropertyRelative(nameof(MethodExecutionItem.Priority).ConvertToBackingField());

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