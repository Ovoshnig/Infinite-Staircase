using R3;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

[RequireComponent(typeof(Button))]
public class WarningResetButton : MonoBehaviour
{
    [SerializeField] private Button _continueGameButton;

    private SaveStorage _saveStorage;
    private Button _resetButton;

    [Inject]
    public void Construct(SaveStorage saveStorage) => _saveStorage = saveStorage;

    private void Awake() => _resetButton = GetComponent<Button>();

    private void Start()
    {
        _resetButton.OnClickAsObservable()
            .Subscribe(_ => OnResetButtonClicked())
            .AddTo(this);
    }

    private void OnResetButtonClicked()
    {
        _saveStorage.ResetData();
        _saveStorage.Set(SaveConstants.SaveCreatedKey, false);

        _continueGameButton.interactable = false;
    }
}
