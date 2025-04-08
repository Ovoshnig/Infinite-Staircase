using System;
using VContainer.Unity;

public class ButtonViewNewGameStarterMediator : IInitializable, IDisposable
{
    private readonly FirstLevelButtonView _firstLevelButtonView;
    private readonly NewGameStarter _newGameStarter;

    public ButtonViewNewGameStarterMediator(FirstLevelButtonView firstLevelButtonView, NewGameStarter newGameStarter)
    {
        _firstLevelButtonView = firstLevelButtonView;
        _newGameStarter = newGameStarter;
    }

    public void Initialize() => _firstLevelButtonView.ButtonClicked += OnButtonClicked;

    public void Dispose() => _firstLevelButtonView.ButtonClicked -= OnButtonClicked;

    private void OnButtonClicked() => _newGameStarter.StartGame();
}
