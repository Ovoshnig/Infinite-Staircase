using UnityEngine;
using VContainer;

public abstract class MediatorViewFactory<TMediator, TView> : MediatorFactory<TMediator, TView>
    where TMediator : Mediator
    where TView : Component
{
    public abstract override TMediator Create(TView view);

    public virtual TMediator[] CreateForEachView(IObjectResolver container)
    {
        Canvas canvas = container.Resolve<Canvas>();
        TView[] views = canvas.GetComponentsInChildren<TView>(true);
        TMediator[] mediators = new TMediator[views.Length];

        for (int i = 0; i < views.Length; i++)
            mediators[i] = Create(views[i]);

        return mediators;
    }
}

public abstract class MediatorViewFactory<TMediator, TDependency, TView> : MediatorFactory<TMediator, TDependency, TView>
    where TMediator : Mediator
    where TView : Component
{
    public abstract override TMediator Create(TDependency dependency, TView view);

    public virtual TMediator[] CreateForEachView(IObjectResolver container)
    {
        Canvas canvas = container.Resolve<Canvas>();
        TView[] views = canvas.GetComponentsInChildren<TView>(true);
        TMediator[] mediators = new TMediator[views.Length];

        for (int i = 0; i < views.Length; i++)
        {
            TDependency dependency = container.Resolve<TDependency>();
            mediators[i] = Create(dependency, views[i]);
        }

        return mediators;
    }
}
