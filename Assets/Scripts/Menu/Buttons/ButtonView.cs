using R3;
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class ButtonView : MonoBehaviour
{
    private Button _button = null;

    public event Action ButtonClicked;

    private Button Button
    {
        get
        {
            if (_button == null)
                _button = GetComponent<Button>();

            return _button;
        }
    }

    protected virtual void Awake() { }

    protected virtual void Start()
    {
        Button.OnClickAsObservable()
            .Subscribe(_ => ButtonClicked?.Invoke())
            .AddTo(this);
    }

    protected virtual void OnDestroy() { }
}
