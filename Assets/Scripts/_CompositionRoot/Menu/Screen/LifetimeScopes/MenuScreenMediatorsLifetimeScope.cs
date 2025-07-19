using UnityEngine;
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
        Canvas canvas = Container.Resolve<Canvas>();

        FullScreenTunerToggleViewMediatorFactory fullScreenMediatorFactory = Container
            .Resolve<FullScreenTunerToggleViewMediatorFactory>();
        fullScreenMediatorFactory.CreateForEachView(canvas);

        VSyncTunerToggleViewMediatorFactory vSyncMediatorFactory = Container
            .Resolve<VSyncTunerToggleViewMediatorFactory>();
        vSyncMediatorFactory.CreateForEachView(canvas);

        ResolutionTunerDropdownViewMediatorFactory resolutionMediatorFactory = Container
            .Resolve<ResolutionTunerDropdownViewMediatorFactory>();
        resolutionMediatorFactory.CreateForEachView(canvas);
    }
}
