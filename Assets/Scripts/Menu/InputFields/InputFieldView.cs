using R3;
using TMPro;
using UnityEngine;

public class InputFieldView : MonoBehaviour
{
    private readonly ReactiveProperty<string> _text = new(string.Empty);
    private TMP_InputField _inputField;

    public ReadOnlyReactiveProperty<string> Text => _text;

    private void Awake() => _inputField = GetComponent<TMP_InputField>();

    private void Start()
    {
        _inputField.onValueChanged
            .AsObservable()
            .Subscribe(value => _text.Value = value)
            .AddTo(this);
    }
}
