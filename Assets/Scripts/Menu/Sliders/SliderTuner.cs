using R3;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public abstract class SliderTuner : MonoBehaviour
{
    private Slider _slider;

    protected abstract float MinValue { get; }
    protected abstract float MaxValue { get; }

    protected virtual void Awake() => _slider = GetComponent<Slider>();

    protected virtual void OnEnable() => _slider.onValueChanged.AddListener(OnSliderValueChanged);

    protected virtual void Start()
    {
        _slider.minValue = MinValue;
        _slider.maxValue = MaxValue;
    }

    protected virtual void OnDisable() => _slider.onValueChanged.RemoveListener(OnSliderValueChanged);

    protected void SetSliderValue(float value) => _slider.SetValueWithoutNotify(value);

    protected abstract void OnSliderValueChanged(float value);
}
