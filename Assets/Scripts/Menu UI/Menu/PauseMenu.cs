using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public sealed class PauseMenu : Menu
{
    [SerializeField] private Button _resetLevelButton;
    [SerializeField] private Button _loadMainMenuButton;

    protected override void InitializeSettings()
    {
        base.InitializeSettings();

        gameObject.SetActive(false);
    }

    protected override void AddListeners()
    {
        base.AddListeners();

        _resetLevelButton.onClick.AddListener(OnResetButtonClicked);
        _loadMainMenuButton.onClick.AddListener(OnLoadMainMenuButtonClicked);
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();

        _resetLevelButton.onClick.RemoveListener(OnResetButtonClicked);
        _loadMainMenuButton.onClick.RemoveListener(OnLoadMainMenuButtonClicked);
    }

    private void OnResetButtonClicked() => SceneSwitch.LoadCurrentLevel();

    private void OnLoadMainMenuButtonClicked() => SceneSwitch.LoadLevel(0).Forget();
}
