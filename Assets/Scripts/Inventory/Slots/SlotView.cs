using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class SlotView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private GameObject _itemPrefab;

    private readonly Vector2 _halfVector2 = Vector2.one / 2f;
    private RectTransform _storedItem;
    private DraggedItemHolder _draggedItemHolder;
    private SlotKeeper _slotKeeper;
    private SlotModel _slotModel;

    [Inject]
    private void Construct(DraggedItemHolder draggedItemHolder, SlotKeeper slotKeeper)
    {
        _draggedItemHolder = draggedItemHolder;
        _slotKeeper = slotKeeper;
        _slotModel = new SlotModel();
    }

    public SlotModel SlotModel => _slotModel;

    private Transform CanvasTransform => transform.parent.parent;

    public void OnPointerEnter(PointerEventData _) => _slotKeeper.SelectedSlot = this;

    public void OnPointerExit(PointerEventData _) => _slotKeeper.SelectedSlot = null;

    public void OnPointerDown(PointerEventData _) => TakeItem();

    public void PlaceItem()
    {
        if (_draggedItemHolder.DraggedItem != null)
        {
            _storedItem = _draggedItemHolder.DraggedItem;
            _slotModel.PlaceItem(_storedItem.GetComponent<ItemView>().ItemModel);
            _draggedItemHolder.DraggedItem = null;
            _storedItem.transform.SetParent(transform);
            _storedItem.anchorMax = _halfVector2;
            _storedItem.anchorMin = _halfVector2;
            _storedItem.anchoredPosition = Vector2.zero;
        }
    }

    public void TakeItem()
    {
        if (_storedItem != null)
        {
            _storedItem.transform.SetParent(CanvasTransform);
            _draggedItemHolder.DraggedItem = _storedItem;
            _slotModel.TakeItem();
            _storedItem = null;
            _slotKeeper.StartingSlot = this;
        }
    }

    public SlotData Save() => _slotModel.Save();

    public void Load(SlotData data, ItemDataRepository itemDataRepository)
    {
        _slotModel.Load(data, itemDataRepository);

        if (_slotModel.HasItem)
        {
            GameObject itemObject = InstantiateItem(_slotModel.StoredItem);
            _storedItem = itemObject.GetComponent<RectTransform>();
            _storedItem.transform.SetParent(transform);
            _storedItem.anchorMax = _halfVector2;
            _storedItem.anchorMin = _halfVector2;
            _storedItem.anchoredPosition = Vector2.zero;

            _slotModel.PlaceItem(_storedItem.GetComponent<ItemView>().ItemModel);
        }
        else
        {
            _storedItem = null;
        }
    }

    private GameObject InstantiateItem(ItemModel itemModel)
    {
        GameObject itemObject = Instantiate(_itemPrefab);
        ItemView itemView = itemObject.GetComponent<ItemView>();
        itemView.Initialize(itemModel);
        return itemObject;
    }
}
