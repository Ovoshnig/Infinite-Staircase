using UnityEngine;
using UnityEngine.InputSystem;

public class InventorySwitch : MonoBehaviour
{
    [SerializeField] private GameObject _panel;

    private PlayerInput _playerInput;
    private bool _isOpen = false;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.Inventory.OpenOrClose.performed += OpenOrClosePerformed;
    }

    private void Start() => _panel.SetActive(false);

    private void OnEnable() => _playerInput.Enable();

    private void OnDisable() => _playerInput.Disable();

    private void OpenOrClosePerformed(InputAction.CallbackContext obj)
    {
        _isOpen = !_isOpen;
        _panel.SetActive(_isOpen);

        Cursor.lockState = _isOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = _isOpen;
    }
}
