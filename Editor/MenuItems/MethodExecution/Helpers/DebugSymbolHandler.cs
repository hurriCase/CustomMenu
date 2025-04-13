using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace CustomMenu.Editor.MenuItems.MethodExecution.Helpers
{
    public static class DebugSymbolHandler
    {
        private const string DebugDefineSymbol = "IS_DEBUG";

        public const string EnableDebugSymbolKey = "EnableDebugSymbol";

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorApplication.delayCall += SyncDebugSymbolWithPrefs;
        }

        public static void ToggleDebugSymbol()
        {
            var isEnabled = EditorPrefs.GetBool(EnableDebugSymbolKey, false);
            isEnabled = isEnabled is false;
            EditorPrefs.SetBool(EnableDebugSymbolKey, isEnabled);

            if (isEnabled)
            {
                AddDefineSymbol(DebugDefineSymbol);
                Debug.Log("[DebugSymbolHandler::ToggleDebugSymbol] IS_DEBUG symbol enabled");
            }
            else
            {
                RemoveDefineSymbol(DebugDefineSymbol);
                Debug.Log("[DebugSymbolHandler::ToggleDebugSymbol] IS_DEBUG symbol disabled");
            }
        }

        public static bool IsDebugSymbolEnabled() => EditorPrefs.GetBool(EnableDebugSymbolKey, false);

        private static void AddDefineSymbol(string symbolToAdd)
        {
            var currentBuildTarget = NamedBuildTarget.FromBuildTargetGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup);

            var currentDefines = PlayerSettings.GetScriptingDefineSymbols(currentBuildTarget);

            if (currentDefines.Contains(symbolToAdd))
                return;

            var updatedDefines = string.IsNullOrEmpty(currentDefines)
                ? symbolToAdd
                : currentDefines + ";" + symbolToAdd;

            PlayerSettings.SetScriptingDefineSymbols(currentBuildTarget, updatedDefines);
        }

        private static void RemoveDefineSymbol(string symbolToRemove)
        {
            var currentBuildTarget = NamedBuildTarget.FromBuildTargetGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup);

            var currentDefines = PlayerSettings.GetScriptingDefineSymbols(currentBuildTarget);

            if (currentDefines.Contains(symbolToRemove) is false)
                return;

            var definesList = currentDefines.Split(';');

            var updatedDefines =
                string.Join(";", definesList.Where(defineSymbol => defineSymbol != symbolToRemove));

            PlayerSettings.SetScriptingDefineSymbols(currentBuildTarget, updatedDefines);
        }

        private static void SyncDebugSymbolWithPrefs()
        {
            var isEnabled = EditorPrefs.GetBool(EnableDebugSymbolKey, false);
            var currentBuildTarget = NamedBuildTarget.FromBuildTargetGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup);
            var currentDefines = PlayerSettings.GetScriptingDefineSymbols(currentBuildTarget);
            var symbolDefined = currentDefines.Contains(DebugDefineSymbol);

            switch (isEnabled)
            {
                case true when symbolDefined is false:
                    AddDefineSymbol(DebugDefineSymbol);
                    break;

                case false when symbolDefined:
                    RemoveDefineSymbol(DebugDefineSymbol);
                    break;
            }
        }
    }
}