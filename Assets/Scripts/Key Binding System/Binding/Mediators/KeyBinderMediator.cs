using R3;

public class KeyBinderMediator : Mediator
{
    private readonly KeyBinder _keyBinder;
    private readonly KeyBinderView _keyBinderView;

    public KeyBinderMediator(KeyBinder keyBinder, KeyBinderView keyBinderView)
    {
        _keyBinder = keyBinder;
        _keyBinderView = keyBinderView;
    }

    public override void Initialize()
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
            .AddTo(CompositeDisposable);
        _keyBinder.BindingText
            .Subscribe(_keyBinderView.SetBindingText)
            .AddTo(CompositeDisposable);
        _keyBinder.HasOverrides
            .Subscribe(_keyBinderView.SetResetButtonInteractable)
            .AddTo(CompositeDisposable);

        _keyBinderView.BindingClicked
            .Subscribe(_ => _keyBinder.StartListening())
            .AddTo(CompositeDisposable);
        _keyBinderView.ResetClicked
            .Subscribe(_ => _keyBinder.ResetBinding())
            .AddTo(CompositeDisposable);

        _keyBinder.Initialize();
    }
}
