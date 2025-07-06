using R3;
using System;
using VContainer.Unity;

public class ResumeButtonViewWindowMediator : IInitializable, IDisposable
{ 
    private readonly ResumeButtonView _resumeButtonView;
    private readonly Window _window;
    private readonly CompositeDisposable _compositeDisposable = new();

    public ResumeButtonViewWindowMediator(ResumeButtonView resumeButtonView, Window window)
    {
        _resumeButtonView = resumeButtonView;
        _window = window;
    }

    public void Initialize()
    {
        _resumeButtonView.ButtonClicked
            .Subscribe(_ => _window.TryClose())
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();
}
