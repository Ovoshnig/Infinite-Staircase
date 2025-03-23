using R3;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public abstract class SliderTuner : MonoBehaviour
{
    private Slider _slider;
    private CompositeDisposable _compositeDisposable = new();

    protected Slider Slider => _slider;

    protected virtual void Awake() => _slider = GetComponent<Slider>();

    protected virtual void OnEnable()
    {
        _slider.OnValueChangedAsObservable()
            .Skip(1)
            .Subscribe(OnSliderValueChanged)
            .AddTo(_compositeDisposable);
    }

    protected virtual void Start()
    {
    }

    protected virtual void OnDisable()
    {
        _compositeDisposable?.Dispose();
        _compositeDisposable = new CompositeDisposable();
    }

    protected abstract void OnSliderValueChanged(float value);
}
