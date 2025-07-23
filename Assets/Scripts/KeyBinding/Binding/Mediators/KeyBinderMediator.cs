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
            .Subscribe(OnListening)
            .AddTo(CompositeDisposable);
        _keyBinder.HasOverrides
            .Subscribe(OnHasOverrides)
            .AddTo(CompositeDisposable);
        _keyBinder.HasConflict
            .Subscribe(_keyBinderView.SetConflictImageEnabled)
            .AddTo(CompositeDisposable);
        _keyBinder.AnyButtonPressed
            .Subscribe(_keyBinderView.SetBindingText);

        _keyBinderView.BindingClicked
            .Subscribe(_ => _keyBinder.StartListening())
            .AddTo(CompositeDisposable);
        _keyBinderView.ResetClicked
            .Subscribe(_ => _keyBinder.ResetBinding())
            .AddTo(CompositeDisposable);
    }

    private void OnListening(bool isListening)
    {
        _keyBinderView.SetColor(isListening);
        _keyBinderView.SetBindingText(isListening ? _keyBinder.WaitInputText : _keyBinder.ActionDisplayName);
        _keyBinderView.SetResetButtonInteractable(!isListening && _keyBinder.HasOverrides.CurrentValue);
    }

    private void OnHasOverrides(bool hasOverrides)
    {
        _keyBinderView.SetResetButtonInteractable(hasOverrides);

        if (!hasOverrides)
            _keyBinderView.SetBindingText(_keyBinder.ActionDisplayName);
    }
}
