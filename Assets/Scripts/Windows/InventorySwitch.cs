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
        _playerInput.Inventory.Switch.performed += OnSwitchPerformed;
    }

    private void Start() => _panel.SetActive(false);

    private void OnEnable() => _playerInput.Enable();

    private void OnDisable() => _playerInput.Disable();

    private void OnSwitchPerformed(InputAction.CallbackContext _)
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

        _panel.SetActive(true);
        _isOpen = true;
    }

    private void Close() 
    {
        if (!_windowTracker.TryCloseWindow())
            return;

        _panel.SetActive(false);
        _isOpen = false; 
    }
}
