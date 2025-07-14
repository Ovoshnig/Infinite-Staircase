using System;
using UnityEngine;

[Serializable]
public class KeyBindingSettings
{
    [field: SerializeField] public Color NormalTextColor { get; private set; } = Color.black;
    [field: SerializeField] public Color WaitingTextColor { get; private set; } = Color.orangeRed;
}
