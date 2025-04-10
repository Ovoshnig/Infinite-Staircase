using R3;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public abstract class SliderView : MonoBehaviour
{
    private readonly ReactiveProperty<float> _value = new(default);
    private Slider _slider = null;

    public ReadOnlyReactiveProperty<float> Value => _value;

    private Slider Slider
    {
        get
        {
            if (_slider == null)
                _slider = GetComponent<Slider>();

            return _slider;
        }
    }

    protected virtual void Start()
    {
        Slider.OnValueChangedAsObservable()
            .Subscribe(value => _value.Value = value)
            .AddTo(this);
    }

    public void SetValue(float value) => Slider.SetValueWithoutNotify(value);

    public void SetMinValue(float value) => Slider.minValue = value;

    public void SetMaxValue(float value) => Slider.maxValue = value;
}
