using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MenuGameQuitLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder) => 
        builder.Register<ApplicationGameQuitViewMediatorFactory>(Lifetime.Singleton);

    private void Start()
    {
        Canvas canvas = Container.Resolve<Canvas>();
        ApplicationGameQuitViewMediatorFactory factory = Container
            .Resolve<ApplicationGameQuitViewMediatorFactory>();
        GameQuitButtonView[] views = canvas.GetComponentsInChildren<GameQuitButtonView>(true);

        foreach (var view in views)
            factory.Create(view);
    }
}
