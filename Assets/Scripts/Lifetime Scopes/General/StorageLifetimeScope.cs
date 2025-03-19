using UnityEngine;
using VContainer.Unity;

public class StorageLifetimeScope : LifetimeScope
{
    [SerializeField] private StaircaseGenerator _staircaseGenerator;
    [SerializeField] private ProceduralGlassFloor _proceduralGlassFloor;

    private void Start()
    {
        Container.Inject(_staircaseGenerator);
        Container.Inject(_proceduralGlassFloor);
    }
}
