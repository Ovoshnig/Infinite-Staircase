using R3;
using System;
using VContainer.Unity;

public class ResumeButtonViewWindowSwitchMediator : IInitializable, IDisposable
{ 
    private readonly ResumeButtonView _resumeButtonView;
    private readonly WindowSwitch _windowSwitch;
    private readonly CompositeDisposable _compositeDisposable = new();

    public ResumeButtonViewWindowSwitchMediator(ResumeButtonView resumeButtonView, WindowSwitch windowSwitch)
    {
        _resumeButtonView = resumeButtonView;
        _windowSwitch = windowSwitch;
    }

    public void Initialize()
    {
        _resumeButtonView.ButtonClicked
            .Subscribe(_ => _windowSwitch.TryClose())
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();
}
