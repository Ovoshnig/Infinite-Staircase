using UnityEngine;
using VContainer.Unity;

public class GlassFloorLifetimeScope : LifetimeScope
{
    [SerializeField] private GlassFloorGenerator _glassFloorGenerator;

    private void Start() => Container.Inject(_glassFloorGenerator);
}
