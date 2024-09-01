using R3;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CharacterController))]
public abstract class PlayerHorizontalMover : MonoBehaviour
{
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;

    private PlayerState _playerState;
    private CameraSwitch _cameraSwitch;
    private CharacterController _characterController;

    [Inject]
    protected void Construct(PlayerState playerState,
        [Inject(Id = BindConstants.PlayerId)] CameraSwitch cameraSwitch)
    {
        _playerState = playerState;
        _cameraSwitch = cameraSwitch;
    }

    protected float WalkSpeed => _walkSpeed;
    protected float RunSpeed => _runSpeed;
    protected PlayerState PlayerState => _playerState;
    protected CameraSwitch CameraSwitch => _cameraSwitch;
    protected CharacterController CharacterController => _characterController;
    protected Vector3 MoveDirection { get; set; }
    protected bool IsMoving {  get; set; }
    protected CompositeDisposable CompositeDisposable { get; private set; }

    protected virtual void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        CompositeDisposable = new CompositeDisposable();
    }

    protected virtual void OnDestroy() => CompositeDisposable?.Dispose();

    protected virtual void Update()
    {
        if (IsMoving)
            Move();
    }

    protected virtual void Move() => _characterController.Move(MoveDirection * Time.deltaTime);
}
