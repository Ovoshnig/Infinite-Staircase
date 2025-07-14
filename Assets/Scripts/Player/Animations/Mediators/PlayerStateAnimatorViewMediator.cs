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
        _playerState.IsWalking
            .Subscribe(isWalking => _playerAnimatorView.SetWalking(isWalking))
            .AddTo(CompositeDisposable);
        _playerState.IsRunning
            .Subscribe(isRunning => _playerAnimatorView.SetRunning(isRunning))
            .AddTo(CompositeDisposable);
        _playerState.IsGrounded
            .Subscribe(isGrounded => _playerAnimatorView.SetGrounded(isGrounded))
            .AddTo(CompositeDisposable);
        _playerState.Jumped
            .Subscribe(_ => _playerAnimatorView.SetJumped())
            .AddTo(CompositeDisposable);
    }
}
