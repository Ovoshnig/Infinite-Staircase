using VContainer;
using VContainer.Unity;

public class PlayerCameraMenuLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder) =>
        builder.Register<SensitivitySliderInputAxisViewMediatorFactory>(Lifetime.Singleton);

    private void Start()
    {
        InputAxisView inputAxisView = Container.Resolve<InputAxisView>();
        SensitivitySliderInputAxisViewMediatorFactory mediatorFactory = Container
            .Resolve<SensitivitySliderInputAxisViewMediatorFactory>();
        mediatorFactory.Create(inputAxisView);
    }
}
