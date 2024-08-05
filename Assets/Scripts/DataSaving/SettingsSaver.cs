public sealed class SettingsSaver : DataSaver
{
    protected override void SetFileName() => SaveFileName = "playerSettings.json";
}
