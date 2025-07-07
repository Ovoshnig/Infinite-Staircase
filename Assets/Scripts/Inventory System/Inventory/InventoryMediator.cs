using R3;
using System;
using System.Threading;
using UnityEngine;
using VContainer.Unity;

public class InventoryMediator : Mediator, ITickable
{
    private readonly InventoryView _inventoryView;
    private readonly Inventory _inventory;
    private readonly InventorySaver _inventorySaver;
    private readonly InventorySettings _inventorySettings;
    private readonly CancellationTokenSource _cts = new();

    private SlotMediator[] _slotMediators;

    public InventoryMediator(InventoryView inventoryView, Inventory inventory,
        InventorySaver inventorySaver, InventorySettings inventorySettings)
    {
        _inventoryView = inventoryView;
        _inventory = inventory;
        _inventorySaver = inventorySaver;
        _inventorySettings = inventorySettings;
    }

    public override async void Initialize()
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

        try
        {
            await _inventorySaver.LoadInventoryAsync(_inventory, _cts.Token);
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    public void Tick()
    {
        if (_inventory.IsDragging)
            _inventoryView.MoveItemToMouse();
    }

    public override void Dispose()
    {
        base.Dispose();
        
        _cts?.Cancel();
        _cts?.Dispose();

        for (int i = 0; i < _inventorySettings.SlotCount; i++)
            _slotMediators[i].Dispose();

        _inventorySaver.SaveInventory(_inventory);
    }
}
