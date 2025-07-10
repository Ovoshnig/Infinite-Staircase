using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class MediatorFactory<TMediator, TView> : IDisposable
    where TMediator : Mediator 
    where TView : Component
{
    public List<IDisposable> Disposables { get; private set; } = new();

    public void Dispose()
    {
        foreach (var disposable in Disposables)
            disposable.Dispose();
    }

    public abstract TMediator Create(TView view);
}
