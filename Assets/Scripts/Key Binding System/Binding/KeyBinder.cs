using R3;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;

public class KeyBinder : MonoBehaviour
{
    [SerializeField] private Button _bindingButton;
    [SerializeField] private Button _bindingResetButton;
    [SerializeField] private TMP_Text _actionNameText;
    [SerializeField] private Color _waitingTextColor;
    [SerializeField] private InputActionReference _inputActionReference;

    private PlayerInput _playerInput;
    private KeyBindingsTracker _bindingsTracker;
    private IBindingHandler _bindingHandler;
    private TMP_Text _bindingButtonText;
    private InputAction _inputAction;

    [Inject]
    public void Construct(KeyBindingsTracker bindingsTracker, PlayerInput playerInput)
    {
        _bindingsTracker = bindingsTracker;
        _playerInput = playerInput;
    }

    public InputActionReference InputActionReference
    {
        get => _inputActionReference;
        set => _inputActionReference = value;
    }

    public TMP_Text ActionNameText => _actionNameText;

    private void Awake() => _bindingButtonText = _bindingButton.GetComponentInChildren<TMP_Text>();

    private void Start()
    {
        Color normalTextColor = _bindingButtonText.color;

        _inputAction = _inputActionReference.action;

        if (_inputAction.type == InputActionType.Button)
        {
            _bindingHandler = new ButtonBindingHandler(_bindingsTracker, _bindingButtonText,
                normalTextColor, _waitingTextColor, _playerInput, _inputActionReference);
        }
        else if (_inputAction.type == InputActionType.Value &&
            _inputAction.expectedControlType == nameof(Vector2))
        {
            _bindingHandler = new Vector2BindingHandler(_bindingsTracker, _bindingButtonText,
                normalTextColor, _waitingTextColor, _playerInput, _inputActionReference);
        }

        _bindingButton.OnClickAsObservable()
            .Subscribe(_ => _bindingHandler.StartListening())
            .AddTo(this);
        _bindingResetButton.OnClickAsObservable()
            .Subscribe(_ => _bindingHandler.Reset())
            .AddTo(this);
        Observable
            .EveryValueChanged(this, _ => _inputAction.bindings.Any(b => b.hasOverrides))
            .Subscribe(value => _bindingResetButton.interactable = value)
            .AddTo(this);
    }
}
