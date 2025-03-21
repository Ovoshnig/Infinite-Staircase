using UnityEngine;

public class CameraDetacher : MonoBehaviour
{
    private void Start() => transform.SetParent(null);
}
