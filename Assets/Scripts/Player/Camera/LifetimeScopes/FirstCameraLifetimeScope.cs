using UnityEngine;

public class FirstCameraLifetimeScope : CameraLifetimeScope<FirstCameraPriorityView>
{
    protected override Transform GetTrackingTarget(CharacterController characterController)
    {
        EyeCenterView eyeCenterView = characterController.GetComponentInChildren<EyeCenterView>();
        return eyeCenterView.transform;
    }
}
