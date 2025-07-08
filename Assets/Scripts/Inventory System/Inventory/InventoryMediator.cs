using R3;
using VContainer.Unity;

public class InventoryMediator : Mediator, ITickable
{
    private readonly InventoryView _inventoryView;
    private readonly Inventory _inventory;
    private readonly InventorySettings _inventorySettings;

    private SlotMediator[] _slotMediators;

    public InventoryMediator(InventoryView inventoryView, Inventory inventory,
        InventorySettings inventorySettings)
    {
        _inventoryView = inventoryView;
        _inventory = inventory;
        _inventorySettings = inventorySettings;
    }

    public override void Initialize()
    {
        _slotMediators = new SlotMediator[_inventorySettings.SlotCount];

        for (int i = 0; i < _inventorySettings.SlotCount; i++)
        {
            SlotMediator slotMediator = new(_inventory.GetSlot(i), _inventoryView.SlotViews[i], _inventory);
            slotMediator.Initialize();
            _slotMediators[i] = slotMediator;
        }

        _inventory.DraggingSlot
            .Subscribe(value =>
            {
                if (value != null)
                {
                    int index = _inventory.DraggingSlotIndex;
                    _inventoryView.OnDragStarted(index);
                }
                else
                {
                    _inventoryView.OnDragEnded();
                }
            })
            .AddTo(CompositeDisposable);
    }

    public void Tick()
    {
        if (_inventory.IsDragging)
            _inventoryView.MoveItemToMouse();
    }

    public override void Dispose()
    {
        base.Dispose();
        
        for (int i = 0; i < _inventorySettings.SlotCount; i++)
            _slotMediators[i].Dispose();
    }
}
