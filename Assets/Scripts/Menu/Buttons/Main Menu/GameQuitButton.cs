using R3;
using UnityEngine;
using UnityEngine.UI;

public class GameQuitButton : MonoBehaviour
{
    private Button _button;

    private void Awake() => _button = GetComponent<Button>();

    private void Start()
    {
        _button.OnClickAsObservable()
            .Subscribe(_ => Application.Quit())
            .AddTo(this);
    }
}
