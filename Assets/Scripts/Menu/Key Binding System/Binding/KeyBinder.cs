using R3;
using TMPro;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using R3.Triggers;

public class KeyBinder : MonoBehaviour
{
    [SerializeField] private Button _bindingButton;
    [SerializeField] private Button _bindingResetButton;
    [SerializeField] private TMP_Text _actionNameText;
    [SerializeField] private Color _waitingTextColor;
    [SerializeField] private InputActionReference _inputActionReference;

    private KeyBindingsTracker _bindingsTracker;
    private IBindingHandler _bindingHandler;
    private TMP_Text _bindingButtonText;
    private InputAction _inputAction;
    private CompositeDisposable _compositeDisposable = new();

    [Inject]
    public void Construct(KeyBindingsTracker bindingsTracker) => _bindingsTracker = bindingsTracker;

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
    }

    private void OnEnable()
    {
        _bindingButton.OnClickAsObservable()
            .Subscribe(_ => OnBindingButtonPressed())
            .AddTo(_compositeDisposable);
        _bindingResetButton.OnClickAsObservable()
            .Subscribe(_ => OnBindingResetButtonClicked())
            .AddTo(_compositeDisposable);
        Observable
            .EveryValueChanged(this, _ => _inputAction.bindings.Any(b => b.hasOverrides))
            .Subscribe(value => _bindingResetButton.interactable = value)
            .AddTo(_compositeDisposable);
    }

    private void OnDisable()
    {
        _compositeDisposable?.Dispose();
        _compositeDisposable = new CompositeDisposable();
    }

    private void OnBindingButtonPressed() => _bindingHandler.StartListening();

    private void OnBindingResetButtonClicked() => _bindingHandler.Reset();
}
