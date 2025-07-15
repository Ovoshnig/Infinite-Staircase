using VContainer;
using VContainer.Unity;

public class MenuGameQuitLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder) => 
        builder.Register<ApplicationGameQuitViewMediatorFactory>(Lifetime.Singleton);

    private void Start()
    {
        ApplicationGameQuitViewMediatorFactory mediatorFactory = Container
            .Resolve<ApplicationGameQuitViewMediatorFactory>();
        mediatorFactory.CreateForEachView(Container);
    }
}
