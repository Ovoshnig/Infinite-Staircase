using R3;
using UnityEngine;
using UnityEngine.Rendering;

public sealed class FirstPersonLook : PersonLook
{
    protected override Transform FollowPoint => PlayerTransform.Find(ZenjectIdConstants.PlayerEyeCenterName);

    protected override void Awake()
    {
        base.Awake();

        CameraSwitch.IsFirstPerson
            .Subscribe(value =>
            {
                IsSelected = value;

                if (value)
                    SkinnedMeshRenderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
            })
            .AddTo(PermanentCompositeDisposable);
    }
}
