using R3;
using System;
using UnityEngine.EventSystems;
using VContainer.Unity;

public class SlotMediator : IInitializable, IDisposable
{
    private readonly Slot _slot;
    private readonly SlotView _slotView;
    private readonly Inventory _inventory;
    private readonly CompositeDisposable _compositeDisposable = new();

    public SlotMediator(Slot slot, SlotView slotView, Inventory inventory)
    {
        _slot = slot;
        _slotView = slotView;
        _inventory = inventory;
    }

    public void Initialize() => Subscribe();

    public void Dispose() => Unsubscribe();

    public void Subscribe()
    {
        _slotView.PointerDown += OnPointerDown;
        _slotView.PointerUp += OnPointerUp;
        _slotView.PointerEntered += OnPointerEntered;
        _slotView.PointerExited += OnPointerExited;

        _slot.ItemData
            .Subscribe(OnItemChanged)
            .AddTo(_compositeDisposable);
    }

    public void Unsubscribe()
    {
        _slotView.PointerDown -= OnPointerDown;
        _slotView.PointerUp -= OnPointerUp;
        _slotView.PointerEntered -= OnPointerEntered;
        _slotView.PointerExited -= OnPointerExited;

        _compositeDisposable?.Dispose();
    }

    private void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            _inventory.BeginDrag(_slot);
        else if (eventData.button == PointerEventData.InputButton.Right)
            _inventory.TryRemoveItem(_slot);
    }

    private void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            _inventory.Drop();
    }

    private void OnPointerEntered() => _inventory.SelectSlot(_slot);

    private void OnPointerExited() => _inventory.DeselectSlot(_slot);

    private void OnItemChanged(ItemData itemData)
    {
        if (itemData == null)
            _slotView.HideItem();
        else
            _slotView.DisplayItem(itemData);
    }
}
