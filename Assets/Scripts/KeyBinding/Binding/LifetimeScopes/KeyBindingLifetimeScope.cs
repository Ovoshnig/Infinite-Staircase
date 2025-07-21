using UnityEngine;
using VContainer;
using VContainer.Unity;

public class KeyBindingLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<KeyListeningTracker>(Lifetime.Singleton);
        builder.Register<KeyBinderMediatorFactory>(Lifetime.Singleton);
        builder.Register<KeyListeningTrackerBlockerViewMediatorFactory>(Lifetime.Singleton);
    }

    private void Start()
    {
        Canvas canvas = Container.Resolve<Canvas>();
        KeyBinderMediatorFactory keyBinderMediatorFactory = Container
            .Resolve<KeyBinderMediatorFactory>();
        keyBinderMediatorFactory.CreateForEachView(canvas);

        BlockerView blockerView = canvas.GetComponentInChildren<BlockerView>(true);
        KeyListeningTrackerBlockerViewMediatorFactory keyListeningTrackerBlockerViewMediatorFactory = Container
            .Resolve<KeyListeningTrackerBlockerViewMediatorFactory>();
        keyListeningTrackerBlockerViewMediatorFactory.Create(blockerView);
    }
}
