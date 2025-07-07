using Cysharp.Threading.Tasks;
using R3;

public class NewGameStarterSceneSwitchMediator : Mediator
{
    private readonly NewGameStarter _newGameStarter;
    private readonly SceneSwitch _sceneSwitch;

    public NewGameStarterSceneSwitchMediator(NewGameStarter newGameStarter, SceneSwitch sceneSwitch)
    {
        _newGameStarter = newGameStarter;
        _sceneSwitch = sceneSwitch;
    }

    public override void Initialize()
    {
        _newGameStarter.NewGameStarted
            .Subscribe(_ => _sceneSwitch.LoadFirstLevelAsync().Forget())
            .AddTo(CompositeDisposable);
    }
}
