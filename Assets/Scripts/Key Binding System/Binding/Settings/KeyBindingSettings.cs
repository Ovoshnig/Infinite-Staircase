using UnityEngine;

[CreateAssetMenu(fileName = nameof(KeyBindingSettings), 
    menuName = "Scriptable Objects/Key Binding Settings")]
public class KeyBindingSettings : ScriptableObject
{
    [field: SerializeField] public Color NormalTextColor { get; private set; } = Color.black;
    [field: SerializeField] public Color WaitingTextColor { get; private set; } = Color.orangeRed;
}
