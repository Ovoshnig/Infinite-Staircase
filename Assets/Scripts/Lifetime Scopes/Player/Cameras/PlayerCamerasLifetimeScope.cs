using VContainer;
using VContainer.Unity;

public class PlayerCamerasLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint(resolver =>
        {
            CameraSwitch cameraSwitch = resolver.Resolve<CameraSwitch>();
            FirstCameraPriorityView firstCameraView = resolver.Resolve<FirstCameraPriorityView>();
            return new CameraSwitchPriorityViewMediator(cameraSwitch, firstCameraView);
        }, Lifetime.Scoped);

        builder.RegisterEntryPoint(resolver =>
        {
            SensitivitySliderModel sensitivitySliderModel = resolver.Resolve<SensitivitySliderModel>();
            FirstCameraPriorityView firstCameraView = resolver.Resolve<FirstCameraPriorityView>();
            InputAxisController firstAxisController = firstCameraView.GetComponent<InputAxisController>();
            return new SensitivitySliderInputAxisControllerMediator(sensitivitySliderModel, firstAxisController);
        }, Lifetime.Scoped);

        builder.RegisterEntryPoint(resolver =>
        {
            CameraSwitch cameraSwitch = resolver.Resolve<CameraSwitch>();
            ThirdCameraPriorityView thirdCameraView = resolver.Resolve<ThirdCameraPriorityView>();
            return new CameraSwitchPriorityViewMediator(cameraSwitch, thirdCameraView);
        }, Lifetime.Scoped);

        builder.RegisterEntryPoint(resolver =>
        {
            SensitivitySliderModel sensitivitySliderModel = resolver.Resolve<SensitivitySliderModel>();
            ThirdCameraPriorityView thirdCameraView = resolver.Resolve<ThirdCameraPriorityView>();
            InputAxisController thirdAxisController = thirdCameraView.GetComponent<InputAxisController>();
            return new SensitivitySliderInputAxisControllerMediator(sensitivitySliderModel, thirdAxisController);
        }, Lifetime.Scoped);
    }
}
