using R3;

public class SeedInputFieldNewGameStarterMediator : Mediator
{
    private readonly SeedInputFieldView _seedInputView;
    private readonly NewGameStarter _newGameStarter;

    public SeedInputFieldNewGameStarterMediator(SeedInputFieldView seedInputView, 
        NewGameStarter newGameStarter)
    {
        _seedInputView = seedInputView;
        _newGameStarter = newGameStarter;
    }

    public override void Initialize()
    {
        _seedInputView.Text
            .Subscribe(value => _newGameStarter.SetSeedText(value))
            .AddTo(CompositeDisposable);
    }
}
