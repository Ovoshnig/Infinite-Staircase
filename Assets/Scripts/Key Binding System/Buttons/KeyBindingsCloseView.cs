using R3;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

[RequireComponent(typeof(Button), typeof(PanelCloseButton))]
public class KeyBindingsCloseView : MonoBehaviour
{
    private KeyListeningTracker _listeningTracker;
    private Button _button;
    private PanelCloseButton _panelClose;

    [Inject]
    public void Construct(KeyListeningTracker listeningTracker) =>
        _listeningTracker = listeningTracker;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _panelClose = GetComponent<PanelCloseButton>();
    }

    private void Start()
    {
        _listeningTracker.IsListening
            .Select(value => value
            ? Observable.Return(value)
            : Observable.Return(value).DelayFrame(1)
            )
            .Switch()
            .Subscribe(value =>
            {
                _button.interactable = !value;
                _panelClose.enabled = !value;
            })
            .AddTo(this);
    }
}
