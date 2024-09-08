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
    [SerializeField] private InputAction _inputAction;

    private ButtonPanelCloser _doneButtonCloser;
    private TMP_Text _bindingButtonText;
    private InputAction _anyKeyInputAction;
    private ReadOnlyArray<InputControl> _allControls;
    private IDisposable _disposable;

    [Inject]
    private void Construct([Inject(Id = ZenjectIdConstants.BindingDoneButtonId)] ButtonPanelCloser buttonPanelCloser) =>
        _doneButtonCloser = buttonPanelCloser;

    public InputAction InputAction
    {
        get => _inputAction;
        set => _inputAction = value;
    }

    public TMP_Text ActionNameText => _actionNameText;
    public TMP_Text BindingButtonText => _bindingButtonText;

    private void Awake()
    {
        _bindingButtonText = _bindingButton.GetComponentInChildren<TMP_Text>();
        _bindingButtonText.text = _inputAction.GetBindingDisplayString();

        _allControls = Keyboard.current.allControls
            .Where(c => c != Keyboard.current.anyKey)
            .ToArray();

        _anyKeyInputAction = new InputAction(type: InputActionType.PassThrough);
        _anyKeyInputAction.AddBinding(InputConstants.KeyboardAnyKeyPath);
        _anyKeyInputAction.performed += OnKeyPressed;

        _bindingButton.onClick.AddListener(OnBindingButtonPressed);
        _bindingResetButton.onClick.AddListener(OnBindingResetButtonClicked);

        _disposable = R3.Observable
            .EveryUpdate()
            .Select(_ => _inputAction.bindings[0].overridePath == null)
            .DistinctUntilChanged()
            .Subscribe(value => _bindingResetButton.interactable = !value);
    }

    private void OnDestroy()
    {
        _bindingButton.onClick.RemoveListener(OnBindingButtonPressed);
        _bindingResetButton.onClick.RemoveListener(OnBindingResetButtonClicked);

        _disposable?.Dispose();
    }

    private void OnBindingButtonPressed()
    {
        _anyKeyInputAction.Enable();
        _bindingButtonText.text = "Waiting any key";

        _doneButtonCloser.enabled = false;
    }

    private void OnBindingResetButtonClicked()
    {
        _inputAction.RemoveBindingOverride(0);
        _bindingButtonText.text = _inputAction.GetBindingDisplayString();
    }

    private void CancelKeyBinding() => _bindingButtonText.text = _inputAction.GetBindingDisplayString();

    private void OnKeyPressed(InputAction.CallbackContext _)
    {
        var pressedControl = _allControls.First(c => c.IsPressed());

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

        _doneButtonCloser.enabled = true;
    }
}
