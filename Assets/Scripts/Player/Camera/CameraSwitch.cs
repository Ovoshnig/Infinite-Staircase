using R3;
using System;
using VContainer;
using VContainer.Unity;

public class CameraSwitch : IInitializable, IDisposable
{
    private readonly ReactiveProperty<bool> _isFirstPerson = new(true);
    private readonly CompositeDisposable _compositeDisposable = new();
    private PlayerInputHandler _playerInputHandler;

    [Inject]
    public void Construct(PlayerInputHandler playerInputHandler) => 
        _playerInputHandler = playerInputHandler;

    public ReadOnlyReactiveProperty<bool> IsFirstPerson => _isFirstPerson;

    public void Initialize()
    {
        _playerInputHandler.IsTogglePerspectivePressed
            .Where(value => value)
            .Subscribe(_ => _isFirstPerson.Value = !_isFirstPerson.Value)
            .AddTo(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable?.Dispose();
}
