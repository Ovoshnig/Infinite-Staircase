using R3;

public class NewGameStarterSeedInputFieldViewMediator : Mediator
{
    private readonly NewGameStarter _newGameStarter;
    private readonly SeedInputFieldView _seedInputView;

    public NewGameStarterSeedInputFieldViewMediator(NewGameStarter newGameStarter,
        SeedInputFieldView seedInputView)
    {
        _newGameStarter = newGameStarter;
        _seedInputView = seedInputView;
    }

    public override void Initialize()
    {
        _seedInputView.Text
            .Subscribe(value => _newGameStarter.SetSeedText(value))
            .AddTo(CompositeDisposable);
    }
}
