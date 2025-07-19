using UnityEngine;

public abstract class MediatorViewFactory<TMediator, TView> : MediatorFactory<TMediator, TView>
    where TMediator : Mediator
    where TView : Component
{
    public virtual TMediator[] CreateForEachView(Component parentComponent)
    {
        TView[] views = parentComponent.GetComponentsInChildren<TView>(true);
        TMediator[] mediators = new TMediator[views.Length];

        for (int i = 0; i < views.Length; i++)
            mediators[i] = Create(views[i]);

        return mediators;
    }

    protected abstract override TMediator CreateMediatorInstance(TView view);
}

public abstract class MediatorViewFactory<TMediator, TDependency, TView>
    : MediatorFactory<TMediator, TDependency, TView>
    where TMediator : Mediator
    where TView : Component
{
    public virtual TMediator[] CreateForEachView(TDependency dependency, Component parentComponent)
    {
        TView[] views = parentComponent.GetComponentsInChildren<TView>(true);
        TMediator[] mediators = new TMediator[views.Length];

        for (int i = 0; i < views.Length; i++)
            mediators[i] = Create(dependency, views[i]);

        return mediators;
    }

    protected abstract override TMediator CreateMediatorInstance(TDependency dependency, TView view);
}
