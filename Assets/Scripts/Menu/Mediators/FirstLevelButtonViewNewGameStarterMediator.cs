using R3;

public class FirstLevelButtonViewNewGameStarterMediator : Mediator
{
    private readonly FirstLevelButtonView _firstLevelButtonView;
    private readonly NewGameStarter _newGameStarter;

    public FirstLevelButtonViewNewGameStarterMediator(FirstLevelButtonView firstLevelButtonView, 
        NewGameStarter newGameStarter)
    {
        _firstLevelButtonView = firstLevelButtonView;
        _newGameStarter = newGameStarter;
    }

    public override void Initialize()
    {
        _firstLevelButtonView.Clicked
            .Subscribe(_ => _newGameStarter.StartGame())
            .AddTo(CompositeDisposable);
    }
}
