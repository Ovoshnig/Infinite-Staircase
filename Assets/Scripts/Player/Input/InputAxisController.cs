using R3;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using VContainer;

public class InputAxisController : InputAxisControllerBase<InputAxisController.Reader>
{
    private readonly CompositeDisposable _compositeDisposable = new();
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

        _actionMap.FindAction(nameof(playerActions.Look)).performed += OnActionTriggered;
        _actionMap.FindAction(nameof(playerActions.Look)).canceled += OnActionTriggered;
    }

    private void Start()
    {
        _lookTuner.Sensitivity
            .Subscribe(value => Controllers.ForEach(controller => controller.Input.Multiplier = value))
            .AddTo(_compositeDisposable);

        _windowTracker.IsOpen
            .Subscribe(isOpen =>
            {
                if (isOpen)
                    _actionMap.Disable();
                else
                    _actionMap.Enable();
            })
            .AddTo(_compositeDisposable);
    }

    private void OnActionTriggered(InputAction.CallbackContext context)
    {
        foreach (var controller in Controllers)
            controller.Input.ProcessInput(context.action);
    }

    private void Update()
    {
        if (Application.isPlaying)
            UpdateControllers();
    }

    private void OnDestroy()
    {
        _actionMap.Disable();

        _compositeDisposable.Dispose();
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

                float sign = _invert ? -1f : 1f;
                _value *= sign * Multiplier;
            }
        }

        public float GetValue(UnityEngine.Object context, IInputAxisOwner.AxisDescriptor.Hints hint) => 
            hint == IInputAxisOwner.AxisDescriptor.Hints.Y ? _value.y : _value.x;
    }
}
