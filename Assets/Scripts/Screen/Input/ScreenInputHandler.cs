using R3;
using System;
using VContainer.Unity;

public class ScreenInputHandler : IInitializable, IDisposable
{
    private readonly InputActions.ScreenActions _screenActions;
    private readonly CompositeDisposable _compositeDisposable = new();

    public ScreenInputHandler(InputActions inputActions) => _screenActions = inputActions.Screen;

    public ReadOnlyReactiveProperty<bool> SwitchFullScreenPressed { get; private set; }
    public ReadOnlyReactiveProperty<bool> PassSplashImagePressed { get; private set; }

    public void Initialize()
    {
        _screenActions.Enable();

        SwitchFullScreenPressed = _screenActions.SwitchFullScreen
            .AsButtonStream()
            .AddTo(_compositeDisposable);
        PassSplashImagePressed = _screenActions.PassSplashImage
            .AsButtonStream()
            .AddTo(_compositeDisposable);
    }

    public void Dispose()
    {
        _compositeDisposable.Dispose();
        _screenActions.Disable();
    }
}
