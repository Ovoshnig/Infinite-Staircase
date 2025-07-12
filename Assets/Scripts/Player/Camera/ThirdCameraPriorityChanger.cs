using R3;

public sealed class ThirdCameraPriorityChanger : CameraPriorityChanger
{
    protected override void Start()
    {
        base.Start();

        CameraSwitch.IsFirstPerson
            .Subscribe(isFirstPerson => Camera.Priority = isFirstPerson ? 0 : 1)
            .AddTo(CompositeDisposable);
    }
}
