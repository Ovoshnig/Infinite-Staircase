using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MainMenuScreenMediatorsLifetimeScope : LifetimeScope
{
    [SerializeField] private FullScreenToggleView _fullScreenToggleView;
    [SerializeField] private VSyncToggleView _vSyncToggleView;
    [SerializeField] private ResolutionDropdownView _resolutionDropdownView;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_fullScreenToggleView);
        builder.RegisterInstance(_vSyncToggleView);
        builder.RegisterInstance(_resolutionDropdownView);

        builder.RegisterEntryPoint<FullScreenToggleViewScreenTunerMediator>();
        builder.RegisterEntryPoint<VSyncToggleViewQualityTunerMediator>();
        builder.RegisterEntryPoint<ResolutionDropdownViewScreenTunerMediator>();
    }
}
