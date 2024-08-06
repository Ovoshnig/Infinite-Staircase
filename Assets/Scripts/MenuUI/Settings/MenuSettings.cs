using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MenuSettings : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private Button _closeSettingsPanelButton;
    [SerializeField] private Slider _sensitivitySlider;
    [SerializeField] private Slider _soundsVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;

    private LookTuner _lookTuner;
    private AudioTuner _audioTuner;
    private GameSettingsInstaller.ControlSettings _controlSettings;
    private GameSettingsInstaller.AudioSettings _audioSettings;

    [Inject]
    protected void Construct(LookTuner lookTuner, AudioTuner audioTuner, 
        GameSettingsInstaller.ControlSettings controlSettings,
        GameSettingsInstaller.AudioSettings audioSettings)
    {
        _lookTuner = lookTuner;
        _audioTuner = audioTuner;
        _controlSettings = controlSettings;
        _audioSettings = audioSettings;
    }

    protected GameObject MenuPanel => _menuPanel;

    protected void Start() => InitializeSliders();

    protected virtual void OnEnable() => AddListeners();

    protected virtual void OnDisable() => RemoveListeners();

    private void InitializeSliders()
    {
        _sensitivitySlider.maxValue = _controlSettings.MaxSensitivity;
        _sensitivitySlider.value = _lookTuner.Sensitivity;

        _soundsVolumeSlider.minValue = _audioSettings.MinVolume;
        _soundsVolumeSlider.maxValue = _audioSettings.MaxVolume;
        _soundsVolumeSlider.value = _audioTuner.SoundsVolume;

        _musicVolumeSlider.minValue = _audioSettings.MinVolume;
        _musicVolumeSlider.maxValue = _audioSettings.MaxVolume;
        _musicVolumeSlider.value = _audioTuner.MusicVolume;
    }

    private void AddListeners()
    {
        _closeSettingsPanelButton.onClick.AddListener(CloseSettingsPanel);

        _sensitivitySlider.onValueChanged.AddListener(OnSensitivitySliderValueChanged);
        _soundsVolumeSlider.onValueChanged.AddListener(OnSoundsVolumeSliderValueChanged);
        _musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeSliderValueChanged);
    }

    private void RemoveListeners()
    {
        _closeSettingsPanelButton.onClick.RemoveListener(CloseSettingsPanel);

        _sensitivitySlider.onValueChanged.RemoveListener(OnSensitivitySliderValueChanged);
        _soundsVolumeSlider.onValueChanged.RemoveListener(OnSoundsVolumeSliderValueChanged);
        _musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeSliderValueChanged);
    }

    private void CloseSettingsPanel()
    {
        MenuPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnSensitivitySliderValueChanged(float value) => _lookTuner.Sensitivity = value;

    private void OnSoundsVolumeSliderValueChanged(float value) => _audioTuner.SoundsVolume = value;

    private void OnMusicVolumeSliderValueChanged(float value) => _audioTuner.MusicVolume = value;
}
