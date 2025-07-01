using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, 
    IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private ItemView _itemView;
    [SerializeField] private RectTransform _itemParent;
    [SerializeField] private float _itemPadding = 0f;

    public event Action<PointerEventData> OnPointerDownEvent;
    public event Action<PointerEventData> OnPointerUpEvent;
    public event Action OnPointerEnterEvent;
    public event Action OnPointerExitEvent;

    public ItemView ItemView => _itemView;

    private void Awake()
    {
        if (_itemView != null)
            _itemView.Clear();
    }

    public void OnPointerDown(PointerEventData eventData) => OnPointerDownEvent?.Invoke(eventData);

    public void OnPointerUp(PointerEventData eventData) => OnPointerUpEvent?.Invoke(eventData);

    public void OnPointerEnter(PointerEventData eventData) => OnPointerEnterEvent?.Invoke();

    public void OnPointerExit(PointerEventData eventData) => OnPointerExitEvent?.Invoke();

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
