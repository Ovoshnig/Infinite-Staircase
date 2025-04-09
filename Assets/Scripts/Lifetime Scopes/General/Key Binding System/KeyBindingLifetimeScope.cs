using UnityEngine;
using VContainer;
using VContainer.Unity;

public class KeyBindingLifetimeScope : LifetimeScope
{
    [SerializeField] private KeyBindingsCloseView _bindingsCloseView;

    protected override void Configure(IContainerBuilder builder) =>
        builder.Register<KeyListeningTracker>(Lifetime.Singleton);

    private void Start()
    {
        if (_bindingsCloseView != null)
            Container.Inject(_bindingsCloseView);
    }
}
