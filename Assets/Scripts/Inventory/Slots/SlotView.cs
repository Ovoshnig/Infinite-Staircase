using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image),
                  typeof(Slot))]
public class SlotView : MonoBehaviour
{
    private Slot _slot;
    private Image _image;
    private Sprite _emptySprite;

    private void Awake()
    {
        _slot = GetComponent<Slot>();
        _image = GetComponent<Image>();
        _emptySprite = _image.sprite;
    }

    private void OnEnable()
    {
        _slot.ItemTaken += OnItemTaken;
        _slot.ItemPlaced += OnItemPlaced;
    }

    private void OnDisable()
    {
        _slot.ItemTaken -= OnItemTaken;
        _slot.ItemPlaced -= OnItemPlaced;
    }

    private void OnItemTaken(Item item)
    {
        _image.sprite = _emptySprite;
    }

    private void OnItemPlaced(Item item)
    {
        _image.sprite = _emptySprite;
    }
}
