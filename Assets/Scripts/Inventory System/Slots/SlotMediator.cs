using R3;
using UnityEngine.EventSystems;

public class SlotMediator : Mediator
{
    private readonly Slot _slot;
    private readonly SlotView _slotView;
    private readonly Inventory _inventory;

    public SlotMediator(Slot slot, SlotView slotView, Inventory inventory)
    {
        _slot = slot;
        _slotView = slotView;
        _inventory = inventory;
    }

    public override void Initialize()
    {
        _slotView.PointerDown
            .Subscribe(OnPointerDown)
            .AddTo(CompositeDisposable);
        _slotView.PointerUp
            .Subscribe(OnPointerUp)
            .AddTo(CompositeDisposable);
        _slotView.PointerEntered
            .Subscribe(OnPointerEntered)
            .AddTo(CompositeDisposable);
        _slotView.PointerExited
            .Subscribe(OnPointerExited)
            .AddTo(CompositeDisposable);

        _slot.ItemData
            .Subscribe(OnItemChanged)
            .AddTo(CompositeDisposable);
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
            _inventory.EndDrag();
    }

    private void OnPointerEntered(PointerEventData eventData) => _inventory.SelectSlot(_slot);

    private void OnPointerExited(PointerEventData eventData) => _inventory.DeselectSlot(_slot);

    private void OnItemChanged(ItemData itemData)
    {
        if (itemData == null)
            _slotView.HideItem();
        else
            _slotView.DisplayItem(itemData);
    }
}
