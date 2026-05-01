using R3;
using UnityEngine;

public class WindowView : MonoBehaviour
{
    public ReadOnlyReactiveProperty<bool> IsActive { get; private set; }

    private void Awake()
    {
        IsActive = Observable
            .EveryValueChanged(gameObject, g => g.activeSelf, destroyCancellationToken)
            .ToReadOnlyReactiveProperty(false);
    }

    public void SetActive(bool value) => gameObject.SetActive(value);
}
