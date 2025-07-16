using R3;
using System;
using VContainer.Unity;

public abstract class Mediator : IInitializable, IDisposable
{
    private readonly CompositeDisposable _compositeDisposable = new();

    protected CompositeDisposable CompositeDisposable => _compositeDisposable;

    public abstract void Initialize();

    public virtual void Dispose() => CompositeDisposable.Dispose();
}
