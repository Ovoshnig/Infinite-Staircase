public class GameStarterFirstLevelViewMediatorFactory
    : MediatorViewFactory<GameStarterFirstLevelViewMediator, FirstLevelButtonView>
{
    private readonly GameStarter _gameStarter;

    public GameStarterFirstLevelViewMediatorFactory(GameStarter gameStarter) =>
        _gameStarter = gameStarter;

    public override GameStarterFirstLevelViewMediator Create(FirstLevelButtonView view)
    {
        GameStarterFirstLevelViewMediator mediator = new(_gameStarter, view);
        mediator.Initialize();
        Disposables.Add(mediator);
        return mediator;
    }
}
