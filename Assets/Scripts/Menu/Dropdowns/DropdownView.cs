using R3;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Dropdown))]
public abstract class DropdownView : MonoBehaviour
{
    private readonly ReactiveProperty<int> _value = new(0);
    private TMP_Dropdown _dropdown = null;

    public ReadOnlyReactiveProperty<int> Value => _value;

    private TMP_Dropdown Dropdown
    {
        get
        {
            if (_dropdown == null)
                _dropdown = GetComponent<TMP_Dropdown>();

            return _dropdown;
        }
    }

    private void Start()
    {
        Dropdown.onValueChanged.AsObservable()
            .Subscribe(value => _value.Value = value)
            .AddTo(this);
    }

    public void SetOptions(List<TMP_Dropdown.OptionData> options) => 
        Dropdown.options = options;

    public void SetValue(int value) => Dropdown.value = value;
}
