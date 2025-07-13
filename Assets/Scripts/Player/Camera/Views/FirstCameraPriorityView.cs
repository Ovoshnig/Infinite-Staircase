public sealed class FirstCameraPriorityView : CameraPriorityView
{
    public override void ApplyPriority(bool isFirstPerson) => SetPriority(isFirstPerson ? 1 : 0);
}
