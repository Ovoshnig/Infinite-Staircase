using R3;
using System;
using VContainer.Unity;

public class ButtonViewNewGameStarterMediator : IInitializable, IDisposable
{
    private readonly FirstLevelButtonView _firstLevelButtonView;
    private readonly NewGameStarter _newGameStarter;
    private readonly CompositeDisposable _compositeDisposable = new();

    public ButtonViewNewGameStarterMediator(FirstLevelButtonView firstLevelButtonView, NewGameStarter newGameStarter)
    {
        _firstLevelButtonView = firstLevelButtonView;
        _newGameStarter = newGameStarter;
    }

    public void Initialize()
    {
        _firstLevelButtonView.Clicked
            .Subscribe(_ => OnButtonClicked())
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();

    private void OnButtonClicked() => _newGameStarter.StartGame();
}
