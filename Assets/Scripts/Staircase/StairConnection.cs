using UnityEngine;

[CreateAssetMenu(fileName = nameof(StairConnection), menuName = "Scriptable Objects/StairConnection")]
public class StairConnection : ScriptableObject
{
    public GameObject Prefab;
    public int MinCount;
    public int MaxCount;
    public bool CanBeInStart;

    private GameObject _stair1;
    private GameObject _stair2;
    private Vector3? _positionDifference;
    private Vector3? _rotationDifference;

    public GameObject Stair1
    {
        get
        {
            if (_stair1 == null)
                _stair1 = Prefab.transform.GetChild(0).gameObject;
            return _stair1;
        }
    }

    public GameObject Stair2
    {
        get
        {
            if (_stair2 == null)
                _stair2 = Prefab.transform.GetChild(1).gameObject;
            return _stair2;
        }
    }

    public Vector3 PositionDifference 
    {
        get
        {
            if (_positionDifference == null)
                _positionDifference = Stair2.transform.position - Stair1.transform.position;
            return (Vector3)_positionDifference;
        }
    }

    public Vector3 RotationDifference
    {
        get
        {
            if (_rotationDifference == null)
                _rotationDifference = Stair2.transform.rotation.eulerAngles - Stair1.transform.rotation.eulerAngles;
            return (Vector3)_rotationDifference;
        }
    }
}
