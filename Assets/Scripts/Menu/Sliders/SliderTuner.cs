using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public abstract class SliderTuner : MonoBehaviour
{
    private Slider _slider;

    protected abstract float MinValue { get; }
    protected abstract float MaxValue { get; }
    protected abstract float InitialValue { get; }

    protected virtual void Awake() => _slider = GetComponent<Slider>();

    protected virtual void OnEnable() => _slider.onValueChanged.AddListener(OnSliderValueChanged);

    protected virtual void Start() => InitializeSlider();

    protected virtual void OnDisable() => _slider.onValueChanged.RemoveListener(OnSliderValueChanged);

    protected abstract void OnSliderValueChanged(float value);

    private void InitializeSlider()
    {
        _slider.minValue = MinValue;
        _slider.maxValue = MaxValue;
        _slider.value = InitialValue;
    }
}
