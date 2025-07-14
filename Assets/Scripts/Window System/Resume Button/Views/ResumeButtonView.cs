using R3;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ResumeButtonView : MonoBehaviour
{
    private readonly Subject<Unit> _clicked = new();

    private Button _button = null;

    private Button Button
    {
        get
        {
            if (_button == null)
                _button = GetComponent<Button>();

            return _button;
        }
    }

    public Observable<Unit> Clicked => _clicked;

    private void Start()
    {
        Button.OnClickAsObservable()
            .Subscribe(_clicked.OnNext)
            .AddTo(this);
    }
}
