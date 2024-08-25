using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public abstract class Window : MonoBehaviour, IWindow
{
    [SerializeField] private GameObject _panel;

    private PlayerInput _playerInput;
    private WindowTracker _windowTracker;
    private bool _isOpen = false;

    [Inject]
    protected void Construct(WindowTracker windowTracker) => _windowTracker = windowTracker;

    public bool IsOpen => _isOpen;

    protected GameObject Panel => _panel;
    protected PlayerInput PlayerInput => _playerInput;

    protected virtual void Awake()
    {
        _playerInput = new PlayerInput();
        InitializeInput();
    }

    protected virtual void Start() => Panel.SetActive(false);

    protected virtual void OnEnable() => PlayerInput.Enable();

    protected virtual void OnDisable() => PlayerInput.Disable();

    protected abstract void InitializeInput();

    protected virtual void OnSwitchPerformed(InputAction.CallbackContext _)
    {
        if (IsOpen)
            Close();
        else
            Open();
    }

    public virtual void Open()
    {
        if (!_windowTracker.TryOpenWindow()) 
            return;

        Panel.SetActive(true);
        _isOpen = true;
    }

    public virtual void Close()
    {
        if (!_windowTracker.TryCloseWindow()) 
            return;

        Panel.SetActive(false);
        _isOpen = false;
    }
}
