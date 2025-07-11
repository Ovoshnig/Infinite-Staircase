using UnityEngine;

public abstract class MediatorViewFactory<TMediator, TView> : MediatorFactory<TMediator, TView>
    where TMediator : Mediator
    where TView : Component
{
    public abstract override TMediator Create(TView view);
}

public abstract class MediatorViewFactory<TMediator, TDependency, TView> : MediatorFactory<TMediator, TDependency, TView>
    where TMediator : Mediator
    where TView : Component
{
    public abstract override TMediator Create(TDependency dependency, TView view);
}
