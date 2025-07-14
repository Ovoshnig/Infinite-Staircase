using VContainer;
using VContainer.Unity;

public class MenuScreenMediatorsLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<FullScreenTunerToggleViewMediatorFactory>(Lifetime.Singleton);
        builder.Register<VSyncTunerToggleViewMediatorFactory>(Lifetime.Singleton);
        builder.Register<ResolutionTunerDropdownViewMediatorFactory>(Lifetime.Singleton);
    }

    private void Start()
    {
        FullScreenTunerToggleViewMediatorFactory fullScreenMediatorFactory = Container
            .Resolve<FullScreenTunerToggleViewMediatorFactory>();
        fullScreenMediatorFactory.CreateForEachView(Container);

        VSyncTunerToggleViewMediatorFactory vSyncMediatorFactory = Container
            .Resolve<VSyncTunerToggleViewMediatorFactory>();
        vSyncMediatorFactory.CreateForEachView(Container);

        ResolutionTunerDropdownViewMediatorFactory resolutionMediatorFactory = Container
            .Resolve<ResolutionTunerDropdownViewMediatorFactory>();
        resolutionMediatorFactory.CreateForEachView(Container);
    }
}
