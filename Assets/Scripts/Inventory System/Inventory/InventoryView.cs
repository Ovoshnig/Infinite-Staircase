using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InventoryView : MonoBehaviour
{
    private InputAction _mouseClickAction;
    private InventoryModel _inventoryModel;
    private SlotView[] _slotViews;
    private InventorySaver _inventorySaver;

    public SlotView StartingSlot { get; set; }
    public SlotView SelectedSlot { get; set; }

    [Inject]
    private void Construct(InventorySaver inventorySaver) => _inventorySaver = inventorySaver;

    private void Awake()
    {
        _mouseClickAction = new(type: InputActionType.Button,
        binding: InputConstants.MouseLeftButtonBinding);
        _mouseClickAction.canceled += OnMouseClickCanceled;

        _slotViews = GetComponentsInChildren<SlotView>();
        _inventorySaver.LoadSlots(_slotViews);
        SlotModel[] slotModels = _slotViews.Select(x => x.SlotModel).ToArray();
        _inventoryModel = new InventoryModel(slotModels);
    }

    private void OnEnable() => _mouseClickAction.Enable();

    private void OnDisable() => _mouseClickAction.Disable();

    private void OnDestroy()
    {
        SlotData[] slotDataArray = _inventoryModel.SaveSlots();
        _inventorySaver.SaveSlots(_slotViews, slotDataArray);
    }

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
            //Item use logic
            slotView.TakeItem();
        }
    }

    private void OnMouseClickCanceled(InputAction.CallbackContext _)
    {
        if (StartingSlot == null) return;

        if (SelectedSlot == null)
            StartingSlot.PlaceItem();
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
