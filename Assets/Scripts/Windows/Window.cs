using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public abstract class Window : MonoBehaviour, IWindow
{
    [SerializeField] protected GameObject Panel;

    protected WindowTracker _windowTracker;
    protected PlayerInput PlayerInput;

    private bool _isOpen = false;

    [Inject]
    protected void Construct(WindowTracker windowTracker) => _windowTracker = windowTracker;

    public bool IsOpen => _isOpen;

    protected virtual void Awake()
    {
        PlayerInput = new PlayerInput();
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
