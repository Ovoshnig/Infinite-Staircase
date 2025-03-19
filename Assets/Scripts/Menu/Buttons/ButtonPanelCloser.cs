using R3;
using VContainer;

public sealed class ButtonPanelCloser : ButtonPanelChanger
{
    private CompositeDisposable _compositeDisposable = new();
    private WindowInputHandler _windowInputHandler;

    [Inject]
    public void Construct(WindowInputHandler windowInputHandler) => _windowInputHandler = windowInputHandler;

    protected override void OnEnable()
    {
        base.OnEnable();

        _windowInputHandler.CloseCurrentPressed
            .Where(value => value)
            .Subscribe(_ => Change())
            .AddTo(_compositeDisposable);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _compositeDisposable?.Dispose();
        _compositeDisposable = new CompositeDisposable();
    }
}
