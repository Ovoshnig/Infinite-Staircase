using Unity.Cinemachine;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public abstract class CameraLifetimeScope<TCameraView> : LifetimeScope where TCameraView : CameraPriorityView
{
    [SerializeField] private TCameraView _cameraPriorityViewPrefab;

    protected abstract Transform GetTrackingTarget(CharacterController characterController);

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInNewPrefab(_cameraPriorityViewPrefab, Lifetime.Singleton)
            .As<CameraPriorityView>().As<TCameraView>();

        builder.Register(resolver =>
        {
            TCameraView cameraPriorityView = resolver.Resolve<TCameraView>();
            return cameraPriorityView.GetComponent<InputAxisView>();
        }, Lifetime.Singleton);

        builder.Register<CameraSwitchPriorityViewMediatorFactory>(Lifetime.Singleton);
        builder.Register<PlayerInputHandlerAxisViewMediatorFactory>(Lifetime.Singleton);
    }

    private void Start()
    {
        TCameraView cameraPriorityView = Container.Resolve<TCameraView>();
        CharacterController characterController = Container.Resolve<CharacterController>();
        CinemachineCamera cinemachineCamera = cameraPriorityView.GetComponent<CinemachineCamera>();

        Transform trackingTarget = GetTrackingTarget(characterController);
        cinemachineCamera.Target.TrackingTarget = trackingTarget;

        CameraSwitchPriorityViewMediatorFactory cameraSwitchPriorityViewMediatorFactory =
            Container.Resolve<CameraSwitchPriorityViewMediatorFactory>();
        cameraSwitchPriorityViewMediatorFactory.Create(cameraPriorityView);

        InputAxisView inputAxisView = Container.Resolve<InputAxisView>();
        PlayerInputHandlerAxisViewMediatorFactory playerInputHandlerAxisViewMediatorFactory =
            Container.Resolve<PlayerInputHandlerAxisViewMediatorFactory>();
        playerInputHandlerAxisViewMediatorFactory.Create(inputAxisView);
    }
}
