using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GlassFloorLifetimeScope : LifetimeScope
{
    [SerializeField] private Transform _glassFloorStartPoint;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_glassFloorStartPoint);

        builder.RegisterEntryPoint<GlassFloorGenerator>(Lifetime.Singleton);
    }
}
