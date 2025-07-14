using UnityEngine;

public class CameraDetacherView : MonoBehaviour
{
    private void Start() => transform.SetParent(null);
}
