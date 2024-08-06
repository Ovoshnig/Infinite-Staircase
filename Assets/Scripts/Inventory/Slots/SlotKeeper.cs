using System;
using UnityEngine.InputSystem;
using Zenject;

public class SlotKeeper : IInitializable, IDisposable
{
    private readonly InputAction _mouseClickAction = new(type: InputActionType.Button, 
        binding: InputConstants.MouseLeftButtonBinding);

    public SlotView StartingSlot { get; set; }
    public SlotView SelectedSlot { get; set; }

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
            if (SelectedSlot.SlotModel.HasItem)
                StartingSlot.PlaceItem();
            else
                SelectedSlot.PlaceItem();
        }

        StartingSlot = null;
    }
}
