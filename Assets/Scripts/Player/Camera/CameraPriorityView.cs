using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineCamera))]
public abstract class CameraPriorityView : MonoBehaviour
{
    private CinemachineCamera _cinemachineCamera = null;

    protected CinemachineCamera CinemachineCamera
    {
        get
        {
            if (_cinemachineCamera == null)
                _cinemachineCamera = GetComponent<CinemachineCamera>();

            return _cinemachineCamera;
        }
    }

    public abstract void ApplyPriority(bool isFirstPerson);

    protected void SetPriority(int value) => CinemachineCamera.Priority = value;
}
