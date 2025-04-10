using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using VContainer.Unity;

public class ResolutionDropdownViewScreenTunerMediator : IInitializable, IDisposable
{
    private readonly ResolutionDropdownView _resolutionDropdownView;
    private readonly ScreenTuner _screenTuner;
    private readonly CompositeDisposable _compositeDisposable = new();

    public ResolutionDropdownViewScreenTunerMediator(ResolutionDropdownView resolutionDropdownView, 
        ScreenTuner screenTuner)
    {
        _resolutionDropdownView = resolutionDropdownView;
        _screenTuner = screenTuner;
    }

    public void Initialize()
    {
        _resolutionDropdownView.Value
            .Skip(1)
            .Subscribe(_screenTuner.SetResolution)
            .AddTo(_compositeDisposable);

        List<TMP_Dropdown.OptionData> options = _screenTuner.Resolutions
            .Select(r => new TMP_Dropdown.OptionData($"{r.width}x{r.height}@{r.refreshRate.value:F2}"))
            .ToList();
        _resolutionDropdownView.SetOptions(options);
        _resolutionDropdownView.SetValue(_screenTuner.CurrentResolutionNumber);
    }

    public void Dispose() => _compositeDisposable?.Dispose();
}
