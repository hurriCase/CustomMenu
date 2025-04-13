using System;
using UnityEngine;

namespace CustomMenu.Editor.MenuItems
{
    [Serializable]
    internal abstract class BaseMenuItem<T>
    {
        [field: SerializeField] internal T MenuTarget { get; private set; }
        [field: SerializeField] internal string MenuPath { get; private set; }
        [field: SerializeField] internal int Priority { get; private set; }
    }
}