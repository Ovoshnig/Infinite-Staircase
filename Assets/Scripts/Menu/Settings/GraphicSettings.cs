using R3;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GraphicSettings : MonoBehaviour
{
    [SerializeField] private Toggle _fullScreenToggle;
    [SerializeField] private TMP_Dropdown _resolutionDropdown;

    private ScreenTuner _screenTuner;
    private IDisposable _disposable;

    [Inject]
    private void Construct(ScreenTuner screenTuner) => _screenTuner = screenTuner;

    private void Awake()
    {
        _disposable = _screenTuner.IsFullScreen
            .Subscribe(value =>
            {
                if (_fullScreenToggle.isOn != value)
                    _fullScreenToggle.SetIsOnWithoutNotify(value);
            });
    }

    private void OnEnable()
    {
        _fullScreenToggle.onValueChanged.AddListener(_ => _screenTuner.SwitchFullScreen());
        _resolutionDropdown.onValueChanged.AddListener(value => _screenTuner.SetResolution(value));
    }

    private void Start()
    {
        _resolutionDropdown.options = _screenTuner.Resolutions
            .Select(x => new TMP_Dropdown.OptionData($"{x.width}x{x.height}"))
            .ToList();

        _resolutionDropdown.SetValueWithoutNotify(_screenTuner.CurrentResolutionNumber);
    }

    private void OnDisable()
    {
        _fullScreenToggle.onValueChanged.RemoveListener(_ => _screenTuner.SwitchFullScreen());
        _resolutionDropdown.onValueChanged.RemoveListener(value => _screenTuner.SetResolution(value));
    }

    private void OnDestroy() => _disposable?.Dispose();
}
