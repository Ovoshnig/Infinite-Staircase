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
        _isOpen = !_isOpen;

        if (_isOpen)
        {
            if (!_windowTracker.TryOpenWindow(_panel))
            {
                _isOpen = false;
                return;
            }
        }
        else
        {
            _windowTracker.CloseWindow();
        }

        _panel.SetActive(_isOpen);
    }
}
