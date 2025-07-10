using UnityEngine;

public abstract class MediatorViewFactory<TMediator, TView> : MediatorFactory<TMediator, TView>
    where TMediator : Mediator
    where TView : Component
{
}
