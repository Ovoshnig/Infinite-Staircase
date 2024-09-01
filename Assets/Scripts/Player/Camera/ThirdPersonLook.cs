using R3;
using System;
using UnityEngine;
using UnityEngine.Rendering;

public sealed class ThirdPersonLook : PersonLook
{
    [SerializeField] private float _slewSpeed = 1f;

    private Transform _playerHead;
    private IDisposable _cameraSwitchDisposable;

    protected override void Awake()
    {
        _playerHead = PlayerTransform.Find(BindConstants.PlayerHeadCenterName);
        Camera.Follow = _playerHead;

        _cameraSwitchDisposable = CameraSwitch.IsFirstPerson
            .Subscribe(value =>
            {
                enabled = !value;

                if (!value)
                    SkinnedMeshRenderer.shadowCastingMode = ShadowCastingMode.On;
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
                RotationY += PlayerState.LookInput.x * RotationSpeed;
                transform.localRotation = Quaternion.Euler(RotationX, RotationY, 0f);
            });

        CompositeDisposable.Add(lookDisposable);
    }

    protected override void OnDestroy() => _cameraSwitchDisposable?.Dispose();

    protected override void Update()
    {
        transform.position = _playerHead.position;

        if (PlayerState.IsWalking.CurrentValue)
        {
            Vector3 inputDirection = new(PlayerState.WalkInput.x, 0, PlayerState.WalkInput.y);
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
            targetAngle += transform.eulerAngles.y;
            float smoothedAngle = Mathf.LerpAngle(PlayerTransform.eulerAngles.y, targetAngle, Time.deltaTime * _slewSpeed);
            PlayerTransform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);
        }
    }
}
