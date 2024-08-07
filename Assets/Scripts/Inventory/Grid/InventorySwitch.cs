using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InventorySwitch : MonoBehaviour
{
    [SerializeField] private GameObject _panel;

    private PlayerInput _playerInput;
    private WindowTracker _windowTracker;
    private bool _isOpen = false;

    [Inject]
    private void Construct(WindowTracker windowTracker) => _windowTracker = windowTracker;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.Inventory.OpenOrClose.performed += OpenOrClosePerformed;
    }

    private void Start() => _panel.SetActive(false);

    private void OnEnable() => _playerInput.Enable();

    private void OnDisable() => _playerInput.Disable();

    private void OpenOrClosePerformed(InputAction.CallbackContext _)
    {
        if (_isOpen)
            Close();
        else
            Open();
    }

    private void Open()
    {
        if (!_windowTracker.TryOpenWindow(_panel))
            return;

        _isOpen = true;
    }

    private void Close() 
    {
        _windowTracker.TryCloseWindow();

        _isOpen = false; 
    }
}
