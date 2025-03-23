using System;
using UnityEngine;
using VContainer;

public abstract class WindowSwitch : MonoBehaviour, IWindowSwitch
{
    [SerializeField] private GameObject _panel;

    private WindowInputHandler _windowInputHandler;
    private WindowTracker _windowTracker;
    private bool _isOpen = false;

    [Inject]
    public void Construct(WindowInputHandler windowInputHandler, WindowTracker windowTracker)
    {
        _windowInputHandler = windowInputHandler;
        _windowTracker = windowTracker;
    }

    public bool IsOpen => _isOpen;

    protected IDisposable Disposable { get; set; }
    protected GameObject Panel => _panel;
    protected WindowInputHandler WindowInputHandler => _windowInputHandler;

    protected virtual void Awake() => InitializeInput();

    protected virtual void Start() => Panel.SetActive(false);

    protected virtual void OnDestroy() => Disposable?.Dispose();

    protected abstract void InitializeInput();

    protected virtual void Switch()
    {
        if (IsOpen)
            Close();
        else
            Open();
    }

    public virtual bool Open()
    {
        if (!_windowTracker.TryOpenWindow(this, GetType()))
            return false;

        Panel.SetActive(true);
        _isOpen = true;

        return true;
    }

    public virtual bool Close()
    {
        if (!_windowTracker.TryCloseWindow()) 
            return false;

        Panel.SetActive(false);
        _isOpen = false;

        return true;
    }
}
