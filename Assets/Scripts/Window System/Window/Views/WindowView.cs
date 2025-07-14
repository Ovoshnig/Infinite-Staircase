using R3;
using UnityEngine;

public class WindowView : MonoBehaviour
{
    private readonly ReactiveProperty<bool> _isActive = new();

    public ReadOnlyReactiveProperty<bool> IsActive => _isActive;

    private void Awake()
    {
        Observable
            .EveryValueChanged(gameObject, g => g.activeSelf)
            .Subscribe(activeSelf => _isActive.Value = activeSelf)
            .AddTo(this);
    }

    public void SetActive(bool value) => gameObject.SetActive(value);
}
