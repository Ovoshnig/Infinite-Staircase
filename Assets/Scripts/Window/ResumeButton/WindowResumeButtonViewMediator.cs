using R3;

public class WindowResumeButtonViewMediator : Mediator
{
    private readonly Window _window;
    private readonly ResumeButtonView _resumeButtonView;

    public WindowResumeButtonViewMediator(Window window, ResumeButtonView resumeButtonView)
    {
        _window = window;
        _resumeButtonView = resumeButtonView;
    }

    public override void Initialize()
    {
        _resumeButtonView.Clicked
            .Subscribe(_ => _window.TryClose())
            .AddTo(CompositeDisposable);
    }
}
