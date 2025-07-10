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
        MainMenuCanvasView canvasView = FindFirstObjectByType<MainMenuCanvasView>();
        KeyBinderView[] views = canvasView.GetComponentsInChildren<KeyBinderView>(true);

        KeyBinderMediatorFactory mediatorFactory = Container.Resolve<KeyBinderMediatorFactory>();

        foreach (var view in views)
            mediatorFactory.Create(view);
    }
}
