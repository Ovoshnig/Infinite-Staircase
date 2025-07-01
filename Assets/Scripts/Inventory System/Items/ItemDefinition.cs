using UnityEngine;

[CreateAssetMenu(fileName = "ItemDefinition", menuName = "Scriptable Objects/Item Definition")]
public class ItemDefinition : ScriptableObject
{
    [field: SerializeField] public Sprite Icon { get; private set; }
}
