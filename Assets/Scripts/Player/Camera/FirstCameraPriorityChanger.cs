using R3;

public sealed class FirstCameraPriorityChanger : CameraPriorityChanger
{
    protected override void Start()
    {
        base.Start();

        CameraSwitch.IsFirstPerson
            .Subscribe(isFirstPerson => Camera.Priority = isFirstPerson ? 1 : 0)
            .AddTo(CompositeDisposable);
    }
}
