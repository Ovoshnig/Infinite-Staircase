public class PlayerInputHandlerAxisViewMediatorFactory
    : MediatorViewFactory<PlayerInputHandlerAxisViewMediator, InputAxisView>
{
    private readonly PlayerInputHandler _playerInputHandler;

    public PlayerInputHandlerAxisViewMediatorFactory(PlayerInputHandler playerInputHandler) =>
        _playerInputHandler = playerInputHandler;

    protected override PlayerInputHandlerAxisViewMediator CreateMediatorInstance(InputAxisView view) =>
        new(_playerInputHandler, view);
}
