using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

public class InventoryMediator : IInitializable, ITickable, IDisposable
{
    private readonly InventoryView _inventoryView;
    private readonly Inventory _inventory;
    private readonly InventorySaver _inventorySaver;
    private readonly InventorySettings _inventorySettings;
    private readonly CancellationTokenSource _cts = new();

    private SlotMediator[] _slotMediators;
    private ItemView _draggedItemView;
    private Transform _draggedItemParentTransform;

    public InventoryMediator(InventoryView inventoryView, Inventory inventory, 
        InventorySaver inventorySaver, InventorySettings inventorySettings)
    {
        _inventoryView = inventoryView;
        _inventory = inventory;
        _inventorySaver = inventorySaver;
        _inventorySettings = inventorySettings;
    }

    public async void Initialize()
    {
        _slotMediators = new SlotMediator[_inventorySettings.SlotCount];

        for (int i = 0; i < _inventorySettings.SlotCount; i++)
        {
            SlotMediator slotMediator = new(_inventory.GetSlot(i), _inventoryView.SlotViews[i], _inventory);
            slotMediator.Initialize();
            _slotMediators[i] = slotMediator;
        }

        _inventory.DragStarted += OnDragStarted;
        _inventory.DragEnded += OnDragEnded;

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
        if (_inventory.IsDragging && _draggedItemView != null)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _inventoryView.CanvasRectTransform,
                Mouse.current.position.ReadValue(),
                null,
                out var localPoint);

            _draggedItemView.SetAnchoredPosition(localPoint);
        }
    }

    public void Dispose()
    {
        _cts?.Cancel();
        _cts?.Dispose();

        for (int i = 0; i < _inventorySettings.SlotCount; i++)
            _slotMediators[i].Dispose();

        _inventory.DragStarted -= OnDragStarted;
        _inventory.DragEnded -= OnDragEnded;

        _inventorySaver.SaveInventory(_inventory);
    }

    private void OnDragStarted(ItemData itemData)
    {
        for (int i = 0; i < _inventorySettings.SlotCount; i++)
        {
            if (_inventory.GetSlot(i) == _inventory.DraggingSlot)
            {
                _draggedItemView = _inventoryView.SlotViews[i].ItemView;
                break;
            }
        }

        if (_draggedItemView != null)
        {
            _draggedItemParentTransform = _draggedItemView.transform.parent;
            _draggedItemView.transform.SetParent(_inventoryView.CanvasRectTransform, true);
            _draggedItemView.transform.SetAsLastSibling();
            _draggedItemView.SetDraggingState(true);
        }
    }

    private void OnDragEnded()
    {
        if (_draggedItemView != null)
        {
            _draggedItemView.transform.SetParent(_draggedItemParentTransform, false);
            _draggedItemView.SetDraggingState(false);
            _draggedItemView = null;
        }
    }
}
