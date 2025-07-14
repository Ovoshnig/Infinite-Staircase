using UnityEngine;
using VContainer;
using VContainer.Unity;

public class StaircaseLifetimeScope : LifetimeScope
{
    [SerializeField] private Transform _staircaseStartPoint;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_staircaseStartPoint);

        builder.Register<StairsLoader>(Lifetime.Singleton);

        builder.RegisterEntryPoint<StaircaseGenerator>(Lifetime.Singleton);
    }
}
