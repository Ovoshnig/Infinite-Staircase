using R3;

public class KeyBinderMediator : Mediator
{
    private readonly KeyBinder _keyBinder;
    private readonly KeyBinderView _keyBinderView;

    public KeyBinderMediator(KeyBinder keyBinder,
        KeyBinderView keyBinderView)
    {
        _keyBinder = keyBinder;
        _keyBinderView = keyBinderView;
    }

    public override void Initialize()
    {
        _keyBinder.IsListening
            .Subscribe(OnListeningChanged)
            .AddTo(CompositeDisposable);
        _keyBinder.HasOverrides
            .Subscribe(_keyBinderView.SetResetButtonInteractable)
            .AddTo(CompositeDisposable);
        _keyBinder.HasConflict
            .Subscribe(_keyBinderView.SetConflictImageEnabled)
            .AddTo(CompositeDisposable);
        _keyBinder.BindingText
            .Subscribe(_keyBinderView.SetBindingText)
            .AddTo(CompositeDisposable);

        _keyBinderView.BindingClicked
            .Subscribe(_ => _keyBinder.StartListening())
            .AddTo(CompositeDisposable);
        _keyBinderView.ResetClicked
            .Subscribe(_ => _keyBinder.RemoveBindingOverrides())
            .AddTo(CompositeDisposable);
    }

    private void OnListeningChanged(bool isListening)
    {
        _keyBinderView.SetBindingButtonInteractable(!isListening);
        _keyBinderView.SetResetButtonInteractable(!isListening && 
            _keyBinder.HasOverrides.CurrentValue);
        _keyBinderView.SetColor(isListening);
    }
}
