using R3;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public abstract class SliderTuner : MonoBehaviour
{
    private Slider _slider;

    protected Slider Slider => _slider;

    protected virtual void Awake() => _slider = GetComponent<Slider>();

    protected virtual void Start()
    {
        _slider.OnValueChangedAsObservable()
            .Subscribe(OnSliderValueChanged)
            .AddTo(this);
    }

    protected abstract void OnSliderValueChanged(float value);
}
