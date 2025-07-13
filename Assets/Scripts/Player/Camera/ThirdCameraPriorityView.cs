public sealed class ThirdCameraPriorityView : CameraPriorityView
{
    public override void ApplyPriority(bool isFirstPerson) => SetPriority(isFirstPerson ? 0 : 1);
}
