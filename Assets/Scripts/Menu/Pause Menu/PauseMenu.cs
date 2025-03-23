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

    [Inject]
    public void Construct(SceneSwitch sceneSwitch) => _sceneSwitch = sceneSwitch;

    private void Start()
    {
        _resetLevelButton.OnClickAsObservable()
            .Subscribe(_ => _sceneSwitch.LoadCurrentLevel())
            .AddTo(this);
        _loadMainMenuButton.OnClickAsObservable()
            .Subscribe(_ => _sceneSwitch.LoadLevelAsync(0).Forget())
            .AddTo(this);
    }
}
