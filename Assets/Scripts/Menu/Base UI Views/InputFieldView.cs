using R3;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_InputField))]
public abstract class InputFieldView : MonoBehaviour
{
    private readonly ReactiveProperty<string> _text = new(string.Empty);

    private TMP_InputField _inputField = null;

    public ReadOnlyReactiveProperty<string> Text => _text;

    private TMP_InputField InputField
    {
        get
        {
            if (_inputField == null)
                _inputField = GetComponent<TMP_InputField>();

            return _inputField;
        }
    }

    private void Start()
    {
        InputField.onValueChanged
            .AsObservable()
            .Subscribe(value => _text.Value = value)
            .AddTo(this);
    }
}
