using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Stair : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private Vector3 _size;

    public Vector3 Size
    {
        get
        {
            if (_size == default)
                _size = _meshRenderer.bounds.size;
            return _size;
        }
        private set
        {
            _size = value;
        }
    }

    private void OnValidate()
    {
        if (_meshRenderer == null)
            _meshRenderer = GetComponent<MeshRenderer>();
    }
}
