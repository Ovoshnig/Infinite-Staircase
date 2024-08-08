using UnityEngine;

public class ItemModel
{
    public string Name { get; private set; }
    public Sprite Icon { get; private set; }

    public ItemModel(string name, Sprite icon)
    {
        Name = name;
        Icon = icon;
    }
}
