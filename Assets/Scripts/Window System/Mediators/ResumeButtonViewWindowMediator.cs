using R3;

public class ResumeButtonViewWindowMediator : Mediator
{ 
    private readonly ResumeButtonView _resumeButtonView;
    private readonly Window _window;

    public ResumeButtonViewWindowMediator(ResumeButtonView resumeButtonView, Window window)
    {
        _resumeButtonView = resumeButtonView;
        _window = window;
    }

    public override void Initialize()
    {
        _resumeButtonView.ButtonClicked
            .Subscribe(_ => _window.TryClose())
            .AddTo(CompositeDisposable);
    }
}
