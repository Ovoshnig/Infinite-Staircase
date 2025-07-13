using VContainer;
using VContainer.Unity;

public class PlayerCamerasLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register(resolver =>
        {
            FirstCameraPriorityView firstCameraView = resolver.Resolve<FirstCameraPriorityView>();
            return firstCameraView.GetComponent<FirstInputAxisView>();
        }, Lifetime.Singleton);

        builder.RegisterEntryPoint(resolver =>
        {
            CameraSwitch cameraSwitch = resolver.Resolve<CameraSwitch>();
            FirstCameraPriorityView firstCameraView = resolver.Resolve<FirstCameraPriorityView>();
            return new CameraSwitchPriorityViewMediator(cameraSwitch, firstCameraView);
        }, Lifetime.Scoped);

        builder.RegisterEntryPoint(resolver =>
        {
            SensitivitySliderModel sensitivitySliderModel = resolver.Resolve<SensitivitySliderModel>();
            FirstInputAxisView firstInputAxisView = resolver.Resolve<FirstInputAxisView>();
            return new SensitivitySliderInputAxisControllerMediator(sensitivitySliderModel, firstInputAxisView);
        }, Lifetime.Scoped);

        builder.RegisterEntryPoint(resolver =>
        {
            PlayerInputHandler playerInputHandler = resolver.Resolve<PlayerInputHandler>();
            FirstInputAxisView firstInputAxisView = resolver.Resolve<FirstInputAxisView>();
            return new PlayerInputHandlerAxisViewMediator(playerInputHandler, firstInputAxisView);
        }, Lifetime.Scoped);

        builder.Register(resolver =>
        {
            ThirdCameraPriorityView thirdCameraView = resolver.Resolve<ThirdCameraPriorityView>();
            return thirdCameraView.GetComponent<ThirdInputAxisView>();
        }, Lifetime.Singleton);

        builder.RegisterEntryPoint(resolver =>
        {
            CameraSwitch cameraSwitch = resolver.Resolve<CameraSwitch>();
            ThirdCameraPriorityView thirdCameraView = resolver.Resolve<ThirdCameraPriorityView>();
            return new CameraSwitchPriorityViewMediator(cameraSwitch, thirdCameraView);
        }, Lifetime.Scoped);

        builder.RegisterEntryPoint(resolver =>
        {
            SensitivitySliderModel sensitivitySliderModel = resolver.Resolve<SensitivitySliderModel>();
            ThirdInputAxisView thirdInputAxisView = resolver.Resolve<ThirdInputAxisView>();
            return new SensitivitySliderInputAxisControllerMediator(sensitivitySliderModel, thirdInputAxisView);
        }, Lifetime.Scoped);

        builder.RegisterEntryPoint(resolver =>
        {
            PlayerInputHandler playerInputHandler = resolver.Resolve<PlayerInputHandler>();
            ThirdInputAxisView thirdInputAxisView = resolver.Resolve<ThirdInputAxisView>();
            return new PlayerInputHandlerAxisViewMediator(playerInputHandler, thirdInputAxisView);
        }, Lifetime.Scoped);
    }
}
