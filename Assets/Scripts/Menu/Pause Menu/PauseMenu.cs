using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button _resetLevelButton;
    [SerializeField] private Button _loadMainMenuButton;

    private SceneSwitch _sceneSwitch;
    private CompositeDisposable _compositeDisposable = new();

    [Inject]
    public void Construct(SceneSwitch sceneSwitch) => _sceneSwitch = sceneSwitch;

    private void OnEnable()
    {
        _resetLevelButton.OnClickAsObservable()
            .Subscribe(_ => _sceneSwitch.LoadCurrentLevel())
            .AddTo(_compositeDisposable);
        _loadMainMenuButton.OnClickAsObservable()
            .Subscribe(_ => _sceneSwitch.LoadLevel(0).Forget())
            .AddTo(_compositeDisposable);
    }

    private void OnDisable()
    {
        _compositeDisposable?.Dispose();
        _compositeDisposable = new CompositeDisposable();
    }
}
