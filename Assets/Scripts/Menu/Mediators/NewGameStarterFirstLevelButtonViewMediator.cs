using R3;

public class NewGameStarterFirstLevelButtonViewMediator : Mediator
{
    private readonly NewGameStarter _newGameStarter;
    private readonly FirstLevelButtonView _firstLevelButtonView;

    public NewGameStarterFirstLevelButtonViewMediator(NewGameStarter newGameStarter, 
        FirstLevelButtonView firstLevelButtonView)
    {
        _newGameStarter = newGameStarter;
        _firstLevelButtonView = firstLevelButtonView;
    }

    public override void Initialize()
    {
        _firstLevelButtonView.Clicked
            .Subscribe(_ => _newGameStarter.StartGame())
            .AddTo(CompositeDisposable);
    }
}
