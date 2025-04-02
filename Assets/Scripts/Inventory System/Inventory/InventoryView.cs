using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

public class InventoryView : MonoBehaviour
{
    private InputAction _mouseClickAction;
    private InventoryModel _inventoryModel;
    private SlotView[] _slotViews;
    private InventorySaver _inventorySaver;

    [Inject]
    public void Construct(InventorySaver inventorySaver) => _inventorySaver = inventorySaver;

    public SlotView StartingSlot { get; set; } = null;
    public SlotView SelectedSlot { get; set; } = null;

    private void Awake()
    {
        _mouseClickAction = new InputAction(
            type: InputActionType.Button, 
            binding: Mouse.current.leftButton.path);
        _mouseClickAction.canceled += OnMouseClickCanceled;

        _slotViews = GetComponentsInChildren<SlotView>();
    }

    private void OnEnable() => _mouseClickAction.Enable();

    private async void Start()
    {
        await _inventorySaver.LoadSlotsAsync(_slotViews);
        SlotModel[] slotModels = _slotViews.Select(x => x.SlotModel).ToArray();
        _inventoryModel = new InventoryModel(slotModels);
    }

    private void OnDisable()
    {
        _mouseClickAction.Disable();

        if (_inventoryModel == null)
            return;

        SlotData[] slotDataArray = _inventoryModel.SaveSlots();
        _inventorySaver.SaveSlots(_slotViews, slotDataArray);
    }

    private void OnDestroy() => _mouseClickAction.canceled -= OnMouseClickCanceled;

    public bool TryAddItem(ItemModel itemModel)
    {
        foreach (var slotView in _slotViews)
        {
            if (!slotView.SlotModel.HasItem)
            {
                slotView.PlaceItem(itemModel);

                return true;
            }
        }

        return false;
    }

    public void UseItem(SlotView slotView)
    {
        if (slotView.SlotModel.HasItem)
        {
            slotView.TakeItem();
        }
    }

    private void OnMouseClickCanceled(InputAction.CallbackContext _)
    {
        if (StartingSlot == null) 
            return;

        if (SelectedSlot == null)
        {
            StartingSlot.PlaceItem();
        }
        else
        {
            if (SelectedSlot.SlotModel.HasItem)
                StartingSlot.PlaceItem();
            else
                SelectedSlot.PlaceItem();
        }

        StartingSlot = null;
    }
}
