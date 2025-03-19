using CustomMenu.Editor.Menu.MenuConfig;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace CustomMenu.Editor
{
    [InitializeOnLoad]
    internal static class DefaultSceneLoader
    {
        public const string EnableSetPlayModeSceneKey = "EnableSetPlayModeScene";

        private static bool IsChangePlayMoveScene
        {
            get => EditorPrefs.GetBool(EnableSetPlayModeSceneKey, false);
            set => EditorPrefs.SetBool(EnableSetPlayModeSceneKey, value);
        }

        static DefaultSceneLoader()
        {
            EditorApplication.delayCall += ChangePlayModeScene;
            EditorApplication.quitting += OnEditorQuitting;

            ChangePlayModeScene();
        }

        public static void ToggleAutoLoad()
        {
            IsChangePlayMoveScene = !IsChangePlayMoveScene;

            Debug.Log($"Auto load startup scene is now {(IsChangePlayMoveScene ? "enabled" : "disabled")}");

            ChangePlayModeScene();
        }

        private static void OnEditorQuitting()
        {
            EditorSceneManager.activeSceneChangedInEditMode -= OnSceneChanged;
            EditorApplication.quitting -= OnEditorQuitting;
        }

        private static void ChangePlayModeScene()
        {
            if (IsChangePlayMoveScene is false)
            {
                var currentScene = GetAsset<SceneAsset>(SceneManager.GetActiveScene().name);
                EditorSceneManager.playModeStartScene = currentScene;
                return;
            }

            var startUpScene = MenuConfigBase.MenuConfig.DefaultSceneAsset;
            if (startUpScene)
                EditorSceneManager.playModeStartScene = startUpScene;
        }

        private static T GetAsset<T>(string name) where T : Object
        {
            var assets = AssetDatabase.FindAssets($"{name} t:{typeof(T).Name}");
            if (assets.Length > 0)
                return (T)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(assets[0]), typeof(T));

            return null;
        }

        private static void OnSceneChanged(Scene oldScene, Scene newScene) => ChangePlayModeScene();
    }
}