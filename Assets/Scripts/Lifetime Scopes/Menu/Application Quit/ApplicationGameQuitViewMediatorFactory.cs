public class ApplicationGameQuitViewMediatorFactory
    : MediatorViewFactory<ApplicationGameQuitViewMediator, GameQuitButtonView>
{
    public override ApplicationGameQuitViewMediator Create(GameQuitButtonView view)
    {
        ApplicationGameQuitViewMediator mediator = new(view);
        mediator.Initialize();
        Disposables.Add(mediator);
        return mediator;
    }
}
