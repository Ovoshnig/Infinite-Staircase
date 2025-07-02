using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

public class InventoryMediator : IInitializable, ITickable, IDisposable
{
    private readonly InventoryView _inventoryView;
    private readonly Inventory _inventory;
    private readonly InventorySaver _inventorySaver;
    private readonly InventorySettings _inventorySettings;

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

    public void Initialize()
    {
        _slotMediators = new SlotMediator[_inventorySettings.SlotCount];

        for (int i = 0; i < _inventorySettings.SlotCount; i++)
            _slotMediators[i] = new SlotMediator(_inventory.GetSlot(i), _inventoryView.SlotViews[i], _inventory);

        _inventory.OnDragStarted += HandleDragStarted;
        _inventory.OnDragEnded += HandleDragEnded;

        _inventorySaver.LoadInventoryAsync(_inventory).Forget();
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
        _inventory.OnDragStarted -= HandleDragStarted;
        _inventory.OnDragEnded -= HandleDragEnded;

        _inventorySaver.SaveInventory(_inventory);
    }

    private void HandleDragStarted(ItemData itemData)
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

    private void HandleDragEnded()
    {
        if (_draggedItemView != null)
        {
            _draggedItemView.transform.SetParent(_draggedItemParentTransform, false);
            _draggedItemView.SetDraggingState(false);
            _draggedItemView = null;
        }
    }
}
