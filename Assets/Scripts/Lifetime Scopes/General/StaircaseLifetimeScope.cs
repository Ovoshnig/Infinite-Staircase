using UnityEngine;
using VContainer;
using VContainer.Unity;

public class StaircaseLifetimeScope : LifetimeScope
{
    [SerializeField] private StaircaseGenerator _staircaseGenerator;

    protected override void Configure(IContainerBuilder builder) => 
        builder.Register<StairsLoader>(Lifetime.Singleton);

    private void Start() => Container.Inject(_staircaseGenerator);
}
