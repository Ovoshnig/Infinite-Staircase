using R3;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public abstract class SliderView : MonoBehaviour
{
    [SerializeField] private Image _fillingImage;
    [SerializeField] private Sprite[] _fillingSprites;

    private readonly ReactiveProperty<float> _value = new();

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

    public void SetValue(float value)
    {
        Slider.value = value;
        _value.Value = value;

        SetFillingSprite(value);
    }

    public void SetValueWithoutNotify(float value)
    {
        Slider.SetValueWithoutNotify(value);
        _value.Value = value;

        SetFillingSprite(value);
    }

    public void SetMinValue(float value) => Slider.minValue = value;

    public void SetMaxValue(float value) => Slider.maxValue = value;

    private void SetFillingSprite(float value)
    {
        if (_fillingImage != null && _fillingSprites.Length > 1)
        {
            float percentagePerImage = 1f / (_fillingSprites.Length - 1);
            float fillingPercentage = (value - _slider.minValue) / (_slider.maxValue - _slider.minValue);
            int spriteIndex = Mathf.CeilToInt(fillingPercentage / percentagePerImage);
            _fillingImage.sprite = _fillingSprites[spriteIndex];
        }
    }
}
