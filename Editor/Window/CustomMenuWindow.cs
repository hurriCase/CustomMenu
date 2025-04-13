using UnityEditor;
using UnityEngine;

namespace CustomMenu.Editor.Window
{
    internal sealed class CustomMenuWindow : EditorWindow
    {
        private SerializedObject _serializedObject;
        private CustomMenuSettings _settings;

        private SerializedProperty _defaultSceneAssetProperty;
        private SerializedProperty _sceneMenuItemsProperty;
        private SerializedProperty _assetMenuItemsProperty;
        private SerializedProperty _methodExecutionItemsProperty;
        private SerializedProperty _scriptingSymbolsProperty;

        private Vector2 _scrollPosition;

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorApplication.delayCall += () =>
            {
                if (CustomMenuSettings.SettingsExist() is false)
                    CustomMenuSettings.GetOrCreateSettings();
            };
        }

        [MenuItem("Tools/Custom Menu Settings")]
        public static void ShowWindow()
        {
            var window = GetWindow<CustomMenuWindow>("Custom Menu Settings");
            window.minSize = new Vector2(450, 600);
            window.Show();
        }

        private void OnEnable()
        {
            _settings = CustomMenuSettings.GetOrCreateSettings();
            _serializedObject = new SerializedObject(_settings);

            _defaultSceneAssetProperty = _serializedObject.FindField(nameof(_settings.DefaultSceneAsset));
            _sceneMenuItemsProperty = _serializedObject.FindField(nameof(_settings.SceneMenuItems));
            _assetMenuItemsProperty = _serializedObject.FindField(nameof(_settings.AssetMenuItems));
            _methodExecutionItemsProperty = _serializedObject.FindField(nameof(_settings.MethodExecutionItems));
            _scriptingSymbolsProperty = _serializedObject.FindField(nameof(_settings.ScriptingSymbols));
        }

        private void OnGUI()
        {
            if (_serializedObject == null || !_settings)
            {
                OnEnable();

                if (_serializedObject == null)
                {
                    EditorGUILayout.HelpBox("Failed to load or create settings asset.", MessageType.Error);
                    return;
                }
            }

            _serializedObject.Update();

            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Custom Menu Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space(10);

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            EditorGUILayout.PropertyField(_defaultSceneAssetProperty, new GUIContent("Default Scene"));
            EditorGUILayout.PropertyField(_sceneMenuItemsProperty, true);
            EditorGUILayout.PropertyField(_assetMenuItemsProperty, true);
            EditorGUILayout.PropertyField(_methodExecutionItemsProperty, true);
            EditorGUILayout.PropertyField(_scriptingSymbolsProperty, true);

            EditorGUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Generate Menu Items", GUILayout.Width(150), GUILayout.Height(30)))
            {
                _serializedObject.ApplyModifiedProperties();
                MenuManager.GenerateMenuItemsScriptFromSettings(_settings);
                EditorUtility.SetDirty(_settings);
                AssetDatabase.SaveAssets();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            EditorGUILayout.Space(5);

            if (_serializedObject.ApplyModifiedProperties())
                EditorUtility.SetDirty(_settings);
        }
    }
}