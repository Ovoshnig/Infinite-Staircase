using R3;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ResumeButtonView : MonoBehaviour
{
    private readonly ReactiveProperty<Unit> _buttonClicked = new(default);
    private Button _button;

    public ReadOnlyReactiveProperty<Unit> ButtonClicked => _buttonClicked;

    private void Awake() => _button = GetComponent<Button>();

    private void Start()
    {
        _button.OnClickAsObservable()
            .Subscribe(_buttonClicked.OnNext)
            .AddTo(this);
    }
}
