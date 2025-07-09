using UnityEngine;
using VContainer;
using VContainer.Unity;

public class KeyBindingLifetimeScope : LifetimeScope
{
    [SerializeField] private GameObject _bindingsRoot;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<KeyListeningTracker>(Lifetime.Singleton);
        builder.Register<KeyBinderMediatorFactory>(Lifetime.Singleton);
    }

    private void Start()
    {
        KeyBinderView[] views = _bindingsRoot.GetComponentsInChildren<KeyBinderView>();
        KeyBinderMediatorFactory factory = Container.Resolve<KeyBinderMediatorFactory>();

        foreach (var view in views)
            factory.Create(view);
    }
}
