using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MenuSceneLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder) =>
        builder.Register<SceneSwitchButtonViewMediatorFactory>(Lifetime.Singleton);

    private void Start()
    {
        Canvas canvas = Container.Resolve<Canvas>();
        SceneSwitchButtonViewMediatorFactory mediatorFactory = Container
            .Resolve<SceneSwitchButtonViewMediatorFactory>();
        mediatorFactory.CreateForEachView(canvas);
    }
}
