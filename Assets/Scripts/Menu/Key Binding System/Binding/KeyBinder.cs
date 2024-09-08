using R3;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;
using Zenject;

public class KeyBinder : MonoBehaviour
{
    [SerializeField] private Button _bindingButton;
    [SerializeField] private Button _bindingResetButton;
    [SerializeField] private TMP_Text _actionNameText;
    [SerializeField] private Color _waitingTextColor;
    [SerializeField] private InputAction _inputAction;

    private KeyBindingsTracker _bindingsTracker;
    private TMP_Text _bindingButtonText;
    private Color _normalTextColor;
    private InputAction _anyKeyInputAction;
    private IDisposable _disposable;

    [Inject]
    private void Construct(KeyBindingsTracker bindingsTracker) => _bindingsTracker = bindingsTracker;

    public InputAction InputAction
    {
        get => _inputAction;
        set => _inputAction = value;
    }

    public TMP_Text ActionNameText => _actionNameText;

    private void Awake()
    {
        _bindingButtonText = _bindingButton.GetComponentInChildren<TMP_Text>();
        _bindingButtonText.text = _inputAction.GetBindingDisplayString();
        _normalTextColor = _bindingButtonText.color;

        _anyKeyInputAction = new InputAction(type: InputActionType.PassThrough);
        _anyKeyInputAction.AddBinding(InputConstants.KeyboardAnyKeyPath);
        _anyKeyInputAction.performed += OnKeyPressed;

        _bindingButton.onClick.AddListener(OnBindingButtonPressed);
        _bindingResetButton.onClick.AddListener(OnBindingResetButtonClicked);

        _disposable = R3.Observable
            .EveryUpdate()
            .Select(_ => _inputAction.bindings[0].hasOverrides)
            .DistinctUntilChanged()
            .Subscribe(value => _bindingResetButton.interactable = value);
    }

    private void OnDestroy()
    {
        _bindingButton.onClick.RemoveListener(OnBindingButtonPressed);
        _bindingResetButton.onClick.RemoveListener(OnBindingResetButtonClicked);

        _disposable?.Dispose();
    }

    private void OnBindingButtonPressed()
    {
        if (!_bindingsTracker.TryStartListening())
            return;

        _anyKeyInputAction.Enable();
        _bindingButtonText.color = _waitingTextColor;
        _bindingButtonText.text = "ќжидание ввода...";
    }

    private void OnBindingResetButtonClicked()
    {
        _inputAction.RemoveBindingOverride(0);
        _bindingButtonText.text = _inputAction.GetBindingDisplayString();
    }

    private void CancelKeyBinding() => _bindingButtonText.text = _inputAction.GetBindingDisplayString();

    private void OnKeyPressed(InputAction.CallbackContext _)
    {
        var pressedControl = _bindingsTracker.AllControls.First(c => c.IsPressed());

        if (pressedControl == Keyboard.current.escapeKey)
        {
            CancelKeyBinding();
        }
        else
        {
            _bindingButtonText.text = pressedControl.displayName;
            _inputAction.ApplyBindingOverride(pressedControl.path);
        }

        _anyKeyInputAction.Disable();
        _bindingButtonText.color = _normalTextColor;

        _bindingsTracker.StopListening();
    }
}
