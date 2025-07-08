using R3;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

[RequireComponent(typeof(Image))]
public class PlayerScopeView : MonoBehaviour
{
    private CameraSwitch _cameraSwitch;
    private Image _scopeImage;

    [Inject]
    public void Construct(CameraSwitch cameraSwitch) => _cameraSwitch = cameraSwitch;

    private void Awake() => _scopeImage = GetComponent<Image>();

    private void Start()
    {
        _cameraSwitch.IsFirstPerson
            .Subscribe(value => _scopeImage.enabled = value)
            .AddTo(this);
    }

    public void Enable() => _scopeImage.enabled = true;

    public void Disable() => _scopeImage.enabled = false;
}
