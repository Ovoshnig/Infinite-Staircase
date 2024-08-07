public sealed class SettingsSaver : DataSaver
{
    protected override string SaveFileName { get; } = SettingsConstants.SaveFileName;
}
