using System.Collections.Generic;
using UnityEngine;
using VContainer;

public abstract class MediatorViewFactory<TMediator, TView> : MediatorFactory<TMediator, TView>
    where TMediator : Mediator
    where TView : Component
{
    public abstract override TMediator Create(TView view);

    public virtual List<TMediator> CreateForEachView(IObjectResolver container)
    {
        Canvas canvas = container.Resolve<Canvas>();
        TView[] views = canvas.GetComponentsInChildren<TView>(true);
        List<TMediator> mediators = new();

        foreach (var view in views)
        {
            TMediator mediator = Create(view);
            mediators.Add(mediator);
        }

        return mediators;
    }
}

public abstract class MediatorViewFactory<TMediator, TDependency, TView> : MediatorFactory<TMediator, TDependency, TView>
    where TMediator : Mediator
    where TView : Component
{
    public abstract override TMediator Create(TDependency dependency, TView view);

    public virtual List<TMediator> CreateForEachView(IObjectResolver container)
    {
        Canvas canvas = container.Resolve<Canvas>();
        TView[] views = canvas.GetComponentsInChildren<TView>(true);
        List<TMediator> mediators = new();

        foreach (var view in views)
        {
            TDependency dependency = container.Resolve<TDependency>();
            TMediator mediator = Create(dependency, view);
            mediators.Add(mediator);
        }

        return mediators;
    }
}
