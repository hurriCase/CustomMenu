using System;
using UnityEditor;
using UnityEngine;

namespace CustomMenu.Editor.MenuItems
{
    [Serializable]
    internal sealed class SceneMenuItem
    {
        [field: SerializeField] internal string MenuPath { get; private set; } = "Project/Scenes";
        [field: SerializeField] internal SceneAsset Scene { get; private set; }
        internal string ScenePath => AssetDatabase.GetAssetPath(Scene);
        internal string SceneName => Scene.name;
        [field: SerializeField] internal int Priority { get; private set; }
    }
}