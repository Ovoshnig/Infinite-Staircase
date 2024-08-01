using System;
using UnityEngine.InputSystem;
using Zenject;

public class SlotKeeper : IInitializable, IDisposable
{
    private const string MouseBinding = "<Mouse>/leftButton";

    private readonly InputAction _mouseClickAction = new(type: InputActionType.Button, binding: MouseBinding);
    
    public Slot StartingSlot { get; set; }

    public Slot SelectedSlot { get; set; }

    public void Initialize()
    {
        _mouseClickAction.canceled += OnMouseClickCanceled;
        _mouseClickAction.Enable();
    }

    public void Dispose() => _mouseClickAction.Disable();

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
            if (SelectedSlot.HasItem)
                StartingSlot.PlaceItem();
            else
                SelectedSlot.PlaceItem();
        }

        StartingSlot = null;
    }
}
