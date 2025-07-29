using R3;
using UnityEngine.InputSystem;

public static class InputActionExtensions
{
    public static ReadOnlyReactiveProperty<bool> AsButtonStream(this InputAction action)
    {
        return Observable.Create<bool>(observer =>
        {
            void Handle(InputAction.CallbackContext context)
            {
                bool value = context.ReadValueAsButton();
                observer.OnNext(value);
            }

            action.performed += Handle;
            action.canceled += Handle;

            return Disposable.Create(() =>
            {
                action.performed -= Handle;
                action.canceled -= Handle;
            });
        }).ToReadOnlyReactiveProperty();
    }

    public static ReadOnlyReactiveProperty<T> AsValueStream<T>(this InputAction action) where T : struct
    {
        return Observable.Create<T>(observer =>
        {
            void Handle(InputAction.CallbackContext context)
            {
                T value = context.ReadValue<T>();
                observer.OnNext(value);
            }

            action.performed += Handle;
            action.canceled += Handle;

            return Disposable.Create(() =>
            {
                action.performed -= Handle;
                action.canceled -= Handle;
            });
        }).ToReadOnlyReactiveProperty();
    }
}
