using System.Collections;
using UnityEngine;
using YG;

public class PlayerMovement : MonoBehaviour
{
    private enum VerticalState
    {
        Grounded,
        Jumping,
        Falling,
        Dead
    }

    [Header("Lane movement")]
    [SerializeField] private float _lineChangeSpeed = 45f;
    [SerializeField] private float _lineWidth = 9f;

    [Header("Jump")]
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private float _jumpPower = 7f;
    [SerializeField] private float _jumpDuration = 1f;

    [Header("Gravity")]
    [Tooltip("Отрицательное значение.")]
    [SerializeField] private float _gravity = -60f;

    [Tooltip("Начальная скорость вниз при быстром падении.")]
    [SerializeField] private float _downImpulse = 35f;

    [Tooltip("Максимальная скорость падения.")]
    [SerializeField] private float _maxFallSpeed = -60f;

    [Header("Ground check")]
    [SerializeField] private LayerMask _groundMask;

    [SerializeField] private float _groundCheckDistance = 5f;
    [SerializeField] private float _groundCheckOffset = 0.5f;

    [SerializeField] private float _groundCheckOffsetX1 = 3f;
    [SerializeField] private float _groundCheckOffsetX2 = -3f;

    [SerializeField] private float _groundCheckOffsetZ1 = 1.5f;
    [SerializeField] private float _groundCheckOffsetZ2 = -1.5f;

    [Tooltip("Высота pivot игрока относительно поверхности земли.")]
    [SerializeField] private float _playerFeetOffset = 3.7f;

    [Header("Slide")]
    [SerializeField] private float _secToDown = 1f;

    private int _currentLine = 2;

    private Vector3 _targetPos;

    private float _currentY;
    private float _verticalVelocity;

    private float _jumpStartY;
    private float _jumpProgress;
    private float _jumpMultiplier = 1f;

    private VerticalState _verticalState = VerticalState.Grounded;

    private bool _canControl;

    private Coroutine _coroutineDown;

    private IControllable _controllable;

    private PlayerCollision _playerCollision;

    private void Awake()
    {
        _playerCollision = GetComponent<PlayerCollision>();
    }

    private void Start()
    {
        CreateController();

        _targetPos = transform.position;
        _currentY = transform.position.y;
    }

    private void Update()
    {
        HandleInput();
        HandleHorizontal();
        HandleVertical();
        ApplyPosition();
    }

    private void CreateController()
    {
        if (YG2.envir.isDesktop)
        {
            _controllable = gameObject.GetComponent<PcController>();

            if (_controllable == null)
                _controllable = gameObject.AddComponent<PcController>();
        }
        else
        {
            _controllable = gameObject.GetComponent<MobileController>();

            if (_controllable == null)
                _controllable = gameObject.AddComponent<MobileController>();
        }
    }

    private void HandleInput()
    {
        if (!_canControl || _controllable == null)
            return;

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

        if (_controllable.IsUp() && _verticalState == VerticalState.Grounded)
        {
            Jump();
        }

        if (_controllable.IsDown())
        {
            if (_verticalState == VerticalState.Jumping ||
                _verticalState == VerticalState.Falling)
            {
                MoveDown();
            }
            else if (_verticalState == VerticalState.Grounded &&
                     !_playerCollision.IsDown)
            {
                StartDown();
            }
        }
    }

    private void HandleHorizontal()
    {
        if (!_canControl)
        {
            _targetPos = transform.position;
        }
    }

    private void HandleVertical()
    {
        switch (_verticalState)
        {
            case VerticalState.Grounded:
                TickGrounded();
                break;

            case VerticalState.Jumping:
                TickJump();
                break;

            case VerticalState.Falling:
                TickFall();
                break;

            case VerticalState.Dead:
                break;
        }
    }

    private void TickGrounded()
    {
        _verticalVelocity = 0f;

        if (!TryGetGroundY(out float groundY))
        {
            _verticalState = VerticalState.Falling;
            _verticalVelocity = 0f;
            return;
        }

        _currentY = groundY;
    }

    private void TickJump()
    {
        _jumpProgress += Time.deltaTime / _jumpDuration;

        if (_jumpProgress >= 1f)
        {
            _jumpProgress = 1f;

            if (TryGetGroundY(out float groundY))
            {
                _currentY = groundY;
                _verticalVelocity = 0f;
                _verticalState = VerticalState.Grounded;
            }
            else
            {
                _verticalState = VerticalState.Falling;
                _verticalVelocity = 0f;
            }

            return;
        }

        _currentY =
            _jumpStartY +
            _animationCurve.Evaluate(_jumpProgress) *
            _jumpPower *
            _jumpMultiplier;
    }

    private void TickFall()
    {
        _verticalVelocity += _gravity * Time.deltaTime;

        if (_verticalVelocity < _maxFallSpeed)
            _verticalVelocity = _maxFallSpeed;

        _currentY += _verticalVelocity * Time.deltaTime;

        if (!TryGetGroundY(out float groundY))
            return;

        if (_currentY <= groundY)
        {
            _currentY = groundY;
            _verticalVelocity = 0f;
            _verticalState = VerticalState.Grounded;

            if (_playerCollision.RequestToDown)
            {
                _playerCollision.RequestToDown = false;

                if (_coroutineDown == null)
                    _coroutineDown = StartCoroutine(Down());
            }
        }
    }

    private void ApplyPosition()
    {
        if (_verticalState == VerticalState.Dead)
            return;

        Vector3 position = transform.position;

        position.x = Mathf.MoveTowards(
            position.x,
            _targetPos.x,
            _lineChangeSpeed * Time.deltaTime
        );

        position.z = Mathf.MoveTowards(
            position.z,
            _targetPos.z,
            _lineChangeSpeed * Time.deltaTime
        );

        position.y = _currentY;

        transform.position = position;
    }

    private bool TryGetGroundY(out float groundY)
    {
        float rayLength = _groundCheckOffset + _groundCheckDistance;

        Vector3[] origins =
        {
            new Vector3(
                transform.position.x,
                _currentY + _groundCheckOffset,
                transform.position.z
            ),

            new Vector3(
                transform.position.x + _groundCheckOffsetX1,
                _currentY + _groundCheckOffset,
                transform.position.z + _groundCheckOffsetZ1
            ),

            new Vector3(
                transform.position.x + _groundCheckOffsetX2,
                _currentY + _groundCheckOffset,
                transform.position.z + _groundCheckOffsetZ2
            )
        };

        bool foundGround = false;
        float closestDistance = float.MaxValue;
        float bestGroundY = _currentY;

        foreach (Vector3 origin in origins)
        {
            if (!Physics.Raycast(
                    origin,
                    Vector3.down,
                    out RaycastHit hit,
                    rayLength,
                    _groundMask))
            {
                continue;
            }

            if (hit.distance < closestDistance)
            {
                closestDistance = hit.distance;
                bestGroundY = hit.point.y + _playerFeetOffset;
                foundGround = true;
            }

            Debug.DrawRay(
                origin,
                Vector3.down * rayLength,
                Color.green
            );
        }

        if (!foundGround)
        {
            foreach (Vector3 origin in origins)
            {
                Debug.DrawRay(
                    origin,
                    Vector3.down * rayLength,
                    Color.red
                );
            }
        }

        groundY = bestGroundY;
        return foundGround;
    }

    public void Jump(float multiplier = 1f)
    {
        if (_verticalState == VerticalState.Dead)
            return;

        if (_verticalState == VerticalState.Jumping)
            return;

        StopDown();

        _jumpStartY = _currentY;
        _jumpProgress = 0f;
        _jumpMultiplier = multiplier;
        _verticalVelocity = 0f;

        _verticalState = VerticalState.Jumping;
    }

    public void MoveDown()
    {
        if (_verticalState == VerticalState.Dead)
            return;

        _playerCollision.RequestToDown = true;

        if (_verticalState == VerticalState.Jumping)
        {
            _verticalState = VerticalState.Falling;
            _verticalVelocity = -_downImpulse;
        }
        else if (_verticalState == VerticalState.Falling)
        {
            _verticalVelocity = Mathf.Min(
                _verticalVelocity,
                -_downImpulse
            );
        }
    }

    private void StartDown()
    {
        if (_coroutineDown != null)
            return;

        _coroutineDown = StartCoroutine(Down());
    }

    private IEnumerator Down()
    {
        _playerCollision.SetDown(true);

        yield return new WaitForSeconds(_secToDown);

        _playerCollision.SetDown(false);

        _coroutineDown = null;
    }

    private void StopDown()
    {
        if (_coroutineDown == null)
            return;

        StopCoroutine(_coroutineDown);
        _coroutineDown = null;

        _playerCollision.SetDown(false);
    }

    public void StartMovement()
    {
        _canControl = true;
    }

    public void StopMovement()
    {
        _canControl = false;
    }

    public void SetDead()
    {
        StopMovement();
        StopDown();

        _verticalState = VerticalState.Dead;
    }

    public void ResetMovement(Vector3 startPosition)
    {
        StopDown();

        _canControl = false;

        _currentLine = 2;

        _targetPos = startPosition;
        _currentY = startPosition.y;

        _verticalVelocity = 0f;

        _jumpStartY = startPosition.y;
        _jumpProgress = 0f;
        _jumpMultiplier = 1f;

        _verticalState = VerticalState.Grounded;

        _playerCollision.RequestToDown = false;
        _playerCollision.SetDown(false);

        transform.position = startPosition;
    }

    public bool IsDead()
    {
        return _verticalState == VerticalState.Dead;
    }
}