using R3;
using System;
using VContainer.Unity;

public class SeedInputFieldNewGameStarterMediator : IInitializable, IDisposable
{
    private readonly SeedInputFieldView _seedInputView;
    private readonly NewGameStarter _newGameStarter;
    private readonly CompositeDisposable _compositeDisposable = new();

    public SeedInputFieldNewGameStarterMediator(SeedInputFieldView seedInputView, 
        NewGameStarter newGameStarter)
    {
        _seedInputView = seedInputView;
        _newGameStarter = newGameStarter;
    }

    public void Initialize()
    {
        _seedInputView.Text
            .Subscribe(value => _newGameStarter.SetSeedText(value))
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();
}
