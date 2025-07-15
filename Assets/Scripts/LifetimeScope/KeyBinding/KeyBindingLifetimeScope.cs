using VContainer;
using VContainer.Unity;

public class KeyBindingLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder) => 
        builder.Register<KeyBinderMediatorFactory>(Lifetime.Singleton);

    private void Start()
    {
        KeyBinderMediatorFactory mediatorFactory = Container.Resolve<KeyBinderMediatorFactory>();
        mediatorFactory.CreateForEachView(Container);
    }
}
