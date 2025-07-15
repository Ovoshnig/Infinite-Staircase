using R3;

public class SettingsStorageResetViewMediator : Mediator
{
    private readonly SettingsStorage _settingsStorage;
    private readonly SettingsResetButtonView _settingsResetButtonView;

    public SettingsStorageResetViewMediator(SettingsStorage settingsStorage, 
        SettingsResetButtonView settingsResetButtonView)
    {
        _settingsStorage = settingsStorage;
        _settingsResetButtonView = settingsResetButtonView;
    }

    public override void Initialize()
    {
        _settingsResetButtonView.Clicked
            .Subscribe(_ => _settingsStorage.ResetData())
            .AddTo(CompositeDisposable);
    }
}
