using R3;

public class CameraSwitchPlayerScopeViewMediator : Mediator
{
    private readonly CameraSwitch _cameraSwitch;
    private readonly PlayerScopeView _playerScopeView;

    public CameraSwitchPlayerScopeViewMediator(CameraSwitch cameraSwitch, PlayerScopeView playerScopeView)
    {
        _cameraSwitch = cameraSwitch;
        _playerScopeView = playerScopeView;
    }

    public override void Initialize()
    {
        _cameraSwitch.IsFirstPerson
            .Subscribe(_playerScopeView.SetActive)
            .AddTo(CompositeDisposable);
    }
}
