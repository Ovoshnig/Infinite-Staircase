using Cysharp.Threading.Tasks;
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

    private void OnEnable()
    {
        _resetLevelButton.onClick.AddListener(OnResetButtonClicked);
        _loadMainMenuButton.onClick.AddListener(OnLoadMainMenuButtonClicked);
    }

    private void OnDisable()
    {
        _resetLevelButton.onClick.RemoveListener(OnResetButtonClicked);
        _loadMainMenuButton.onClick.RemoveListener(OnLoadMainMenuButtonClicked);
    }

    private void OnResetButtonClicked() => _sceneSwitch.LoadCurrentLevel();

    private void OnLoadMainMenuButtonClicked() => _sceneSwitch.LoadLevel(0).Forget();
}
