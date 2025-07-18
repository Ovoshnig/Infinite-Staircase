public class GameStarterFirstLevelViewMediatorFactory
    : MediatorViewFactory<GameStarterFirstLevelViewMediator, FirstLevelButtonView>
{
    private readonly GameStarter _gameStarter;

    public GameStarterFirstLevelViewMediatorFactory(GameStarter gameStarter) =>
        _gameStarter = gameStarter;

    protected override GameStarterFirstLevelViewMediator CreateMediatorInstance(FirstLevelButtonView view) =>
        new(_gameStarter, view);
}
