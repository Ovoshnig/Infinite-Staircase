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

    private ScreenTuner _screenTuner;
    private QualityTuner _qualityTuner;
    private CompositeDisposable _compositeDisposable = new();

    [Inject]
    public void Construct(ScreenTuner screenTuner, QualityTuner qualityTuner)
    {
        _screenTuner = screenTuner;
        _qualityTuner = qualityTuner;
    }

    private void OnEnable()
    {
        _fullScreenToggle.OnValueChangedAsObservable()
            .Skip(1)
            .Subscribe(OnFullScreenToggleValueChanged)
            .AddTo(_compositeDisposable);
        _vSyncToggle.OnValueChangedAsObservable()
            .Skip(1)
            .Subscribe(OnVSyncToggleValueChanged)
            .AddTo(_compositeDisposable);
        _resolutionDropdown.onValueChanged.AsObservable()
            .Skip(1)
            .Subscribe(OnResolutionDropdownValueChanged)
            .AddTo(_compositeDisposable);
    }

    private void Start()
    {
        _screenTuner.IsFullScreen
            .Subscribe(value => _fullScreenToggle.SetIsOnWithoutNotify(value))
            .AddTo(this);

        _vSyncToggle.SetIsOnWithoutNotify(_qualityTuner.IsVSyncEnabled);

        _resolutionDropdown.options = _screenTuner.Resolutions
            .Select(x => new TMP_Dropdown.OptionData($"{x.width}x{x.height}@{x.refreshRate.value:F2}"))
            .ToList();

        _resolutionDropdown.SetValueWithoutNotify(_screenTuner.CurrentResolutionNumber);
    }

    private void OnDisable()
    {
        _compositeDisposable?.Dispose();
        _compositeDisposable = new CompositeDisposable();
    }

    private void OnFullScreenToggleValueChanged(bool _) => _screenTuner.OnSwitchFullScreenPressed();

    private void OnVSyncToggleValueChanged(bool _) => _qualityTuner.SwitchVSync();

    private void OnResolutionDropdownValueChanged(int value) => _screenTuner.SetResolution(value);
}
