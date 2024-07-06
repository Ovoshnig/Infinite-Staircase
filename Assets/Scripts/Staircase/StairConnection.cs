using UnityEngine;

[CreateAssetMenu(fileName = nameof(StairConnection), menuName = "Scriptable Objects/StairConnection")]
public class StairConnection : ScriptableObject
{
    public GameObject Prefab;
    public int MinCount;
    public int MaxCount;
    public bool CanBeInStart;
    public GameObject Stair1;
    public GameObject Stair2;

    private Vector3 _positionDifference;
    private Vector3 _rotationDifference;

    public Vector3 PositionDifference
    {
        get
        {
            if (_positionDifference == default)
                _positionDifference = Stair2.transform.position - Stair1.transform.position;
            return _positionDifference;
        }
    }

    public Vector3 RotationDifference
    {
        get
        {
            if (_rotationDifference == default)
                _rotationDifference = Stair2.transform.rotation.eulerAngles - Stair1.transform.rotation.eulerAngles;
            return _rotationDifference;
        }
    }

    private void OnValidate()
    {
        if (Stair1 == null)
            Stair1 = Prefab.transform.GetChild(0).gameObject;
        if (Stair2 == null)
            Stair2 = Prefab.transform.GetChild(1).gameObject;
    }
}
