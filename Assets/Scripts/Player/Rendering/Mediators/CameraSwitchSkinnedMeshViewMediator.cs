using R3;

public class CameraSwitchSkinnedMeshViewMediator : Mediator
{
    private readonly CameraSwitch _cameraSwitch;
    private readonly SkinnedMeshRendererView _skinnedMeshRendererView;

    public CameraSwitchSkinnedMeshViewMediator(CameraSwitch cameraSwitch, 
        SkinnedMeshRendererView skinnedMeshRendererView)
    {
        _cameraSwitch = cameraSwitch;
        _skinnedMeshRendererView = skinnedMeshRendererView;
    }

    public override void Initialize()
    {
        _cameraSwitch.IsFirstPerson
            .Subscribe(_skinnedMeshRendererView.ChangeShadowCastingMode)
            .AddTo(CompositeDisposable);
    }
}
