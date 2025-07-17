public class SettingsStorageResetViewMediatorFactory
    : MediatorViewFactory<SettingsStorageResetViewMediator, SettingsResetButtonView>
{
    private readonly SettingsStorage _settingsStorage;

    public SettingsStorageResetViewMediatorFactory(SettingsStorage settingsStorage) =>
        _settingsStorage = settingsStorage;

    public override SettingsStorageResetViewMediator Create(SettingsResetButtonView view)
    {
        SettingsStorageResetViewMediator mediator = new(_settingsStorage, view);
        mediator.Initialize();
        Disposables.Add(mediator);
        return mediator;
    }
}
