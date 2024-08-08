public sealed class SaveSaver : DataSaver
{
    protected override string SaveFileName { get; } = SaveConstants.SaveFileName;
}
