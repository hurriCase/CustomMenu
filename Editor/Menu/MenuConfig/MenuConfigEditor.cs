using CustomExtensions.Editor;
using UnityEditor;
using UnityEngine;

namespace CustomMenu.Editor.Menu.MenuConfig
{
    [CustomEditor(typeof(MenuConfigBase), true)]
    internal sealed class MenuConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUI.enabled = false;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            GUI.enabled = true;

            var sceneMenuItemsField = nameof(MenuConfigBase.SceneMenuItems).ConvertToBackingField();
            var assetMenuItemsField = nameof(MenuConfigBase.AssetMenuItems).ConvertToBackingField();
            var methodExecutionItemsField = nameof(MenuConfigBase.MethodExecutionItems).ConvertToBackingField();

            DrawPropertiesExcluding(serializedObject,
                "m_Script",
                sceneMenuItemsField,
                assetMenuItemsField,
                methodExecutionItemsField);

            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(serializedObject.FindProperty(sceneMenuItemsField), true);
            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(serializedObject.FindProperty(assetMenuItemsField), true);
            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(serializedObject.FindProperty(methodExecutionItemsField), true);

            serializedObject.ApplyModifiedProperties();

            if (GUI.changed)
                EditorUtility.SetDirty(target);

            EditorGUILayout.Space(20);
            if (GUILayout.Button("Generate Menu Items"))
                MenuManager.GenerateMenuItemsScript();
        }
    }
}