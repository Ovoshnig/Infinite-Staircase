using R3;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

public abstract class InputHandler<TActions> : IInitializable, IDisposable
{
    private readonly CompositeDisposable _compositeDisposable = new();

    protected InputHandler(TActions actions) => Actions = actions;

    protected TActions Actions { get; }

    public virtual void Initialize() => EnableActions();

    public virtual void Dispose()
    {
        _compositeDisposable.Dispose();
        DisableActions();
    }

    protected abstract void EnableActions();

    protected abstract void DisableActions();

    protected ReadOnlyReactiveProperty<Vector2> BindValue(Func<TActions, InputAction> selector)
        => selector(Actions).AsValueStream<Vector2>().AddTo(_compositeDisposable);

    protected ReadOnlyReactiveProperty<bool> BindButton(Func<TActions, InputAction> selector)
        => selector(Actions).AsButtonStream().AddTo(_compositeDisposable);
}
