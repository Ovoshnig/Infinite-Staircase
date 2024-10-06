using R3;
using UnityEngine;
using UnityEngine.Rendering;

public sealed class ThirdPersonLook : PersonLook
{
    protected override Transform FollowPoint => PlayerTransform.Find(ZenjectIdConstants.PlayerHeadCenterName);

    protected override void Awake()
    {
        base.Awake();

        CameraSwitch.IsFirstPerson
            .Subscribe(value =>
            {
                IsSelected = !value;

                if (!value)
                    SkinnedMeshRenderer.shadowCastingMode = ShadowCastingMode.On;
            })
            .AddTo(PermanentCompositeDisposable);
    }
}
