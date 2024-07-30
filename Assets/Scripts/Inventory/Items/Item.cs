using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Item : MonoBehaviour
{
    [SerializeField] private ItemData _itemData;

    private Image _image;

    private void OnValidate()
    {
        if (_image == null)
            _image = GetComponent<Image>();

        if (_itemData != null)
            _image.sprite = _itemData.Icon;
    }

    /*private void Awake() => _image = GetComponent<Image>();

    private void Start()
    {
        _image.sprite = _itemData.Sprite;
    }*/
}
