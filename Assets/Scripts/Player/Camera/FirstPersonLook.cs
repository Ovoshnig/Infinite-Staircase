using R3;
using System;
using UnityEngine;
using UnityEngine.Rendering;

public sealed class FirstPersonLook : PersonLook
{
    private IDisposable _cameraSwitchDisposable;

    protected override void Awake()
    {
        _cameraSwitchDisposable = CameraSwitch.IsFirstPerson
            .Subscribe(value =>
            {
                enabled = value;

                if (value)
                    SkinnedMeshRenderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
            });
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        var lookDisposable = PlayerState.IsLooking
            .Where(value => value)
            .Subscribe(_ =>
            {
                RotationX -= PlayerState.LookInput.y * RotationSpeed;
                RotationX = Mathf.Clamp(RotationX, -XRotationLimitDown, XRotationLimitUp);
                transform.localRotation = Quaternion.Euler(RotationX, 0f, 0f);

                RotationY += PlayerState.LookInput.x * RotationSpeed;
                PlayerTransform.localRotation = Quaternion.Euler(0f, RotationY, 0f);
            });

        CompositeDisposable.Add(lookDisposable);
    }

    protected override void OnDestroy() => _cameraSwitchDisposable?.Dispose();
}
