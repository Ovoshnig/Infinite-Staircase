using R3;

public sealed class ThirdCameraPriorityChanger : CameraPriorityChanger
{
    protected override void Start()
    {
        base.Start();

        CameraSwitch.IsFirstPerson
            .Subscribe(value => Camera.Priority = value ? 0 : 1)
            .AddTo(CompositeDisposable);
    }
}
