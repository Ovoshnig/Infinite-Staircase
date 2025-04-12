using Cysharp.Threading.Tasks;
using R3;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;

public class KeyBinderView : MonoBehaviour
{
    [SerializeField] private Button _bindingButton;
    [SerializeField] private Button _bindingResetButton;
    [SerializeField] private TMP_Text _actionNameText;
    [SerializeField] private Color _waitingTextColor;
    [SerializeField] private InputActionReference _inputActionReference;

    private InputActions _inputActions;
    private KeyListeningTracker _listeningTracker;
    private BindingHandler _bindingHandler;
    private TMP_Text _bindingButtonText;
    private InputAction _inputAction;

    [Inject]
    public void Construct(KeyListeningTracker listeningTracker, InputActions inputActions)
    {
        _listeningTracker = listeningTracker;
        _inputActions = inputActions;
    }

    private void Awake() => _bindingButtonText = _bindingButton.GetComponentInChildren<TMP_Text>();

    private void Start()
    {
        Color normalTextColor = _bindingButtonText.color;

        _inputAction = _inputActionReference.action;

        if (_inputAction.type == InputActionType.Button)
            _bindingHandler = new ButtonBindingHandler(_listeningTracker, _inputActions, _inputActionReference);
        else if (_inputAction.type == InputActionType.Value && _inputAction.expectedControlType == nameof(Vector2))
            _bindingHandler = new Vector2BindingHandler(_listeningTracker, _inputActions, _inputActionReference);

        _bindingHandler.Initialize();

        _bindingHandler.IsListening
            .Subscribe(value =>
            {
                if (value)
                    _bindingButtonText.color = _waitingTextColor;
                else
                    _bindingButtonText.color = normalTextColor;
            })
            .AddTo(this);
        _bindingHandler.BindingText
            .Subscribe(value => _bindingButtonText.text = value)
            .AddTo(this);

        _bindingButton.OnClickAsObservable()
            .Subscribe(_ => _bindingHandler.StartListening())
            .AddTo(this);
        _bindingResetButton.OnClickAsObservable()
            .Subscribe(_ => _bindingHandler.ResetBinding())
            .AddTo(this);
        Observable
            .EveryValueChanged(this, _ => _inputAction.bindings.Any(b => b.hasOverrides))
            .Subscribe(value => _bindingResetButton.interactable = value)
            .AddTo(this);
    }

    private void OnDestroy() => _bindingHandler.Dispose();

    public void SetInputAction(InputActionReference inputActionReference, string name)
    {
        _inputActionReference = inputActionReference;
        _actionNameText.text = name;
    }
}
