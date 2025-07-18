public class ApplicationGameQuitViewMediatorFactory
    : MediatorViewFactory<ApplicationGameQuitViewMediator, GameQuitButtonView>
{
    protected override ApplicationGameQuitViewMediator CreateMediatorInstance(GameQuitButtonView view) =>
        new(view);
}
