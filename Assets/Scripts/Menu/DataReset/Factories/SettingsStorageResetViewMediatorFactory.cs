public class SettingsStorageResetViewMediatorFactory
    : MediatorViewFactory<SettingsStorageResetViewMediator, SettingsResetButtonView>
{
    private readonly SettingsStorage _settingsStorage;

    public SettingsStorageResetViewMediatorFactory(SettingsStorage settingsStorage) =>
        _settingsStorage = settingsStorage;

    protected override SettingsStorageResetViewMediator CreateMediatorInstance(SettingsResetButtonView view) =>
        new(_settingsStorage, view);
}
