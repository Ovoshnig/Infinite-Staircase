using Cysharp.Threading.Tasks;
using R3;

public class SceneSwitchButtonViewMediator : Mediator
{
    private readonly SceneSwitch _sceneSwitch;
    private readonly SceneButtonView _sceneButtonView;

    public SceneSwitchButtonViewMediator(SceneSwitch sceneSwitch,
        SceneButtonView sceneButtonView)
    {
        _sceneSwitch = sceneSwitch;
        _sceneButtonView = sceneButtonView;
    }

    public override void Initialize()
    {
        _sceneButtonView.Clicked
            .Subscribe(_ => OnButtonClicked())
            .AddTo(CompositeDisposable);
    }

    private void OnButtonClicked()
    {
        switch (_sceneButtonView)
        {
            case AchievedLevelButtonView:
                _sceneSwitch.LoadAchievedLevelAsync().Forget();
                break;
            case CurrentLevelButtonView:
                _sceneSwitch.LoadCurrentLevelAsync().Forget();
                break;
            case FirstLevelButtonView:
                _sceneSwitch.LoadFirstLevelAsync().Forget();
                break;
            case MainMenuButtonView:
                _sceneSwitch.LoadLevelAsync(0).Forget();
                break;
            default:
                throw new System.Exception($"Unknown scene button view type: " +
                    $"{_sceneButtonView.GetType().Name}");
        }
    }
}
