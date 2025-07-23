using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeyBinderView : MonoBehaviour
{
    [SerializeField] private Button _bindingButton;
    [SerializeField] private Button _bindingResetButton;
    [SerializeField] private TMP_Text _actionNameText;
    [SerializeField] private TMP_Text _bindingButtonText;
    [SerializeField] private Image _bindingConflictImage;
    [SerializeField] private InputActionReference _inputActionReference;
    [SerializeField] private GameSettings _gameSettings;

    private readonly Subject<Unit> _bindingClicked = new();
    private readonly Subject<Unit> _resetClicked = new();

    public InputAction InputAction => _inputActionReference.action;
    public Observable<Unit> BindingClicked => _bindingClicked;
    public Observable<Unit> ResetClicked => _resetClicked;

    private KeyBindingSettings KeyBindingSettings => _gameSettings.KeyBindingSettings;

    private void Start()
    {
        _bindingButton.OnClickAsObservable()
            .Subscribe(_ => _bindingClicked.OnNext(Unit.Default))
            .AddTo(this);
        _bindingResetButton.OnClickAsObservable()
            .Subscribe(_ => _resetClicked.OnNext(Unit.Default))
            .AddTo(this);
    }

    public void SetInputActionReference(InputActionReference inputActionReference, string name)
    {
        if (!Application.isEditor || Application.isPlaying)
            return;

        _inputActionReference = inputActionReference;
        _actionNameText.text = name;
    }

    public void SetColor(bool isListening)
    {
        if (isListening)
            _bindingButtonText.color = KeyBindingSettings.WaitingTextColor;
        else
            _bindingButtonText.color = KeyBindingSettings.NormalTextColor;
    }

    public void SetBindingText(string text) => _bindingButtonText.text = text;

    public void SetResetButtonInteractable(bool value) => 
        _bindingResetButton.interactable = value;

    public void SetConflictImageEnabled(bool value) => _bindingConflictImage.enabled = value;
}
