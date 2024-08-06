using UnityEngine;
using UnityEngine.UI;
using Zenject;

public abstract class Menu : MonoBehaviour
{
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private Button _openSettingsButton;

    [Inject]
    protected void Construct(SceneSwitch sceneSwitch) => SceneSwitch = sceneSwitch;

    protected GameObject SettingsPanel => _settingsPanel;
    protected SceneSwitch SceneSwitch { get; private set; }

    protected void Start() => InitializeSettings();

    protected virtual void OnEnable() => AddListeners();

    protected virtual void OnDisable() => RemoveListeners();

    protected abstract void InitializeSettings();

    protected virtual void AddListeners() => 
        _openSettingsButton.onClick.AddListener(OnOpenSettingsButtonClicked);

    protected virtual void RemoveListeners() => 
        _openSettingsButton.onClick.RemoveListener(OnOpenSettingsButtonClicked);

    private void OnOpenSettingsButtonClicked()
    {
        _settingsPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
