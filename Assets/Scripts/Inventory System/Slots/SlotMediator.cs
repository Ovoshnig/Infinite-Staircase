using UnityEngine.EventSystems;

public class SlotMediator
{
    private readonly Slot _slot;
    private readonly SlotView _slotView;
    private readonly Inventory _inventory;

    public SlotMediator(Slot slot, SlotView slotView, Inventory inventory)
    {
        _slot = slot;
        _slotView = slotView;
        _inventory = inventory;

        Subscribe();
        OnItemChanged(_slot.ItemData);
    }

    private void Subscribe()
    {
        _slotView.OnPointerDownEvent += HandlePointerDown;
        _slotView.OnPointerUpEvent += HandlePointerUp;
        _slotView.OnPointerEnterEvent += HandlePointerEnter;
        _slotView.OnPointerExitEvent += HandlePointerExit;

        _slot.OnItemChanged += OnItemChanged;
    }

    public void Unsubscribe() 
    {

    }

    private void HandlePointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            _inventory.BeginDrag(_slot);
        else if (eventData.button == PointerEventData.InputButton.Right)
            _inventory.TryRemoveItem(_slot);
    }

    private void HandlePointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            _inventory.Drop();
    }

    private void HandlePointerEnter() => _inventory.SelectSlot(_slot);

    private void HandlePointerExit() => _inventory.DeselectSlot(_slot);

    private void OnItemChanged(ItemData itemData)
    {
        if (itemData == null)
            _slotView.HideItem();
        else
            _slotView.DisplayItem(itemData);
    }
}
