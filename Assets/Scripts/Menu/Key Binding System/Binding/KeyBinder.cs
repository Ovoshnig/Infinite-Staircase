using R3;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
using System.Linq;

public class KeyBinder : MonoBehaviour
{
    [SerializeField] private Button _bindingButton;
    [SerializeField] private Button _bindingResetButton;
    [SerializeField] private TMP_Text _actionNameText;
    [SerializeField] private Color _waitingTextColor;
    [SerializeField] private InputActionReference _inputActionReference;

    private readonly CompositeDisposable _compositeDisposable = new();
    private KeyBindingsTracker _bindingsTracker;
    private IBindingHandler _bindingHandler;
    private TMP_Text _bindingButtonText;
    private InputAction _inputAction;

    [Inject]
    private void Construct(KeyBindingsTracker bindingsTracker) => _bindingsTracker = bindingsTracker;

    public InputActionReference InputActionReference
    {
        get => _inputActionReference;
        set => _inputActionReference = value;
    }

    public TMP_Text ActionNameText => _actionNameText;

    private void Awake()
    {
        _bindingButtonText = _bindingButton.GetComponentInChildren<TMP_Text>();
        var normalTextColor = _bindingButtonText.color;

        _inputAction = _inputActionReference.action;

        if (_inputAction.type == InputActionType.Button)
        {
            _bindingHandler = new ButtonBindingHandler(_bindingsTracker, _bindingButtonText,
                normalTextColor, _waitingTextColor, _inputActionReference);
        }
        else if (_inputAction.type == InputActionType.Value &&
            _inputAction.expectedControlType == nameof(Vector2))
        {
            _bindingHandler = new Vector2BindingHandler(_bindingsTracker, _bindingButtonText,
                normalTextColor, _waitingTextColor, _inputActionReference);
        }

        _bindingButton.onClick.AddListener(OnBindingButtonPressed);
        _bindingResetButton.onClick.AddListener(OnBindingResetButtonClicked);

        Observable
            .EveryUpdate()
            .Select(_ => _inputAction.bindings.Any(b => b.hasOverrides))
            .DistinctUntilChanged()
            .Subscribe(value => _bindingResetButton.interactable = value)
            .AddTo(_compositeDisposable);
    }
    
    private void OnDestroy()
    {
        _bindingButton.onClick.RemoveListener(OnBindingButtonPressed);
        _bindingResetButton.onClick.RemoveListener(OnBindingResetButtonClicked);

        _compositeDisposable?.Dispose();
    }

    private void OnBindingButtonPressed() => _bindingHandler.StartListening();

    private void OnBindingResetButtonClicked() => _bindingHandler.Reset();
}
