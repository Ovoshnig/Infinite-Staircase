using R3;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public abstract class ToggleView : MonoBehaviour
{
    private readonly ReactiveProperty<bool> _isOn = new();

    private Toggle _toggle = null;

    public ReadOnlyReactiveProperty<bool> IsOn => _isOn;

    private Toggle Toggle
    {
        get
        {
            if (_toggle == null)
                _toggle = GetComponent<Toggle>();

            return _toggle;
        }
    }

    private void Start()
    {
        Toggle.OnValueChangedAsObservable()
            .Subscribe(value => _isOn.Value = value)
            .AddTo(this);
    }

    public void SetIsOn(bool value)
    {
        Toggle.isOn = value;
        _isOn.Value = value;
    }

    public void SetIsOnWithoutNotify(bool value)
    {
        Toggle.SetIsOnWithoutNotify(value);
        _isOn.Value = value;
    }
}
