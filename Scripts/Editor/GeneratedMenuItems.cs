using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using CustomMenu.Editor;

namespace CustomMenu.Scripts.Editor
{
    internal static class GeneratedMenuItems
    {
        [MenuItem("--Project--/Toggle Default Scene Auto Load", priority = 1)]
        private static void ToggleDefaultSceneAutoLoad()
        {
            DefaultSceneLoader.ToggleAutoLoad();
        }

        [MenuItem("--Project--/Toggle Default Scene Auto Load", true)]
        private static bool ValidateToggleDefaultSceneAutoLoad()
        {
            Menu.SetChecked("--Project--/Toggle Default Scene Auto Load", EditorPrefs.GetBool(DefaultSceneLoader.EnableSetPlayModeSceneKey, false));
            return true;
        }
    }
}