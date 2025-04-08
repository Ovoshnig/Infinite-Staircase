using R3;
using VContainer;

public sealed class PanelCloseButtonView : PanelChangeButtonView
{
    private MenuInputHandler _menuInputHandler;

    [Inject]
    public void Construct(MenuInputHandler menuInputHandler) => _menuInputHandler = menuInputHandler;

    protected override void Start()
    {
        base.Start();

        _menuInputHandler.CloseCurrentPressed
            .Skip(1)
            .Where(value => isActiveAndEnabled && value)
            .Subscribe(_ => Change())
            .AddTo(this);
    }
}
