using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InputAxisView : InputAxisControllerBase<InputAxisView.Reader>
{
    private void Update()
    {
        if (!Application.isPlaying)
            return;

        UpdateControllers();
    }

    public void SetLookControllersGain(float value)
    {
        foreach (var controller in Controllers)
            if (controller.Input.InputAction.id == PlayerInputConstants.LookActionId)
                controller.Input.SetGain(value);
    }

    public void ProcessInput(InputAction action)
    {
        foreach (var controller in Controllers)
        {
            if (controller.Input.InputAction.id == action.id)
                controller.Input.ProcessInput(action);
        }
    }

    [Serializable]
    public class Reader : IInputAxisReader
    {
        [SerializeField] private InputActionReference _actionReference;
        [SerializeField] private float _gain = 1f;
        [SerializeField] private bool _invert = false;

        private Vector2 _value;

        public InputAction InputAction => _actionReference.action;

        public void ProcessInput(InputAction action)
        {
            if (_actionReference != null && _actionReference.action.id == action.id)
            {
                _value = action.expectedControlType == nameof(Vector2)
                    ? action.ReadValue<Vector2>()
                    : new Vector2(action.ReadValue<float>(), action.ReadValue<float>());

                int sign = _invert ? -1 : 1;
                _value *= sign;
                _value *= _gain;
            }
        }

        public float GetValue(UnityEngine.Object context, IInputAxisOwner.AxisDescriptor.Hints hint) =>
            hint == IInputAxisOwner.AxisDescriptor.Hints.Y ? _value.y : _value.x;

        public void SetGain(float value) => _gain = value;
    }
}
