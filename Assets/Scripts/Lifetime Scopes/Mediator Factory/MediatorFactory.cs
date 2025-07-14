using System;
using System.Collections.Generic;

public abstract class MediatorFactory<TMediator, TDependency> : IDisposable
    where TMediator : Mediator
{
    public List<IDisposable> Disposables { get; private set; } = new();

    public void Dispose()
    {
        foreach (var disposable in Disposables)
            disposable.Dispose();
    }

    public abstract TMediator Create(TDependency dependency);
}

public abstract class MediatorFactory<TMediator, TDependency1, TDependency2> : IDisposable
    where TMediator : Mediator
{
    public List<IDisposable> Disposables { get; private set; } = new();

    public void Dispose()
    {
        foreach (var disposable in Disposables)
            disposable.Dispose();
    }

    public abstract TMediator Create(TDependency1 dependency1, TDependency2 dependency2);
}
