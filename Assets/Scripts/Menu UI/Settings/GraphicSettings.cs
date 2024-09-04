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

    [Inject]
    private void Construct(ScreenTuner screenTuner) => _screenTuner = screenTuner;

    private void Start()
    {
        _fullScreenToggle.isOn = _screenTuner.IsFullScreen;

        var resolutions = _screenTuner.GetResolutions();

        _resolutionDropdown.options = resolutions
            .Select(x => new TMP_Dropdown.OptionData($"{x.width}x{x.height}"))
            .ToList();

        _resolutionDropdown.value = _screenTuner.CurrentResolutionNumber;
    }

    private void OnEnable()
    {
        _fullScreenToggle.onValueChanged.AddListener(_ => _screenTuner.SwitchFullScreen());
        _resolutionDropdown.onValueChanged.AddListener(value => _screenTuner.SetResolution(value));
    }

    private void OnDisable()
    {
        _fullScreenToggle.onValueChanged.RemoveListener(_ => _screenTuner.SwitchFullScreen());
        _resolutionDropdown.onValueChanged.RemoveListener(value => _screenTuner.SetResolution(value));
    }
}
