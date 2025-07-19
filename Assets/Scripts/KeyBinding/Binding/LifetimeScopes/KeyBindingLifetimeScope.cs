using UnityEngine;
using VContainer;
using VContainer.Unity;

public class KeyBindingLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<KeyListeningTracker>(Lifetime.Singleton);
        builder.Register<KeyBinderMediatorFactory>(Lifetime.Singleton);
    }

    private void Start()
    {
        KeyBinderMediatorFactory mediatorFactory = Container.Resolve<KeyBinderMediatorFactory>();
        Canvas canvas = Container.Resolve<Canvas>();
        mediatorFactory.CreateForEachView(canvas);
    }
}
