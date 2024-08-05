public sealed class SaveSaver : DataSaver
{
    protected override void SetFileName() => SaveFileName = "save.json";

    public void Reset()
    {

    }
}
