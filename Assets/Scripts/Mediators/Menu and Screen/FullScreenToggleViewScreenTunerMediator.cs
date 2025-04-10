using R3;
using System;
using VContainer.Unity;

public class FullScreenToggleViewScreenTunerMediator : IInitializable, IDisposable
{
    private readonly FullScreenToggleView _fullScreenToggleView;
    private readonly ScreenTuner _screenTuner;
    private readonly CompositeDisposable _compositeDisposable = new();

    public FullScreenToggleViewScreenTunerMediator(FullScreenToggleView fullScreenToggleView, 
        ScreenTuner screenTuner)
    {
        _fullScreenToggleView = fullScreenToggleView;
        _screenTuner = screenTuner;
    }

    public void Initialize()
    {
        _fullScreenToggleView.IsOn
            .Skip(1)
            .Subscribe(value => 
            {
                if (value)
                    _screenTuner.EnableFullScreen();
                else
                    _screenTuner.DisableFullScreen();
            })
            .AddTo(_compositeDisposable);
        _screenTuner.IsFullScreen
            .Subscribe(value =>
            {
                if (value)
                    _fullScreenToggleView.Enable();
                else
                    _fullScreenToggleView.Disable();
            })
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();
}
