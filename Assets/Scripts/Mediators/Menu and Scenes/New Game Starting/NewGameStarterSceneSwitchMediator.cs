using Cysharp.Threading.Tasks;
using System;
using VContainer.Unity;

public class NewGameStarterSceneSwitchMediator : IInitializable, IDisposable
{
    private readonly NewGameStarter _newGameStarter;
    private readonly SceneSwitch _sceneSwitch;

    public NewGameStarterSceneSwitchMediator(NewGameStarter newGameStarter, SceneSwitch sceneSwitch)
    {
        _newGameStarter = newGameStarter;
        _sceneSwitch = sceneSwitch;
    }

    public void Initialize() => _newGameStarter.NewGameStarted += OnNewGameStarted;

    public void Dispose() => _newGameStarter.NewGameStarted -= OnNewGameStarted;

    private void OnNewGameStarted() => _sceneSwitch.LoadFirstLevelAsync().Forget();
}
