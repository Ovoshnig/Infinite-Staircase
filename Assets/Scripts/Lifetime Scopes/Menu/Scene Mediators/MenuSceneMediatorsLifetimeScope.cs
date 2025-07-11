using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MenuSceneMediatorsLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder) => 
        builder.Register<SceneSwitchButtonViewMediatorFactory>(Lifetime.Singleton);

    private void Start()
    {
        SceneSwitchButtonViewMediatorFactory factory = Container
            .Resolve<SceneSwitchButtonViewMediatorFactory>();
        Canvas canvas = Container.Resolve<Canvas>();
        SceneButtonView[] views = canvas.GetComponentsInChildren<SceneButtonView>(true);

        foreach (var view in views)
            factory.Create(view);
    }
}
