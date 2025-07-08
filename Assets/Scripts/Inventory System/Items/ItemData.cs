using UnityEngine;

public class ItemData
{
    public string Name { get; }
    public Sprite Icon { get; }

    public ItemData(string name, Sprite icon)
    {
        Name = name;
        Icon = icon;
    }
}
