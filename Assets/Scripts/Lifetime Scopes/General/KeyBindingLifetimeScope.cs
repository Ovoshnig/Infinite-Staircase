using UnityEngine;
using VContainer;
using VContainer.Unity;

public class KeyBindingLifetimeScope : LifetimeScope
{
    [SerializeField] private KeyBindingsCloseView _bindingsCloseView;

    protected override void Configure(IContainerBuilder builder) =>
        builder.Register<KeyListeningTracker>(Lifetime.Singleton);

    private void Start() => Container.Inject(_bindingsCloseView);
}
