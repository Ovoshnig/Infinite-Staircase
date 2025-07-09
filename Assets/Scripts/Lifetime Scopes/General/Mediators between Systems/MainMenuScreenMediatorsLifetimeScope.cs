using VContainer;
using VContainer.Unity;

public class MainMenuScreenMediatorsLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        MainMenuCanvasView canvasView = FindFirstObjectByType<MainMenuCanvasView>();
        FullScreenToggleView fullScreenToggleView = canvasView
            .GetComponentInChildren<FullScreenToggleView>(true);

        if (fullScreenToggleView != null)
        {
            builder.RegisterInstance(fullScreenToggleView);
            builder.RegisterEntryPoint<ScreenTunerFullScreenToggleViewMediator>();
        }

        VSyncToggleView vSyncToggleView = canvasView
            .GetComponentInChildren<VSyncToggleView>(true);

        if (vSyncToggleView != null)
        {
            builder.RegisterInstance(vSyncToggleView);
            builder.RegisterEntryPoint<QualityTunerVSyncToggleViewMediator>();
        }

        ResolutionDropdownView resolutionDropdownView = canvasView
            .GetComponentInChildren<ResolutionDropdownView>(true);

        if (resolutionDropdownView != null)
        {
            builder.RegisterInstance(resolutionDropdownView);
            builder.RegisterEntryPoint<ScreenTunerResolutionDropdownViewMediator>();
        }
    }
}
