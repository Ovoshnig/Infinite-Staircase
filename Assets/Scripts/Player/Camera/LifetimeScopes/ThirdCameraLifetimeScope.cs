using UnityEngine;

public class ThirdCameraLifetimeScope : CameraLifetimeScope<ThirdCameraPriorityView>
{
    protected override Transform GetTrackingTarget(CharacterController characterController)
    {
        HeadCenterView headCenterView = characterController.GetComponentInChildren<HeadCenterView>();
        return headCenterView.transform;
    }
}
