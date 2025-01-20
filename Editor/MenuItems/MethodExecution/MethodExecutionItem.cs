using System;
using UnityEngine;

namespace CustomMenu.Editor.MenuItems.MethodExecution
{
    [Serializable]
    internal sealed class MethodExecutionItem
    {
        [field: SerializeField] internal string MenuPath { get; private set; } = "Project/MethodExecution";
        [field: SerializeField] internal MethodExecutionType MethodExecutionType { get; private set; }
        [field: SerializeField] internal int Priority { get; private set; }
    }
}