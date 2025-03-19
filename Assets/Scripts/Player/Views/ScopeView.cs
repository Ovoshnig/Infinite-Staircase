using R3;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

[RequireComponent(typeof(Image))]
public class ScopeView : MonoBehaviour
{
    private readonly CompositeDisposable _compositeDisposable = new();
    private CameraSwitch _cameraSwitch;
    private Image _scopeImage;
    
    [Inject]
    public void Construct(CameraSwitch cameraSwitch) => _cameraSwitch = cameraSwitch;

    private void Awake() => _scopeImage = GetComponent<Image>();

    private void Start()
    {
        _cameraSwitch.IsScopeEnabled
            .Subscribe(value => _scopeImage.enabled = value)
            .AddTo(_compositeDisposable);
    }

    private void OnDestroy() => _compositeDisposable?.Dispose();
}
