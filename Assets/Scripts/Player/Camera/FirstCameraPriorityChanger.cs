using R3;

public sealed class FirstCameraPriorityChanger : CameraPriorityChanger
{
    protected override void Start()
    {
        base.Start();

        CameraSwitch.IsFirstPerson
            .Subscribe(value => Camera.Priority = value ? 1 : 0)
            .AddTo(CompositeDisposable);
    }
}
