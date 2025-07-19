public class CameraSwitchPriorityViewMediatorFactory
    : MediatorViewFactory<CameraSwitchPriorityViewMediator, CameraPriorityView>
{
    private readonly CameraSwitch _cameraSwitch;

    public CameraSwitchPriorityViewMediatorFactory(CameraSwitch cameraSwitch) =>
        _cameraSwitch = cameraSwitch;

    protected override CameraSwitchPriorityViewMediator CreateMediatorInstance(CameraPriorityView view) =>
        new(_cameraSwitch, view);
}
