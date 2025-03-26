using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

public class SlotView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private InventoryView _inventoryView;
    [SerializeField] private GameObject _itemPrefab;

    private readonly Vector2 _halfVector2 = Vector2.one / 2f;
    private RectTransform _storedItem;
    private DraggedItemHolder _draggedItemHolder;
    private SlotModel _slotModel;

    [Inject]
    public void Construct(DraggedItemHolder draggedItemHolder)
    {
        _draggedItemHolder = draggedItemHolder;

        _slotModel = new SlotModel();
    }

    public SlotModel SlotModel => _slotModel;

    private Transform CanvasTransform => transform.parent.parent.parent;

    public void OnPointerEnter(PointerEventData _) => _inventoryView.SelectedSlot = this;

    public void OnPointerExit(PointerEventData _) => _inventoryView.SelectedSlot = null;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        TakeItem();
    }

    public void PlaceItem()
    {
        if (_draggedItemHolder.DraggedItem != null)
        {
            _storedItem = _draggedItemHolder.DraggedItem;
            _slotModel.PlaceItem(_storedItem.GetComponent<ItemView>().ItemModel);
            _draggedItemHolder.DraggedItem = null;
            InitializeStoredItem();
        }
    }

    public void PlaceItem(ItemModel itemModel)
    {
        GameObject itemObject = InstantiateItem(itemModel);
        _storedItem = itemObject.GetComponent<RectTransform>();
        _slotModel.PlaceItem(itemModel);
        InitializeStoredItem();
    }

    public void TakeItem()
    {
        if (_storedItem != null)
        {
            _storedItem.transform.SetParent(CanvasTransform);
            _draggedItemHolder.DraggedItem = _storedItem;
            _slotModel.TakeItem();
            _storedItem = null;
            _inventoryView.StartingSlot = this;
        }
    }

    public SlotData Save() => _slotModel.Save();

    public async UniTask LoadAsync(SlotData slotData, ItemDataRepository itemDataRepository)
    {
        await _slotModel.LoadAsync(slotData, itemDataRepository);

        if (_slotModel.HasItem)
        {
            GameObject itemObject = InstantiateItem(_slotModel.ItemModel);
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

    private void InitializeStoredItem()
    {
        _storedItem.transform.SetParent(transform);
        _storedItem.anchorMax = _halfVector2;
        _storedItem.anchorMin = _halfVector2;
        _storedItem.anchoredPosition = Vector2.zero;
    }
}
