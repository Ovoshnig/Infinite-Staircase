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
