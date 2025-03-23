using R3;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

[RequireComponent(typeof(Image))]
public class ScopeView : MonoBehaviour
{
    private readonly CompositeDisposable _compositeDisposable = new();
    private CameraSwitch _cameraSwitch;
    private WindowTracker _windowTracker;
    private Image _scopeImage;

    [Inject]
    public void Construct(CameraSwitch cameraSwitch, WindowTracker windowTracker)
    {
        _cameraSwitch = cameraSwitch;
        _windowTracker = windowTracker;
    }

    private void Awake() => _scopeImage = GetComponent<Image>();

    private void Start()
    {
        _cameraSwitch.IsFirstPerson
            .Subscribe(value => _scopeImage.enabled = value)
            .AddTo(_compositeDisposable);

        _windowTracker.IsOpen
            .Where(_ => _cameraSwitch.IsFirstPerson.CurrentValue)
            .Subscribe(value => _scopeImage.enabled = !value)
            .AddTo(_compositeDisposable);
    }

    private void OnDestroy() => _compositeDisposable?.Dispose();
}
