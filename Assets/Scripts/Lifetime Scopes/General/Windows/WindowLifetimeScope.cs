using UnityEngine;
using VContainer;
using VContainer.Unity;

public abstract class WindowLifetimeScope : LifetimeScope
{
    [SerializeField] private WindowView _windowView;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<WindowSwitchWindowViewMediator>(Lifetime.Singleton);

        builder.Register(resolver =>
        {
            WindowView windowView = resolver.Resolve<WindowView>();
            return windowView.GetComponentInChildren<ResumeButtonView>(includeInactive: true);
        }, Lifetime.Singleton);

        builder.RegisterEntryPoint<ResumeButtonViewWindowSwitchMediator>(Lifetime.Singleton);

        builder.RegisterComponentInNewPrefab(_windowView, Lifetime.Singleton);
    }

    protected virtual void Start()
    {
        GameObject window = Container.Resolve<WindowView>().gameObject;
        Container.InjectGameObject(window);
    }
}
