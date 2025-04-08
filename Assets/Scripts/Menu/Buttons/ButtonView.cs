using R3;
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class ButtonView : MonoBehaviour
{
    private Button _button;

    public event Action ButtonClicked;

    protected virtual void Awake() => _button = GetComponent<Button>();

    protected virtual void Start()
    {
        _button.OnClickAsObservable()
            .Subscribe(_ => ButtonClicked?.Invoke())
            .AddTo(this);
    }

    protected virtual void OnDestroy() { }
}
