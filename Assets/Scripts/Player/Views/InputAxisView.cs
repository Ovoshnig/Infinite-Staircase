using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using VContainer;

public abstract class InputAxisView : InputAxisControllerBase<InputAxisView.Reader>
{
    private InputActions.PlayerActions _playerActions;
    private PlayerSettings _playerSettings;

    [Inject]
    public void Construct(InputActions inputActions, PlayerSettings playerSettings)
    {
        _playerActions = inputActions.Player;
        _playerSettings = playerSettings;
    }

    private void Start()
    {
        _playerActions.Enable();

        _playerActions.Look.Subscribe(OnLook);
        _playerActions.Zoom.Subscribe(OnZoom);
    }

    private void OnDestroy()
    {
        _playerActions.Disable();

        _playerActions.Look.Unsubscribe(OnLook);
        _playerActions.Zoom.Unsubscribe(OnZoom);
    }

    private void Update()
    {
        if (Application.isPlaying)
            UpdateControllers();
    }

    public void SetControllersMultiplier(float value)
    {
        foreach (var controller in Controllers)
            controller.Input.Multiplier = value;
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        foreach (Controller controller in Controllers)
        {
            if (controller.Name != CinemachineInputConstants.OrbitScaleControllerName)
                controller.Input.ProcessLookInput(context.action);
        }
    }

    private void OnZoom(InputAction.CallbackContext context)
    {
        Controller orbitScaleController = Controllers
            .FirstOrDefault(c => c.Name == CinemachineInputConstants.OrbitScaleControllerName);

        if (orbitScaleController != default)
            orbitScaleController.Input.ProcessZoomInput(context.action, _playerSettings.ZoomMultiplier);
    }

    [Serializable]
    public class Reader : IInputAxisReader
    {
        [SerializeField] private InputActionReference _input;
        [SerializeField] private bool _invert = false;

        private Vector2 _value;
        public float Multiplier { get; set; } = 1f;

        public void ProcessLookInput(InputAction action)
        {
            if (_input != null && _input.action.id == action.id)
            {
                _value = action.expectedControlType == nameof(Vector2)
                    ? action.ReadValue<Vector2>()
                    : new Vector2(action.ReadValue<float>(), action.ReadValue<float>());

                int sign = _invert ? -1 : 1;
                _value *= sign;
                _value *= Multiplier;
            }
        }

        public void ProcessZoomInput(InputAction action, float zoomMultiplier)
        {
            if (_input != null && _input.action.id == action.id)
            {
                _value = action.expectedControlType == nameof(Vector2)
                    ? action.ReadValue<Vector2>()
                    : new Vector2(action.ReadValue<float>(), action.ReadValue<float>());

                int sign = _invert ? -1 : 1;
                _value *= sign;
                _value *= zoomMultiplier;
            }
        }

        public float GetValue(UnityEngine.Object context, IInputAxisOwner.AxisDescriptor.Hints hint) => 
            hint == IInputAxisOwner.AxisDescriptor.Hints.Y ? _value.y : _value.x;
    }
}
