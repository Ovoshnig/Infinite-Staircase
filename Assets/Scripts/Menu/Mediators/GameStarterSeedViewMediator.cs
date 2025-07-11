using R3;

public class GameStarterSeedViewMediator : Mediator
{
    private readonly GameStarter _gameStarter;
    private readonly SeedInputFieldView _seedInputView;

    public GameStarterSeedViewMediator(GameStarter gameStarter,
        SeedInputFieldView seedInputView)
    {
        _gameStarter = gameStarter;
        _seedInputView = seedInputView;
    }

    public override void Initialize()
    {
        _seedInputView.Text
            .Subscribe(value => _gameStarter.SetSeedText(value))
            .AddTo(CompositeDisposable);
    }
}
