using R3;

public class GameStarterFirstLevelViewMediator : Mediator
{
    private readonly GameStarter _gameStarter;
    private readonly FirstLevelButtonView _firstLevelButtonView;

    public GameStarterFirstLevelViewMediator(GameStarter gameStarter, 
        FirstLevelButtonView firstLevelButtonView)
    {
        _gameStarter = gameStarter;
        _firstLevelButtonView = firstLevelButtonView;
    }

    public override void Initialize()
    {
        _firstLevelButtonView.Clicked
            .Subscribe(_ => _gameStarter.StartGame())
            .AddTo(CompositeDisposable);
    }
}
