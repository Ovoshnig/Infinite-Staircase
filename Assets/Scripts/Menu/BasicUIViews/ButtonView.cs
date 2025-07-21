using R3;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class ButtonView : MonoBehaviour
{
    private readonly Subject<Unit> _clicked = new();

    private Button _button = null;

    public Observable<Unit> Clicked => _clicked;

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

    public void SetEnable(bool value) => enabled = value;

    public void SetInteractable(bool value) => Button.interactable = value;
}
