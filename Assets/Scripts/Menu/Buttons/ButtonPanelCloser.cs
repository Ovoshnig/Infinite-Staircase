using R3;
using VContainer;

public sealed class ButtonPanelCloser : ButtonPanelChanger
{
    private WindowInputHandler _windowInputHandler;

    [Inject]
    public void Construct(WindowInputHandler windowInputHandler) => _windowInputHandler = windowInputHandler;

    protected override void OnEnable()
    {
        base.OnEnable();

        _windowInputHandler.CloseCurrentPressed
            .Where(value => value)
            .Subscribe(_ => Change())
            .AddTo(CompositeDisposable);
    }
}
