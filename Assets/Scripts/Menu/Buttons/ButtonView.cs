using R3;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class ButtonView : MonoBehaviour
{
    private readonly ReactiveProperty<Unit> _clicked = new(default);
    private Button _button = null;

    public ReadOnlyReactiveProperty<Unit> Clicked => _clicked;

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
            .Subscribe(_clicked.OnNext)
            .AddTo(this);
    }

    protected virtual void OnDestroy() { }
}
