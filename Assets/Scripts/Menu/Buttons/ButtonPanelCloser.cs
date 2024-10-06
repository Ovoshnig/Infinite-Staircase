using R3;
using Zenject;

public sealed class ButtonPanelCloser : ButtonPanelChanger
{
    private readonly CompositeDisposable _compositeDisposable = new();
    private WindowInputHandler _inputHandler;

    [Inject]
    private void Construct(WindowInputHandler inputHandler) => _inputHandler = inputHandler;

    protected override void OnEnable()
    {
        base.OnEnable();

        _inputHandler.CloseCurrentPressed
            .Where(value => value)
            .Subscribe(_ => Change())
            .AddTo(_compositeDisposable);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _compositeDisposable?.Dispose();
    }
}
