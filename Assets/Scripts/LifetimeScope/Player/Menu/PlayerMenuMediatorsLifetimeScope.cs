using VContainer;
using VContainer.Unity;

public class PlayerMenuMediatorsLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint(resolver =>
        {
            SensitivitySliderModel sensitivitySliderModel = resolver.Resolve<SensitivitySliderModel>();
            FirstInputAxisView firstInputAxisView = resolver.Resolve<FirstInputAxisView>();
            return new SensitivitySliderInputAxisControllerMediator(sensitivitySliderModel, firstInputAxisView);
        }, Lifetime.Scoped);

        builder.RegisterEntryPoint(resolver =>
        {
            SensitivitySliderModel sensitivitySliderModel = resolver.Resolve<SensitivitySliderModel>();
            ThirdInputAxisView thirdInputAxisView = resolver.Resolve<ThirdInputAxisView>();
            return new SensitivitySliderInputAxisControllerMediator(sensitivitySliderModel, thirdInputAxisView);
        }, Lifetime.Scoped);
    }
}
