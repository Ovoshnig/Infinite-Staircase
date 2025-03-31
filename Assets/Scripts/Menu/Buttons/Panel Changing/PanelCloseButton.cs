using R3;
using VContainer;

public sealed class PanelCloseButton : PanelChangeButton
{
    private WindowInputHandler _windowInputHandler;

    [Inject]
    public void Construct(WindowInputHandler windowInputHandler) => _windowInputHandler = windowInputHandler;

    protected override void Start()
    {
        base.Start();

        _windowInputHandler.CloseCurrentPressed
            .Skip(1)
            .Where(value => isActiveAndEnabled && value)
            .Subscribe(_ => Change())
            .AddTo(this);
    }
}
