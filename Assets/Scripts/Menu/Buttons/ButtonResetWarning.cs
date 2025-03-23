using R3;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

[RequireComponent(typeof(Button))]
public class ButtonResetWarning : MonoBehaviour
{
    private CompositeDisposable _compositeDisposable = new();
    private SaveStorage _saveStorage;
    private Button _button;

    [Inject]
    public void Construct(SaveStorage saveStorage) => _saveStorage = saveStorage;

    private void Awake() => _button = GetComponent<Button>();

    private void OnEnable()
    {
        _button.OnClickAsObservable()
            .Subscribe(_ => OnResetButtonClicked())
            .AddTo(_compositeDisposable);
    }

    private void OnDisable()
    {
        _compositeDisposable?.Dispose();
        _compositeDisposable = new();
    }

    private void OnResetButtonClicked()
    {
        _saveStorage.ResetData();
        _saveStorage.Set(SaveConstants.SaveCreatedKey, false);
    }
}
