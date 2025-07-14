using R3;

public class CameraSwitchPriorityViewMediator : Mediator
{
    private readonly CameraSwitch _cameraSwitch;
    private readonly CameraPriorityView _cameraPriorityView;

    public CameraSwitchPriorityViewMediator(CameraSwitch cameraSwitch,
        CameraPriorityView cameraPriorityView)
    {
        _cameraSwitch = cameraSwitch;
        _cameraPriorityView = cameraPriorityView;
    }

    public override void Initialize()
    {
        _cameraSwitch.IsFirstPerson
            .Subscribe(_cameraPriorityView.ApplyPriority)
            .AddTo(CompositeDisposable);
    }
}
