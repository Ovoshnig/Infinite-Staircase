using R3;
using System;
using VContainer.Unity;

public class CameraSwitch : IInitializable, IDisposable
{
    private readonly ReactiveProperty<bool> _isFirstPerson = new(true);
    private readonly CompositeDisposable _compositeDisposable = new();

    private PlayerInputHandler _playerInputHandler;

    public CameraSwitch(PlayerInputHandler playerInputHandler) => 
        _playerInputHandler = playerInputHandler;

    public ReadOnlyReactiveProperty<bool> IsFirstPerson => _isFirstPerson;

    public void Initialize()
    {
        _playerInputHandler.IsTogglePerspectivePressed
            .Where(isPressed => isPressed)
            .Subscribe(_ => _isFirstPerson.Value = !_isFirstPerson.Value)
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();
}
