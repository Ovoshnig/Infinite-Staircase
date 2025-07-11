public class GameStarterSeedViewMediatorFactory
    : MediatorViewFactory<GameStarterSeedViewMediator, SeedInputFieldView>
{
    private readonly GameStarter _gameStarter;

    public GameStarterSeedViewMediatorFactory(GameStarter gameStarter) => 
        _gameStarter = gameStarter;

    public override GameStarterSeedViewMediator Create(SeedInputFieldView view)
    {
        GameStarterSeedViewMediator mediator = new(_gameStarter, view);
        mediator.Initialize();
        Disposables.Add(mediator);
        return mediator;
    }
}
