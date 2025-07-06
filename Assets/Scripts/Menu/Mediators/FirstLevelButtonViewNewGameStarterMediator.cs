using R3;
using System;
using VContainer.Unity;

public class FirstLevelButtonViewNewGameStarterMediator : IInitializable, IDisposable
{
    private readonly FirstLevelButtonView _firstLevelButtonView;
    private readonly NewGameStarter _newGameStarter;
    private readonly CompositeDisposable _compositeDisposable = new();

    public FirstLevelButtonViewNewGameStarterMediator(FirstLevelButtonView firstLevelButtonView, 
        NewGameStarter newGameStarter)
    {
        _firstLevelButtonView = firstLevelButtonView;
        _newGameStarter = newGameStarter;
    }

    public void Initialize()
    {
        _firstLevelButtonView.Clicked
            .Subscribe(_ => _newGameStarter.StartGame())
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();
}
