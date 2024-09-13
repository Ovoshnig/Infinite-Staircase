using R3;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using Zenject;

// This class receives input from a PlayerInput component and disptaches it
// to the appropriate Cinemachine InputAxis.  The playerInput component should
// be on the same GameObject, or specified in the PlayerInput field.
class InputAxisController : InputAxisControllerBase<InputAxisController.Reader>
{
    [Header("Input Source Override")]
    [SerializeField] private UnityEngine.InputSystem.PlayerInput _playerInput;

    private LookTuner _lookTuner;
    private WindowTracker _windowTracker;
    private CompositeDisposable _compositeDisposable;

    [Inject]
    private void Construct(LookTuner lookTuner, WindowTracker windowTracker)
    {
        _lookTuner = lookTuner;
        _windowTracker = windowTracker;
    }

    private void Awake()
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

        var sensitivityDisposable = _lookTuner.Sensitivity
            .Subscribe(value => Controllers.ForEach(controller => controller.Input.Multiplier = value));

        var windowDisposable = _windowTracker.IsOpen
            .Subscribe(value => _playerInput.enabled = !value);

        _compositeDisposable = new CompositeDisposable()
        {
            sensitivityDisposable,
            windowDisposable
        };
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
