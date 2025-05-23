using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemDataSO : ScriptableObject
{
    [field: SerializeField] public Sprite Icon { get; private set; }

    public string Name { get => name; }
}
