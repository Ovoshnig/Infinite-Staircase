using R3;
using System;
using UnityEngine;
using UnityEngine.Rendering;

public sealed class ThirdPersonLook : PersonLook
{
    private IDisposable _cameraSwitchDisposable;

    protected override Transform FollowPoint => PlayerTransform.Find(BindConstants.PlayerHeadCenterName);

    protected override void Awake()
    {
        base.Awake();

        _cameraSwitchDisposable = CameraSwitch.IsFirstPerson
            .Subscribe(value =>
            {
                enabled = !value;

                if (!value)
                    SkinnedMeshRenderer.shadowCastingMode = ShadowCastingMode.On;
            });
    }

    protected override void OnDestroy() => _cameraSwitchDisposable?.Dispose();
}
