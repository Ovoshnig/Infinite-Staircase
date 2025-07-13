using R3;

public class PlayerInputHandlerAxisViewMediator : Mediator
{
    private readonly PlayerInputHandler _playerInputHandler;
    private readonly InputAxisView _inputAxisView;

    public PlayerInputHandlerAxisViewMediator(PlayerInputHandler playerInputHandler,
        InputAxisView inputAxisView)
    {
        _playerInputHandler = playerInputHandler;
        _inputAxisView = inputAxisView;
    }

    public override void Initialize()
    {
        Observable.EveryUpdate()
            .Where(_ => _playerInputHandler.LookAction != null)
            .Subscribe(_ => _inputAxisView.ProcessInput(_playerInputHandler.LookAction))
            .AddTo(CompositeDisposable);
        Observable.EveryUpdate()
            .Where(_ => _playerInputHandler.ZoomAction != null)
            .Subscribe(_ => _inputAxisView.ProcessInput(_playerInputHandler.ZoomAction))
            .AddTo(CompositeDisposable);
    }
}
