using R3;
using System;

public abstract class MediatorFactory<TMediator, TDependency> : IDisposable
    where TMediator : Mediator
{
    protected CompositeDisposable CompositeDisposable { get; } = new();

    public void Dispose() => CompositeDisposable.Dispose();

    public TMediator Create(TDependency dependency)
    {
        TMediator mediator = CreateMediatorInstance(dependency);

        mediator.Initialize();
        mediator.AddTo(CompositeDisposable);
        return mediator;
    }

    protected abstract TMediator CreateMediatorInstance(TDependency dependency);
}

public abstract class MediatorFactory<TMediator, TDependency1, TDependency2> : IDisposable
    where TMediator : Mediator
{
    protected CompositeDisposable CompositeDisposable { get; } = new();

    public void Dispose() => CompositeDisposable.Dispose();

    public TMediator Create(TDependency1 dependency1, TDependency2 dependency2)
    {
        TMediator mediator = CreateMediatorInstance(dependency1, dependency2);

        mediator.Initialize();
        mediator.AddTo(CompositeDisposable);
        return mediator;
    }

    protected abstract TMediator CreateMediatorInstance(TDependency1 dependency1, TDependency2 dependency2);
}
