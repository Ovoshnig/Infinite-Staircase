using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMoverView : MonoBehaviour
{
    private CharacterController _characterController;
    
    private CharacterController CharacterController
    {
        get
        {
            if (_characterController == null)
                _characterController = GetComponent<CharacterController>();

            return _characterController;
        }
    }

    public void Move(Vector3 motion) => CharacterController.Move(motion);

    public void SetEulerAngles(Vector3 eulerAngles) => transform.eulerAngles = eulerAngles;
}
