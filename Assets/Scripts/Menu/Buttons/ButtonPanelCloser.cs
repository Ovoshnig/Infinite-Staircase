using R3;
using VContainer;

public sealed class ButtonPanelCloser : ButtonPanelChanger
{
    private WindowInputHandler _windowInputHandler;

    [Inject]
    public void Construct(WindowInputHandler windowInputHandler) => _windowInputHandler = windowInputHandler;

    protected override void Start()
    {
        base.Start();

        _windowInputHandler.CloseCurrentPressed
            .Where(value => isActiveAndEnabled && value)
            .Subscribe(_ => Change())
            .AddTo(this);
    }
}
