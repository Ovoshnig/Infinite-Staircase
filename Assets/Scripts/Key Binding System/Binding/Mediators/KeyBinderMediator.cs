using R3;
using System;
using VContainer.Unity;

public class KeyBinderMediator : IInitializable, IDisposable
{
    private readonly KeyBinder _keyBinder;
    private readonly KeyBinderView _keyBinderView;
    private readonly CompositeDisposable _compositeDisposable = new();

    public KeyBinderMediator(KeyBinder keyBinder, KeyBinderView keyBinderView)
    {
        _keyBinder = keyBinder;
        _keyBinderView = keyBinderView;
    }

    public void Initialize()
    {
        _keyBinder.IsListening
            .Subscribe(value =>
            {
                _keyBinderView.SetColor(value);

                if (value)
                    _keyBinderView.SetResetButtonInteractable(false);
                else
                    _keyBinderView.SetResetButtonInteractable(_keyBinder.HasOverrides.CurrentValue);
            })
            .AddTo(_compositeDisposable);
        _keyBinder.BindingText
            .Subscribe(_keyBinderView.SetBindingText)
            .AddTo(_compositeDisposable);
        _keyBinder.HasOverrides
            .Subscribe(_keyBinderView.SetResetButtonInteractable)
            .AddTo(_compositeDisposable);

        _keyBinderView.BindingClicked
            .Subscribe(_ => _keyBinder.StartListening())
            .AddTo(_compositeDisposable);
        _keyBinderView.ResetClicked
            .Subscribe(_ => _keyBinder.ResetBinding())
            .AddTo(_compositeDisposable);

        _keyBinder.Initialize();
    }

    public void Dispose() => _compositeDisposable?.Dispose();
}
