using R3;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(CinemachineInputAxisController))]
public sealed class ThirdPersonLook : PersonLook
{
    private CinemachineInputAxisController _inputAxisController;
    private DefaultInputAxisDriver _driver;

    protected override Transform FollowPoint => PlayerTransform.Find(ZenjectIdConstants.PlayerHeadCenterName);

    protected override void Awake()
    {
        base.Awake();

        _inputAxisController = GetComponent<CinemachineInputAxisController>();

        var cameraSwitchDisposable = CameraSwitch.IsFirstPerson
            .Subscribe(value =>
            {
                IsSelected = !value;

                if (!value)
                    SkinnedMeshRenderer.shadowCastingMode = ShadowCastingMode.On;
            });

        PermanentCompositeDisposable.Add(cameraSwitchDisposable);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        var sensitivityDisposable = LookTuner.Sensitivity
            .Subscribe(value =>
            {
                CinemachineInputAxisController.Reader reader = new()
                {
                    Gain = 10
                };
            });

        EnablingCompositeDisposable.Add(sensitivityDisposable);
    }
}
