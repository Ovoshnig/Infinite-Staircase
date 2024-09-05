using R3;
using System;
using Zenject;

public sealed class ButtonPanelCloser : ButtonPanelChanger
{
    private WindowInputHandler _inputHandler;
    private IDisposable _disposable;

    [Inject]
    private void Construct(WindowInputHandler inputHandler) => _inputHandler = inputHandler;

    protected override void OnEnable()
    {
        base.OnEnable();

        _disposable = _inputHandler.CloseCurrentPressed
            .Where(value => value)
            .Subscribe(_ => Change());
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _disposable?.Dispose();
    }
}
