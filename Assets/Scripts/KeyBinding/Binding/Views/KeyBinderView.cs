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
    [SerializeField] private InputAction _inputAction;
    [SerializeField] private GameSettings _gameSettings;

    private readonly Subject<Unit> _bindingClicked = new();
    private readonly Subject<Unit> _resetClicked = new();

    public InputAction InputAction => _inputAction;
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

    public void SetInputAction(InputAction inputAction)
    {
        if (!Application.isEditor && Application.isPlaying)
        {
            Debug.LogWarning("You cannot set InputActionReference in Play Mode.");
            return;
        }

        _inputAction = inputAction;
    }

    public void SetColor(bool isListening)
    {
        _bindingButtonText.color = isListening
            ? KeyBindingSettings.ListeningTextColor
            : KeyBindingSettings.NormalTextColor;
    }

    public void SetActionNameText(string text) => _actionNameText.text = text;

    public void SetBindingText(string text) => _bindingButtonText.text = text;

    public void SetBindingButtonInteractable(bool value) =>
        _bindingButton.interactable = value;

    public void SetResetButtonInteractable(bool value) =>
        _bindingResetButton.interactable = value;

    public void SetConflictImageEnabled(bool value) => _bindingConflictImage.enabled = value;
}
