using R3;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CharacterController))]
public class PlayerVerticalMover : MonoBehaviour
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _gravity;

    private readonly CompositeDisposable _compositeDisposable = new();
    private PlayerState _playerState;
    private CharacterController _characterController;
    private Vector3 _motion;

    [Inject]
    private void Construct([Inject(Id = ZenjectIdConstants.PlayerId)] PlayerState playerState) => 
        _playerState = playerState;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        var jumpDisposable = _playerState.IsJumping
            .Where(value => value)
            .Subscribe(_ => _motion.y = _jumpForce)
            .AddTo(_compositeDisposable);

        var groundEnterDisposable = _playerState.IsGrounded
            .Where(value => value)
            .Subscribe(_ => _motion.y = -_gravity)
            .AddTo(_compositeDisposable);
    }

    private void OnDestroy() => _compositeDisposable?.Dispose();

    private void Update() => Fall();

    private void Fall()
    {
        if (!_playerState.IsGrounded.CurrentValue)
            _motion.y -= _gravity * Time.deltaTime;

        _characterController.Move(_motion * Time.deltaTime);
    }
}
