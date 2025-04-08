using R3;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public abstract class SliderView : MonoBehaviour
{
    private readonly ReactiveProperty<float> _value = new(default);
    private Slider _slider;

    public ReadOnlyReactiveProperty<float> Value => _value;

    protected Slider Slider => _slider;

    public void Awake() => _slider = GetComponent<Slider>();

    protected virtual void Start()
    {
        _slider.OnValueChangedAsObservable()
            .Subscribe(value => _value.Value = value)
            .AddTo(this);
    }

    public void SetValue(float value) => _slider.SetValueWithoutNotify(value);

    public void SetMinValue(float value) => _slider.minValue = value;

    public void SetMaxValue(float value) => _slider.maxValue = value;
}
