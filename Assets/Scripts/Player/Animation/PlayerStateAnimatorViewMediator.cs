using R3;

public class PlayerStateAnimatorViewMediator : Mediator
{
    private readonly PlayerState _playerState;
    private readonly PlayerAnimatorView _playerAnimatorView;

    public PlayerStateAnimatorViewMediator(PlayerState playerState,
        PlayerAnimatorView playerAnimatorView)
    {
        _playerState = playerState;
        _playerAnimatorView = playerAnimatorView;
    }

    public override void Initialize()
    {
        _playerState.Walking
            .Subscribe(isWalking => _playerAnimatorView.SetWalking(isWalking))
            .AddTo(CompositeDisposable);
        _playerState.Running
            .Subscribe(isRunning => _playerAnimatorView.SetRunning(isRunning))
            .AddTo(CompositeDisposable);
        _playerState.Grounded
            .Subscribe(isGrounded => _playerAnimatorView.SetGrounded(isGrounded))
            .AddTo(CompositeDisposable);
        _playerState.Jumped
            .Subscribe(_ => _playerAnimatorView.SetJumped())
            .AddTo(CompositeDisposable);
    }
}
