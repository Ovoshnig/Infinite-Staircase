using R3;
using UnityEngine;
using UnityEngine.Rendering;
using VContainer;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class SkinnedMeshRendererView : MonoBehaviour
{
    private readonly CompositeDisposable _compositeDisposable = new();
    private CameraSwitch _cameraSwitch;
    private SkinnedMeshRenderer _skinnedMeshRenderer;

    [Inject]
    public void Construct(CameraSwitch cameraSwitch) => _cameraSwitch = cameraSwitch;

    private void Awake() => _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

    private void Start()
    {
        _cameraSwitch.IsFirstPerson
            .Subscribe(value => 
            {
                _skinnedMeshRenderer.shadowCastingMode = value 
                ? ShadowCastingMode.ShadowsOnly 
                : ShadowCastingMode.On;
            })
            .AddTo(_compositeDisposable);
    }

    private void OnDestroy() => _compositeDisposable?.Dispose();
}
