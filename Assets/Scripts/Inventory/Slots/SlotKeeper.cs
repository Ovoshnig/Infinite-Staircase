using System;
using UnityEngine.InputSystem;

public class SlotKeeper : IDisposable
{
    private const string MouseBinding = "<Mouse>/leftButton";

    private readonly InputAction _mouseClickAction;

    public SlotKeeper()
    {
        _mouseClickAction = new InputAction(type: InputActionType.Button, binding: MouseBinding);
        _mouseClickAction.canceled += OnMouseClickCanceled;
        _mouseClickAction.Enable();
    }

    public Slot StartingSlot { get; set; }

    public Slot SelectedSlot { get; set; }

    public void Dispose() => _mouseClickAction.Disable();

    private void OnMouseClickCanceled(InputAction.CallbackContext context)
    {
        if (StartingSlot == null)
            return;

        if (SelectedSlot == null)
        {
            StartingSlot.PlaceItem();
        }
        else
        {
            if (SelectedSlot.HasItem)
                StartingSlot.PlaceItem();
            else
                SelectedSlot.PlaceItem();
        }

        StartingSlot = null;
    }
}
