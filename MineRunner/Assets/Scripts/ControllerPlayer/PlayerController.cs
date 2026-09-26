using System.Collections;
using YG;
using UnityEngine;

[RequireComponent(typeof(PlayerCollision))]
public class PlayerController : MonoBehaviour
{
    private enum VerticalState { Grounded, Jumping, Falling, Dead }

    private int _currentLine = 2;
    private Vector3 _targetPos;
    [SerializeField] private float _lineChangeSpeed = 45f;
    [SerializeField] private float _lineWidth = 9f;

    [Header("Jump")]
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private float _jumpPower = 7f;
    [SerializeField] private float _jumpDuration = 1f;

    [Header("Gravity")]
    [Tooltip("Отрицательное значение. Используется ТОЛЬКО в состоянии Falling.")]
    [SerializeField] private float _gravity = -50f;
    [Tooltip("Начальная скорость вниз при MoveDown, чтобы падение было резким.")]
    [SerializeField] private float _downImpulse = 15f;
    [Tooltip("Максимальная скорость падения, чтобы не проваливаться сквозь землю.")]
    [SerializeField] private float _maxFallSpeed = -60f;

    [Header("Ground check")]
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _groundCheckDistance = 0.6f;
    [SerializeField] private float _groundCheckOffset = 0.5f;
    [Tooltip("Для второго луча,который проверяет землю")]
    [SerializeField] private float _groundCheckOffsetX1 = 3f;
    [Tooltip("Для третьего луча,который проверяет землю")]
    [SerializeField] private float _groundCheckOffsetX2 = -3f;
    [Tooltip("Для второго луча,который проверяет землю")]
    [SerializeField] private float _groundCheckOffsetZ1 = 1f;
    [Tooltip("Для третьего луча,который проверяет землю")]
    [SerializeField] private float _groundCheckOffsetZ2 = -1f;

    [Header("Death")]
    [SerializeField] private float _jumpDeadPower = 25f;

    [Header("Slide")]
    [SerializeField] private float _secToDown = 1f;

    [Header("Misc")]
    [SerializeField] private float _secToInvincible = 1f;
    [SerializeField] private Transform[] _wheels;


    [SerializeField] private VerticalState _vState = VerticalState.Grounded;
    private float _currentY;
    private float _verticalVelocity;
    private float _jumpStartY;
    private float _jumpProgress;
    private float _jumpMultiplier = 1f;

    private bool _isWheelsRotating;
    private bool _canControll;
    private Coroutine _jumpCoroutine;
    private Coroutine _coroutineDown;

    private Rigidbody _rb;
    private Animator _animator;
    private IControllable _controllable;
    private PlayerCollision _playerCollision;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _playerCollision = GetComponent<PlayerCollision>();
    }

    private void Start()
    {
        // Rigidbody выключен по умолчанию — включаем только на смерть
        _rb.isKinematic = true;
        _rb.useGravity = false;

        _targetPos = transform.position;
        _currentY = transform.position.y;

        _controllable = YG2.envir.isDesktop
            ? gameObject.AddComponent<PcController>()
            : gameObject.AddComponent<MobileController>();
    }

    private void OnEnable()
    {
        EventManager.OnLoseGame += Dead;
        EventManager.OnStartGame += StartPlayer;
        EventManager.OnRebirth += Rebirth;
        EventManager.OnResetGame += SetStartPosAndStats;
    }

    private void OnDisable()
    {
        EventManager.OnLoseGame -= Dead;
        EventManager.OnStartGame -= StartPlayer;
        EventManager.OnRebirth -= Rebirth;
        EventManager.OnResetGame -= SetStartPosAndStats;

        TryToStopJumpCoroutine();
        if (_coroutineDown != null)
        {
            StopCoroutine(_coroutineDown);
            _coroutineDown = null;
        }
    }

    private void Update()
    {
        HandleInput();
        HandleHorizontal();
        HandleVertical();
        ApplyPosition();
        RotateWheels();
    }

    private void HandleInput()
    {
        if (!_canControll) return;

        if (_controllable.IsLeft() && _currentLine > 1)
        {
            _targetPos.z += _lineWidth;
            _currentLine--;
        }
        if (_controllable.IsRight() && _currentLine < 3)
        {
            _targetPos.z -= _lineWidth;
            _currentLine++;
        }
        if (_controllable.IsUp() && _vState == VerticalState.Grounded)
        {
            Jump();
        }
        if (_controllable.IsDown())
        {
            if (_vState == VerticalState.Jumping || _vState == VerticalState.Falling)
            {
                // В воздухе — ускоряем падение
                MoveDown();
            }
            else if (_vState == VerticalState.Grounded && !_playerCollision.IsDown)
            {
                _coroutineDown = StartCoroutine(Down());
            }
        }
    }

    private void HandleHorizontal()
    {
        if (!_canControll)
        {
            _targetPos = transform.position;
        }
    }

    private void HandleVertical()
    {
        switch (_vState)
        {
            case VerticalState.Grounded:
                _verticalVelocity = 0f;
                if (!IsGrounded())
                {
                    _vState = VerticalState.Falling;
                    _verticalVelocity = 0f;
                }
                else
                {
                    _currentY = GetGroundY();
                }
                break;

            case VerticalState.Jumping:
                TickJump();
                break;

            case VerticalState.Falling:
                TickFall();
                break;

            case VerticalState.Dead:
                // Rigidbody рулит — не трогаем Y
                return;
        }
    }

    private void TickJump()
    {
        _jumpProgress += Time.deltaTime / _jumpDuration;

        if (_jumpProgress >= 1f)
        {
            // Кривая закончилась. Если под нами ещё есть высота — переходим в падение,
            // чтобы гравитация дотянула до земли. Если уже на земле — Grounded.
            _jumpProgress = 1f;

            if (IsGrounded())
            {
                _vState = VerticalState.Grounded;
                _currentY = GetGroundY();
                _verticalVelocity = 0f;
                _jumpCoroutine = null;
            }
            else
            {
                _vState = VerticalState.Falling;
                _verticalVelocity = 0f;
                _jumpCoroutine = null;
            }
            return;
        }

        _currentY = _jumpStartY + _animationCurve.Evaluate(_jumpProgress) * _jumpPower * _jumpMultiplier;
    }

    private void TickFall()
    {
        _verticalVelocity += _gravity * Time.deltaTime;
        if (_verticalVelocity < _maxFallSpeed)
            _verticalVelocity = _maxFallSpeed;

        _currentY += _verticalVelocity * Time.deltaTime;

        float groundY = GetGroundY();
        if (_currentY <= groundY && IsGrounded())
        {
            _currentY = groundY;
            _verticalVelocity = 0f;
            _vState = VerticalState.Grounded;

            // Если игрок зажал вниз в воздухе — стартуем скольжение после приземления
            if (_playerCollision.RequestToDown)
            {
                _playerCollision.RequestToDown = false;
                _playerCollision.IsFlying = false;
                if (_coroutineDown == null)
                    _coroutineDown = StartCoroutine(Down());
            }
        }
    }

    private void ApplyPosition()
    {
        if (_vState == VerticalState.Dead) return;

        Vector3 pos = transform.position;
        pos.x = Mathf.MoveTowards(pos.x, _targetPos.x, _lineChangeSpeed * Time.deltaTime);
        pos.z = Mathf.MoveTowards(pos.z, _targetPos.z, _lineChangeSpeed * Time.deltaTime);
        pos.y = _currentY;
        transform.position = pos;
    }

    private bool IsGrounded()
    {
        Vector3 origin1 = new Vector3(transform.position.x, _currentY + _groundCheckOffset, transform.position.z);
        Vector3 origin2 = new Vector3(transform.position.x + _groundCheckOffsetX1, _currentY + _groundCheckOffset, transform.position.z + _groundCheckOffsetZ1);
        Vector3 origin3 = new Vector3(transform.position.x + _groundCheckOffsetX2, _currentY + _groundCheckOffset, transform.position.z + _groundCheckOffsetZ2);

        bool isGrounded = Physics.Raycast(origin1, Vector3.down, _groundCheckOffset + _groundCheckDistance, _groundMask) ||
        Physics.Raycast(origin2, Vector3.down, _groundCheckOffset + _groundCheckDistance, _groundMask) ||
        Physics.Raycast(origin3, Vector3.down, _groundCheckOffset + _groundCheckDistance, _groundMask);

        float length = _groundCheckOffset + _groundCheckDistance;
        Debug.DrawRay(origin1, Vector3.down * length, isGrounded ? Color.green : Color.red);
        Debug.DrawRay(origin2, Vector3.down * length, isGrounded ? Color.green : Color.red);
        Debug.DrawRay(origin3, Vector3.down * length, isGrounded ? Color.green : Color.red);

        return isGrounded;
    }

    private float GetGroundY()
    {
        Vector3 origin = new Vector3(transform.position.x, _currentY + _groundCheckOffset, transform.position.z);
        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, _groundCheckOffset + _groundCheckDistance, _groundMask))
        {
            Debug.Log("hitPoint" + hit.point.y + 3.7f);
            return hit.point.y + 3.7f;
        }
        Debug.Log(_currentY);
        return _currentY;
    }

    public void Jump(float multiplier = 1f)
    {
        if (_vState == VerticalState.Dead) return;
        if (_vState == VerticalState.Jumping) return;

        // Прерываем скольжение, если оно было
        if (_coroutineDown != null)
        {
            StopCoroutine(_coroutineDown);
            _coroutineDown = null;
            _playerCollision.IsDown = false;
        }

        _jumpStartY = _currentY;
        _jumpProgress = 0f;
        _jumpMultiplier = multiplier;
        _verticalVelocity = 0f;
        _vState = VerticalState.Jumping;
    }

    public void TryToStopJumpCoroutine()
    {
        // Оставлен для совместимости с PlayerCollision.
        // Мгновенно завершает прыжок, переводя в Falling/Grounded.
        if (_vState == VerticalState.Jumping)
        {
            _jumpProgress = 1f;
            TickJump();
        }
    }

    private void MoveDown()
    {
        if (_vState == VerticalState.Dead) return;

        _playerCollision.RequestToDown = true;

        if (_vState == VerticalState.Jumping)
        {
            // Прерываем кривую — дальше гравитация
            _vState = VerticalState.Falling;
            _verticalVelocity = -_downImpulse;
        }
        else if (_vState == VerticalState.Falling)
        {
            // Уже падаем — просто ускоряем
            _verticalVelocity = Mathf.Min(_verticalVelocity, -_downImpulse);
        }
    }

    public IEnumerator Down()
    {
        _playerCollision.IsDown = true;
        yield return new WaitForSeconds(_secToDown);
        _playerCollision.IsDown = false;
        _coroutineDown = null;
    }

    private void RotateWheels()
    {
        if (!_isWheelsRotating || _wheels == null) return;

        foreach (var wheel in _wheels)
        {
            wheel.Rotate(0, 0, RoadGenerator.Instance.MaxSpeed * -10 * Time.deltaTime);
            if (wheel.rotation.z <= -360f) wheel.rotation = Quaternion.identity;
        }
    }

    private void Dead()
    {
        if (_vState == VerticalState.Dead) return;

        _vState = VerticalState.Dead;
        _canControll = false;
        _isWheelsRotating = false;
        _animator.SetTrigger("Dead");

        TryToStopJumpCoroutine();
        if (_coroutineDown != null)
        {
            StopCoroutine(_coroutineDown);
            _coroutineDown = null;
        }
        _playerCollision.IsDown = false;

        // Включаем Rigidbody только сейчас
        _rb.isKinematic = false;
        _rb.useGravity = true;
        _rb.constraints = RigidbodyConstraints.None;

        float z = (_currentLine <= 2) ? 1f : -1f;
        _rb.AddForce(new Vector3(-0.35f, 1f, z) * _jumpDeadPower, ForceMode.Impulse);
    }

    private void Rebirth()
    {
        SetStartPosAndStats();
        StartPlayer();
        StartCoroutine(Invincible());
    }

    private IEnumerator Invincible()
    {
        _playerCollision.IsInvincible = true;
        yield return new WaitForSeconds(_secToInvincible);
        _playerCollision.IsInvincible = false;
    }

    private void StartPlayer()
    {
        _isWheelsRotating = true;
        _canControll = true;
        _animator.SetTrigger("StartGame");
    }

    private void SetStartPosAndStats()
    {
        _playerCollision.SetStartStats();
        _canControll = false;
        _isWheelsRotating = false;

        TryToStopJumpCoroutine();
        if (_coroutineDown != null)
        {
            StopCoroutine(_coroutineDown);
            _coroutineDown = null;
        }

        _rb.isKinematic = true;
        _rb.useGravity = false;
        _rb.constraints = RigidbodyConstraints.FreezeRotationX
                        | RigidbodyConstraints.FreezeRotationY
                        | RigidbodyConstraints.FreezeRotationZ;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        _targetPos = Vector3.zero;
        _currentY = 0f;
        _verticalVelocity = 0f;
        _vState = VerticalState.Grounded;
        _currentLine = 2;

        _animator.SetTrigger("RestartGame");
    }
}