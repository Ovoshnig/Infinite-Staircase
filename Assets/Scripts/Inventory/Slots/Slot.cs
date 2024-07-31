using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private RectTransform _storedItem;
    private DraggedItemHolder _draggedItemHolder;
    private SlotKeeper _slotKeeper;

    public event Action<RectTransform> ItemPlaced;
    public event Action<RectTransform> ItemTaken;

    [Inject]
    private void Construct(DraggedItemHolder draggedItemHolder, SlotKeeper slotKeeper)
    {
        _draggedItemHolder = draggedItemHolder;
        _slotKeeper = slotKeeper;
    }

    public bool HasItem { get; private set; } = false;

    private void Awake()
    {
        if (transform.childCount > 0)
        {
            _storedItem = transform.GetChild(0).GetComponent<RectTransform>();
            HasItem = true;
        }
        else
        {
            _storedItem = null;
            HasItem = false;
        }
    }

    public void OnPointerEnter(PointerEventData _) => _slotKeeper.SelectedSlot = this;

    public void OnPointerExit(PointerEventData _) => _slotKeeper.SelectedSlot = null;

    public void OnPointerDown(PointerEventData _) => TakeItem();

    public void PlaceItem()
    {
        if (_draggedItemHolder.DraggedItem != null)
        {
            _storedItem = _draggedItemHolder.DraggedItem;
            _storedItem.transform.SetParent(transform);
            _storedItem.anchoredPosition = Vector2.zero;
            HasItem = true;
            ItemPlaced?.Invoke(_storedItem);
        }
    }

    public void TakeItem()
    {
        if (_storedItem != null)
        {
            _storedItem.transform.SetParent(transform.parent);
            HasItem = false;
            ItemTaken?.Invoke(_storedItem);
            _storedItem = null;
            _slotKeeper.StartingSlot = this;
        }
    }
}
