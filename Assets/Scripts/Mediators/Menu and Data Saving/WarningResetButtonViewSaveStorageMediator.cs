using R3;
using System;
using VContainer.Unity;

public class WarningResetButtonViewSaveStorageMediator : IInitializable, IDisposable
{
    private readonly WarningResetButtonView _warningResetButtonView;
    private readonly SaveStorage _saveStorage;
    private readonly CompositeDisposable _compositeDisposable = new();

    public WarningResetButtonViewSaveStorageMediator(WarningResetButtonView warningResetButtonView, 
        SaveStorage saveStorage)
    {
        _warningResetButtonView = warningResetButtonView;
        _saveStorage = saveStorage;
    }

    public void Initialize()
    {
        _warningResetButtonView.Clicked
            .Subscribe(_ => _saveStorage.ResetData())
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();
}
