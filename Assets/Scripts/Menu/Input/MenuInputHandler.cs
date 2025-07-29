using R3;
using System;
using VContainer.Unity;

public class MenuInputHandler : IInitializable, IDisposable
{
    private readonly InputActions.MenuActions _menuActions;
    private readonly CompositeDisposable _compositeDisposable = new();

    public MenuInputHandler(InputActions inputActions) => _menuActions = inputActions.Menu;

    public ReadOnlyReactiveProperty<bool> CloseCurrentPressed { get; private set; }

    public void Initialize()
    {
        _menuActions.Enable();

        CloseCurrentPressed = _menuActions.CloseCurrent
            .AsButtonStream()
            .AddTo(_compositeDisposable);
    }

    public void Dispose()
    {
        _compositeDisposable.Dispose();
        _menuActions.Disable();
    }
}
