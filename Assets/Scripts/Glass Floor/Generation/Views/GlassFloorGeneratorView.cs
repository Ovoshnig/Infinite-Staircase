using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GlassFloorGeneratorView : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private int _seed;

    public void GenerateInEditor()
    {
        GlassFloorGenerator builder = new(transform, _gameSettings.GlassFloorSettings, _seed);
        builder.GenerateFloor();
    }
}
