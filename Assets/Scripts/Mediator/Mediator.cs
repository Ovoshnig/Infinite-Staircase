using R3;
using System;
using VContainer.Unity;

public abstract class Mediator : IInitializable, IDisposable
{
    protected CompositeDisposable CompositeDisposable { get; } = new();

    public abstract void Initialize();

    public virtual void Dispose() => CompositeDisposable.Dispose();
}
