using R3;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class GraphicSettings : MonoBehaviour
{
    [SerializeField] private Toggle _fullScreenToggle;
    [SerializeField] private Toggle _vSyncToggle;
    [SerializeField] private TMP_Dropdown _resolutionDropdown;

    private readonly CompositeDisposable _compositeDisposable = new();
    private ScreenTuner _screenTuner;
    private QualityTuner _qualityTuner;

    [Inject]
    public void Construct(ScreenTuner screenTuner, QualityTuner qualityTuner)
    {
        _screenTuner = screenTuner;
        _qualityTuner = qualityTuner;
    }

    private void Awake()
    {
        _screenTuner.IsFullScreen
            .Subscribe(value =>
            {
                if (_fullScreenToggle.isOn != value)
                    _fullScreenToggle.SetIsOnWithoutNotify(value);
            })
            .AddTo(_compositeDisposable);
    }

    private void OnEnable()
    {
        _fullScreenToggle.onValueChanged.AddListener(OnFullScreenToggleValueChanged);
        _vSyncToggle.onValueChanged.AddListener(OnVSyncToggleValueChanged);
        _resolutionDropdown.onValueChanged.AddListener(OnResolutionDropdownValueChanged);
    }

    private void Start()
    {
        _vSyncToggle.SetIsOnWithoutNotify(_qualityTuner.IsVSyncEnabled);

        _resolutionDropdown.options = _screenTuner.Resolutions
            .Select(x => new TMP_Dropdown.OptionData($"{x.width}x{x.height}"))
            .ToList();

        _resolutionDropdown.SetValueWithoutNotify(_screenTuner.CurrentResolutionNumber);
    }

    private void OnDisable()
    {
        _fullScreenToggle.onValueChanged.RemoveListener(OnFullScreenToggleValueChanged);
        _vSyncToggle.onValueChanged.RemoveListener(OnVSyncToggleValueChanged);
        _resolutionDropdown.onValueChanged.RemoveListener(OnResolutionDropdownValueChanged);
    }

    private void OnDestroy() => _compositeDisposable?.Dispose();

    private void OnFullScreenToggleValueChanged(bool _) => _screenTuner.SwitchFullScreen();

    private void OnVSyncToggleValueChanged(bool _) => _qualityTuner.SwitchVSync();

    private void OnResolutionDropdownValueChanged(int value) => _screenTuner.SetResolution(value);
}
