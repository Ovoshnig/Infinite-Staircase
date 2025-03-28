using R3;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using VContainer;

public class InputAxisController : InputAxisControllerBase<InputAxisController.Reader>
{
    private LookTuner _lookTuner;
    private WindowTracker _windowTracker;
    private InputActionMap _actionMap;

    [Inject]
    public void Construct(LookTuner lookTuner, WindowTracker windowTracker)
    {
        _lookTuner = lookTuner;
        _windowTracker = windowTracker;
    }

    private void Awake()
    {
        PlayerInput playerInput = new();
        PlayerInput.PlayerActions playerActions = playerInput.Player;
        _actionMap = InputSystem.actions.FindActionMap(nameof(playerInput.Player));

        InputAction lookAction = _actionMap.FindAction(nameof(playerActions.Look));
        InputAction zoomAction = _actionMap.FindAction(nameof(playerActions.Zoom));

        lookAction.performed += OnLook;
        lookAction.canceled += OnLook;
        zoomAction.performed += OnZoom;
        zoomAction.canceled += OnZoom;
    }

    private void Start()
    {
        _lookTuner.Sensitivity
            .Subscribe(value => Controllers.ForEach(controller => controller.Input.Multiplier = value))
            .AddTo(this);
        _windowTracker.IsOpen
            .Subscribe(isOpen =>
            {
                if (isOpen)
                    _actionMap.Disable();
                else
                    _actionMap.Enable();
            })
            .AddTo(this);
    }

    private void OnDestroy() => _actionMap.Dispose();

    private void Update()
    {
        if (Application.isPlaying)
            UpdateControllers();
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        foreach (Controller controller in Controllers)
        {
            if (controller.Name != CinemachineInputConstants.OrbitScaleControllerName)
                controller.Input.ProcessInput(context.action);
        }
    }

    private void OnZoom(InputAction.CallbackContext context)
    {
        Controller orbitScaleController = Controllers
            .FirstOrDefault(c => c.Name == CinemachineInputConstants.OrbitScaleControllerName);

        if (orbitScaleController != default)
            orbitScaleController.Input.ProcessInput(context.action);
    }

    [Serializable]
    public class Reader : IInputAxisReader
    {
        [SerializeField] private InputActionReference _input;
        [SerializeField] private bool _invert = false;

        private Vector2 _value;
        public float Multiplier { get; set; } = 1f;

        public void ProcessInput(InputAction action)
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

        public float GetValue(UnityEngine.Object context, IInputAxisOwner.AxisDescriptor.Hints hint) => 
            hint == IInputAxisOwner.AxisDescriptor.Hints.Y ? _value.y : _value.x;
    }
}
