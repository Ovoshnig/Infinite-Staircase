using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private readonly Vector2 _halfVector2 = Vector2.one / 2f;
    private RectTransform _storedItem;
    private DraggedItemHolder _draggedItemHolder;
    private SlotKeeper _slotKeeper;

    [Inject]
    private void Construct(DraggedItemHolder draggedItemHolder, SlotKeeper slotKeeper)
    {
        _draggedItemHolder = draggedItemHolder;
        _slotKeeper = slotKeeper;
    }

    public bool HasItem { get; private set; } = false;

    private Transform CanvasTransform => transform.parent.parent;

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
            _draggedItemHolder.DraggedItem = null;
            _storedItem.transform.SetParent(transform);
            _storedItem.anchorMax = _halfVector2;
            _storedItem.anchorMin = _halfVector2;
            _storedItem.anchoredPosition = Vector2.zero;
            HasItem = true;
        }
    }

    public void TakeItem()
    {
        if (_storedItem != null)
        {
            _storedItem.transform.SetParent(CanvasTransform);
            _draggedItemHolder.DraggedItem = _storedItem;
            HasItem = false;
            _storedItem = null;
            _slotKeeper.StartingSlot = this;
        }
    }
}
