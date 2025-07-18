public class GameStarterSeedViewMediatorFactory
    : MediatorViewFactory<GameStarterSeedViewMediator, SeedInputFieldView>
{
    private readonly GameStarter _gameStarter;

    public GameStarterSeedViewMediatorFactory(GameStarter gameStarter) =>
        _gameStarter = gameStarter;

    protected override GameStarterSeedViewMediator CreateMediatorInstance(SeedInputFieldView view) =>
        new(_gameStarter, view);
}
