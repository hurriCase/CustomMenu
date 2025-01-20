using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace CustomMenu.CustomMenu.Editor
{
    internal static class GeneratedMenuItems
    {
        [MenuItem("Scene/SampleScene", priority = 0)]
        private static void OpenSceneSampleScene()
        {
            var scenePath = "Assets/Scenes/SampleScene.unity";
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        }

        [MenuItem("Project/MenuConfig", priority = 0)]
        private static void SelectAssetMenuConfig()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>("Assets/CustomMenu/Runtime/Resources/MenuConfig.asset");
            Selection.activeObject = asset;
        }

        [MenuItem("Project/DeleteAllPrefs", priority = 0)]
        private static void DeleteAllPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}