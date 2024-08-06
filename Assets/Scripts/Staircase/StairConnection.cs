using UnityEngine;

[CreateAssetMenu(fileName = nameof(StairConnection), menuName = "Scriptable Objects/StairConnection")]
public class StairConnection : ScriptableObject
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private GameObject _stair1;
    [SerializeField] private GameObject _stair2;

    private Vector3 _positionDifference;
    private Vector3 _rotationDifference;

    [field: SerializeField] public bool CanBeInStart { get; private set; }
    [field: SerializeField] public int MinCount { get; private set; }
    [field: SerializeField] public int MaxCount { get; private set; }

    public Vector3 PositionDifference
    {
        get
        {
            GetStairs();

            if (_positionDifference == default)
                _positionDifference = _stair2.transform.position - _stair1.transform.position;

            return _positionDifference;
        }
    }

    public Vector3 RotationDifference
    {
        get
        {
            GetStairs();

            if (_rotationDifference == default)
                _rotationDifference = _stair2.transform.rotation.eulerAngles - _stair1.transform.rotation.eulerAngles;

            return _rotationDifference;
        }
    }

    private void GetStairs()
    {
        if (_stair1 == null)
            _stair1 = _prefab.transform.GetChild(0).gameObject;

        if (_stair2 == null)
            _stair2 = _prefab.transform.GetChild(1).gameObject;
    }
}
