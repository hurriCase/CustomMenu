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

            EditorGUILayout.Space(10);
            var sceneMenuItemsPropertyName = "SceneMenuItems";
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty(sceneMenuItemsPropertyName.ConvertToBackingField()), true);

            EditorGUILayout.Space(10);
            var assetMenuItemsPropertyName = "AssetMenuItems";
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty(assetMenuItemsPropertyName.ConvertToBackingField()), true);

            EditorGUILayout.Space(10);
            var methodExecutionItemsPropertyName = "MethodExecutionItems";
            EditorGUILayout.PropertyField(
                serializedObject.FindProperty(methodExecutionItemsPropertyName.ConvertToBackingField()), true);

            serializedObject.ApplyModifiedProperties();

            if (GUI.changed)
                EditorUtility.SetDirty(target);

            EditorGUILayout.Space(20);
            if (GUILayout.Button("Generate Menu Items"))
                MenuManager.GenerateMenuItemsScript();
        }
    }
}