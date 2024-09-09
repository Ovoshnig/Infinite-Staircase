using R3;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
using System;

public class KeyBinder : MonoBehaviour
{
    [SerializeField] private Button _bindingButton;
    [SerializeField] private Button _bindingResetButton;
    [SerializeField] private TMP_Text _actionNameText;
    [SerializeField] private Color _waitingTextColor;
    [SerializeField] private InputAction _inputAction;

    private KeyBindingsTracker _bindingsTracker;
    private IBindingHandler _bindingHandler;
    private TMP_Text _bindingButtonText;
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
        var normalTextColor = _bindingButtonText.color;

        if (_inputAction.type == InputActionType.Button)
        {
            _bindingHandler = new ButtonBindingHandler(_bindingsTracker, _bindingButtonText,
                normalTextColor, _waitingTextColor, _inputAction);
        }
        else if (_inputAction.type == InputActionType.Value &&
            _inputAction.expectedControlType == nameof(Vector2))
        {
            _bindingHandler = new Vector2BindingHandler(_bindingsTracker, _bindingButtonText,
                normalTextColor, _waitingTextColor, _inputAction);
        }

        _bindingButton.onClick.AddListener(OnBindingButtonPressed);
        _bindingResetButton.onClick.AddListener(OnBindingResetButtonClicked);

        _disposable = Observable
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

    private void OnBindingButtonPressed() => _bindingHandler.Bind();

    private void OnBindingResetButtonClicked() => _bindingHandler.Reset();
}
