using R3;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, 
    IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private ItemView _itemView;
    [SerializeField] private RectTransform _itemParent;
    [SerializeField] private float _itemPadding = 0f;

    private readonly Subject<PointerEventData> _pointerDown = new();
    private readonly Subject<PointerEventData> _pointerUp = new();
    private readonly Subject<PointerEventData> _pointerEntered = new();
    private readonly Subject<PointerEventData> _pointerExited = new();

    public ItemView ItemView => _itemView;
    public Observable<PointerEventData> PointerDown => _pointerDown;
    public Observable<PointerEventData> PointerUp => _pointerUp;
    public Observable<PointerEventData> PointerEntered => _pointerEntered;
    public Observable<PointerEventData> PointerExited => _pointerExited;

    private void Awake()
    {
        if (_itemView != null)
            _itemView.Clear();
    }

    public void OnPointerDown(PointerEventData eventData) => _pointerDown.OnNext(eventData);

    public void OnPointerUp(PointerEventData eventData) => _pointerUp.OnNext(eventData);

    public void OnPointerEnter(PointerEventData eventData) => _pointerEntered.OnNext(eventData);

    public void OnPointerExit(PointerEventData eventData) => _pointerExited.OnNext(eventData);

    public void SetItemPadding(float value) => _itemPadding = value;

    public void DisplayItem(ItemData itemData)
    {
        _itemView.transform.SetParent(_itemParent, false);
        _itemView.Render(itemData);

        ApplyItemLayout();
    }

    public void HideItem() => _itemView.Clear();

    private void ApplyItemLayout()
    {
        RectTransform itemRectTransform = _itemView.transform as RectTransform;

        itemRectTransform.localScale = Vector3.one;

        itemRectTransform.anchorMin = Vector2.zero;
        itemRectTransform.anchorMax = Vector2.one;
        itemRectTransform.offsetMin = new Vector2(_itemPadding, _itemPadding);
        itemRectTransform.offsetMax = new Vector2(-_itemPadding, -_itemPadding);
    }
}
