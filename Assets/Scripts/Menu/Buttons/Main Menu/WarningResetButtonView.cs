using R3;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

[RequireComponent(typeof(Button))]
public class WarningResetButtonView : ButtonView
{
    [SerializeField] private Button _continueGameButton;

    private SaveStorage _saveStorage;

    [Inject]
    public void Construct(SaveStorage saveStorage) => _saveStorage = saveStorage;

    protected override void Start()
    {
        base.Start();

        Clicked
            .Skip(1)
            .Subscribe(_ => OnButtonClicked())
            .AddTo(this);
    }

    private void OnButtonClicked()
    {
        _saveStorage.ResetData();
        _saveStorage.Set(SaveConstants.SaveCreatedKey, false);

        _continueGameButton.interactable = false;
    }
}
