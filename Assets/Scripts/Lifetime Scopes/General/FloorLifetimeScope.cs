using UnityEngine;
using VContainer.Unity;

public class FloorLifetimeScope : LifetimeScope
{
    [SerializeField] private ProceduralGlassFloor _proceduralGlassFloor;

    private void Start() => Container.Inject(_proceduralGlassFloor);
}
