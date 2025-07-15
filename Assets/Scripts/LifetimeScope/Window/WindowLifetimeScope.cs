using UnityEngine;
using VContainer;
using VContainer.Unity;

public abstract class WindowLifetimeScope : LifetimeScope
{
    [SerializeField] private Canvas _windowCanvas;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInNewPrefab(_windowCanvas, Lifetime.Singleton);

        builder.Register(resolver =>
        {
            Canvas windowCanvas = resolver.Resolve<Canvas>();
            return windowCanvas.GetComponentInChildren<WindowView>(includeInactive: true);
        }, Lifetime.Singleton);

        builder.RegisterEntryPoint<WindowMediator>(Lifetime.Singleton);

        builder.Register(resolver =>
        {
            Canvas windowCanvas = resolver.Resolve<Canvas>();
            return windowCanvas.GetComponentInChildren<ResumeButtonView>(includeInactive: true);
        }, Lifetime.Singleton);

        builder.RegisterEntryPoint<WindowResumeButtonViewMediator>(Lifetime.Singleton);

        builder.Register(resolver =>
        {
            Canvas windowCanvas = resolver.Resolve<Canvas>();
            return windowCanvas.GetComponentsInChildren<PanelCloseButtonView>(includeInactive: true);
        }, Lifetime.Singleton);

        builder.RegisterEntryPoint<WindowInputHandlerPanelCloseButtonViewMediator>(Lifetime.Singleton);
    }

    protected virtual void Start()
    {
        GameObject window = Container.Resolve<Canvas>().gameObject;
        Container.InjectGameObject(window);
    }
}
