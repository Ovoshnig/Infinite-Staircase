using R3;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public abstract class LevelLoadButton : MonoBehaviour
{
    private SceneSwitch _sceneSwitch;
    private Button _button;

    [Inject]
    public void Construct(SceneSwitch sceneSwitch) => _sceneSwitch = sceneSwitch;

    protected SceneSwitch SceneSwitch => _sceneSwitch;

    private void Awake() => _button = GetComponent<Button>();

    private void Start()
    {
        _button.OnClickAsObservable()
            .Subscribe(_ => OnButtonClicked())
            .AddTo(this);
    }

    protected abstract void OnButtonClicked();
}
