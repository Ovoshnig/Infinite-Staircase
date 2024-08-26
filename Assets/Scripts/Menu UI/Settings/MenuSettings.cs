using UnityEngine;
using UnityEngine.UI;
using Zenject;

public abstract class MenuSettings : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private Button _closeSettingsButton;
    [SerializeField] private Slider _sensitivitySlider;
    [SerializeField] private Slider _soundVolumeSlider;
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

    protected virtual void InitializeSliders()
    {
        _sensitivitySlider.maxValue = _controlSettings.MaxSensitivity;
        _sensitivitySlider.value = _lookTuner.Sensitivity.Value;

        _soundVolumeSlider.minValue = _audioSettings.MinVolume;
        _soundVolumeSlider.maxValue = _audioSettings.MaxVolume;
        _soundVolumeSlider.value = _audioTuner.SoundVolume.Value;

        _musicVolumeSlider.minValue = _audioSettings.MinVolume;
        _musicVolumeSlider.maxValue = _audioSettings.MaxVolume;
        _musicVolumeSlider.value = _audioTuner.MusicVolume.Value;
    }

    protected virtual void AddListeners()
    {
        _closeSettingsButton.onClick.AddListener(CloseSettingsPanel);

        _sensitivitySlider.onValueChanged.AddListener(OnSensitivitySliderValueChanged);
        _soundVolumeSlider.onValueChanged.AddListener(OnSoundsVolumeSliderValueChanged);
        _musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeSliderValueChanged);
    }

    protected virtual void RemoveListeners()
    {
        _closeSettingsButton.onClick.RemoveListener(CloseSettingsPanel);

        _sensitivitySlider.onValueChanged.RemoveListener(OnSensitivitySliderValueChanged);
        _soundVolumeSlider.onValueChanged.RemoveListener(OnSoundsVolumeSliderValueChanged);
        _musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeSliderValueChanged);
    }

    private void CloseSettingsPanel()
    {
        MenuPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnSensitivitySliderValueChanged(float value) => 
        _lookTuner.Sensitivity.Value = value;

    private void OnSoundsVolumeSliderValueChanged(float value) => 
        _audioTuner.SoundVolume.Value = value;

    private void OnMusicVolumeSliderValueChanged(float value) => 
        _audioTuner.MusicVolume.Value = value;
}
