using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ItemView : MonoBehaviour
{
    private Image _image;

    public ItemModel ItemModel { get; private set; }

    private void Awake() => _image = GetComponent<Image>();

    public void Initialize(ItemModel itemModel)
    {
        _image.sprite = itemModel.Icon;
        ItemModel = new ItemModel(itemModel.Name, itemModel.Icon);
    }
}
