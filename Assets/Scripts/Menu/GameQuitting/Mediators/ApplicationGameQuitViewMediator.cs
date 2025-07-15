using R3;
using UnityEngine;

public class ApplicationGameQuitViewMediator : Mediator
{
    private readonly GameQuitButtonView _gameQuitButtonView;

    public ApplicationGameQuitViewMediator(GameQuitButtonView gameQuitButtonView) => 
        _gameQuitButtonView = gameQuitButtonView;

    public override void Initialize()
    {
        _gameQuitButtonView.Clicked
            .Subscribe(_ => Application.Quit())
            .AddTo(CompositeDisposable);
    }
}
