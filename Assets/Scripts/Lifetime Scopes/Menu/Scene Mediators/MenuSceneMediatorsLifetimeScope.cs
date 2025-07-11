using VContainer;
using VContainer.Unity;

public class MenuSceneMediatorsLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder) => 
        builder.Register<SceneSwitchButtonViewMediatorFactory>(Lifetime.Singleton);

    private void Start()
    {
        SceneSwitchButtonViewMediatorFactory mediatorFactory = Container
            .Resolve<SceneSwitchButtonViewMediatorFactory>();
        mediatorFactory.CreateForEachView(Container);
    }
}
