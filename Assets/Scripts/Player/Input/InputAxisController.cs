using R3;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using VContainer;

class InputAxisController : InputAxisControllerBase<InputAxisController.Reader>
{
    [Header("Input Source Override")]
    [SerializeField] private UnityEngine.InputSystem.PlayerInput _playerInput;

    private LookTuner _lookTuner;
    private WindowTracker _windowTracker;
    private readonly CompositeDisposable _compositeDisposable = new();

    [Inject]
    public void Construct(LookTuner lookTuner, WindowTracker windowTracker)
    {
        _lookTuner = lookTuner;
        _windowTracker = windowTracker;
    }

    private void Start()
    {
        if (_playerInput == null)
        {
            TryGetComponent(out _playerInput);
        }
        if (_playerInput == null)
        {
            Debug.LogError("Cannot find PlayerInput component");
        }
        else
        {
            _playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
            _playerInput.onActionTriggered += (value) =>
            {
                for (var i = 0; i < Controllers.Count; i++)
                    Controllers[i].Input.ProcessInput(value.action);
            };
        }

        _lookTuner.Sensitivity
            .Subscribe(value => Controllers.ForEach(controller => controller.Input.Multiplier = value))
            .AddTo(_compositeDisposable);

        _windowTracker.IsOpen
            .Subscribe(value => _playerInput.enabled = !value)
            .AddTo(_compositeDisposable);
    }

    private void OnDestroy() => _compositeDisposable?.Dispose();

    private void Update()
    {
        if (Application.isPlaying)
            UpdateControllers();
    }

    [Serializable]
    public class Reader : IInputAxisReader
    {
        [SerializeField] private InputActionReference _input;
        [SerializeField] private bool _invert = false;

        private Vector2 m_Value;

        public float Multiplier { get; set; } = 1f;

        public void ProcessInput(InputAction action)
        {
            if (_input != null && _input.action.id == action.id)
            {
                if (action.expectedControlType == nameof(Vector2))
                    m_Value = action.ReadValue<Vector2>();
                else
                    m_Value.x = m_Value.y = action.ReadValue<float>();

                int sign = _invert ? -1 : 1;
                m_Value *= sign * Multiplier;
            }
        }

        public float GetValue(UnityEngine.Object context, IInputAxisOwner.AxisDescriptor.Hints hint) => 
            (hint == IInputAxisOwner.AxisDescriptor.Hints.Y ? m_Value.y : m_Value.x);
    }
}
