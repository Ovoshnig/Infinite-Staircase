using R3;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class ScreenTunerResolutionDropdownViewMediator : Mediator
{
    private readonly ScreenTuner _screenTuner;
    private readonly ResolutionDropdownView _resolutionDropdownView;

    public ScreenTunerResolutionDropdownViewMediator(ScreenTuner screenTuner,
        ResolutionDropdownView resolutionDropdownView)
    {
        _screenTuner = screenTuner;
        _resolutionDropdownView = resolutionDropdownView;
    }

    public override void Initialize()
    {
        _resolutionDropdownView.Value
            .Skip(1)
            .Subscribe(_screenTuner.SetResolution)
            .AddTo(CompositeDisposable);

        List<TMP_Dropdown.OptionData> options = _screenTuner.Resolutions
            .Select(r => new TMP_Dropdown.OptionData($"{r.width}x{r.height}@{r.refreshRate.value:F2}"))
            .ToList();
        _resolutionDropdownView.SetOptions(options);
        _resolutionDropdownView.SetValue(_screenTuner.CurrentResolutionNumber);
    }
}
