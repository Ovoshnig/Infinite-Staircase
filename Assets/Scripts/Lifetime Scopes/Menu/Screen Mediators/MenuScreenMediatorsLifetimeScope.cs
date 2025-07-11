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

        FullScreenToggleView[] fullScreenViews = canvas
            .GetComponentsInChildren<FullScreenToggleView>(true);
        FullScreenTunerToggleViewMediatorFactory fullScreenMediatorFactory = Container
            .Resolve<FullScreenTunerToggleViewMediatorFactory>();

        foreach (FullScreenToggleView fullScreenView in fullScreenViews)
            fullScreenMediatorFactory.Create(fullScreenView);

        VSyncToggleView[] vSyncViews = canvas
            .GetComponentsInChildren<VSyncToggleView>(true);
        VSyncTunerToggleViewMediatorFactory vSyncMediatorFactory = Container
            .Resolve<VSyncTunerToggleViewMediatorFactory>();

        foreach (var vSyncView in vSyncViews)
            vSyncMediatorFactory.Create(vSyncView);

        ResolutionDropdownView[] resolutionViews = canvas
            .GetComponentsInChildren<ResolutionDropdownView>(true);
        ResolutionTunerDropdownViewMediatorFactory resolutionMediatorFactory = Container
            .Resolve<ResolutionTunerDropdownViewMediatorFactory>();

        foreach(var resolutionView in resolutionViews)
            resolutionMediatorFactory.Create(resolutionView);
    }
}
