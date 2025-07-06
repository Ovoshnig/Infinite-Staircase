using Cysharp.Threading.Tasks;
using R3;
using System;
using VContainer.Unity;

public class NewGameStarterSceneSwitchMediator : IInitializable, IDisposable
{
    private readonly NewGameStarter _newGameStarter;
    private readonly SceneSwitch _sceneSwitch;
    private readonly CompositeDisposable _compositeDisposable = new();

    public NewGameStarterSceneSwitchMediator(NewGameStarter newGameStarter, SceneSwitch sceneSwitch)
    {
        _newGameStarter = newGameStarter;
        _sceneSwitch = sceneSwitch;
    }

    public void Initialize()
    {
        _newGameStarter.NewGameStarted
            .Subscribe(_ => _sceneSwitch.LoadFirstLevelAsync().Forget())
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();
}
