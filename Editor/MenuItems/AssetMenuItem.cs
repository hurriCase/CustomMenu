using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CustomMenu.CustomMenu.Editor.MenuItems
{
    [Serializable]
    internal sealed class AssetMenuItem
    {
        [field: SerializeField] internal string MenuPath { get; private set; } = "Project/Assets";
        [field: SerializeField] internal Object Asset { get; private set; }
        [field: SerializeField] internal int Priority { get; private set; }
    }
}