using System.Collections;
using YG;
using UnityEngine;

[RequireComponent(typeof(PlayerCollision))]
public class PlayerController : MonoBehaviour
{
    private int _currentLine = 2;
    private Vector3 _targetPos;
    [SerializeField] private float _lineChangeSpeed = 30f;
    [SerializeField] private float _jumpPower = 20f;
    [SerializeField] private float _jumpDeadPower = 25f;
    [SerializeField] private float _gravity = -40f;
    private bool _isWheelsRotating = false;
    private bool _canControll = false;
    [SerializeField] private float _secToDown = 1;
    [SerializeField] private float _secToInvincible = 1;
    [SerializeField] private Transform[] _wheels;
    private Rigidbody _rb;
    private Animator _animator;
    private Coroutine _coroutineDown;
    private IControllable _controllable;
    private PlayerCollision _playerCollision;
    //public bool IsPc = true;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _playerCollision = GetComponent<PlayerCollision>();
    }

    void Start()
    {
        Physics.gravity = new Vector3(0,_gravity,0);
        _targetPos = transform.position;
        _controllable = YG2.envir.isDesktop ? gameObject.AddComponent<PcController>() : gameObject.AddComponent<MobileController>();
    }

    void OnEnable()
    {
        EventManager.OnLoseGame += Dead;
        EventManager.OnStartGame += StartPlayer;
        EventManager.OnRebirth += Rebirth;
        EventManager.OnResetGame += SetStartPosAndStats;
    }

    void OnDisable()
    {
        EventManager.OnLoseGame -= Dead;
        EventManager.OnStartGame -= StartPlayer;
        EventManager.OnRebirth -= Rebirth;
        EventManager.OnResetGame -= SetStartPosAndStats;
    }

    void Update()
    {
        if(_canControll == true)
        {
            _targetPos = new Vector3(_targetPos.x, transform.position.y, _targetPos.z);
            if (_controllable.IsLeft() && _currentLine > 1)
            {
                _targetPos = new Vector3(transform.position.x, transform.position.y, _targetPos.z += 9);
                _currentLine--;
            }
            if (_controllable.IsRight() && _currentLine < 3)
            {
                _targetPos = new Vector3(transform.position.x, transform.position.y, _targetPos.z -= 9);
                _currentLine++;
            }
            if (_controllable.IsUp() && !_playerCollision.IsFlying)  //&& transform.position.z % 9 == 0
            {
                Jump();
            }
            if (_controllable.IsDown() && !_playerCollision.IsDown && !_playerCollision.IsFlying) 
            {
                _coroutineDown = StartCoroutine(Down());
            }
            else if(_controllable.IsDown() && _playerCollision.IsFlying)
            {
                MoveDown();
            }
        }
        else
        {
            _targetPos = transform.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, _targetPos, _lineChangeSpeed * Time.deltaTime); 

        if (_isWheelsRotating)
        {
            foreach (var wheel in _wheels)
            {
                wheel.Rotate(0, 0 , RoadGenerator.Instance.MaxSpeed * -10 * Time.deltaTime);
                if(wheel.rotation.z <= -360) wheel.rotation = Quaternion.identity;;
            }
        }
    }
    

    void Rebirth()
    {
        SetStartPosAndStats();
        StartPlayer();
        StartCoroutine(Invincible());
    }

    IEnumerator Invincible()
    {
        _playerCollision.IsInvincible = true;
        yield return new WaitForSeconds(_secToInvincible);
        _playerCollision.IsInvincible = false;
    }

    public IEnumerator Down()
    {
        _playerCollision.IsDown = true;
        yield return new WaitForSeconds(_secToDown);
        _playerCollision.IsDown = false;
    }

    void MoveDown()
    {
        _rb.AddForce(Vector3.down * _jumpPower, ForceMode.Impulse);
        _playerCollision.RequestToDown = true;
    }


    void Jump()
    {
        _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
        if(_coroutineDown != null)
        {
            StopCoroutine(_coroutineDown);
        }
        _playerCollision.IsDown = false;
    }

    void StartPlayer()
    {
        _isWheelsRotating = true;
        _canControll = true;
        _animator.SetTrigger("StartGame");
    }

    void Dead()
    {
        _canControll = false;
        _animator.SetTrigger("Dead");
        _rb.constraints &= ~RigidbodyConstraints.FreezeRotationX & ~RigidbodyConstraints.FreezeRotationY & ~RigidbodyConstraints.FreezeRotationZ;
        float z = (_currentLine <= 2) ? 1f : -1f;
        _rb.AddForce(new Vector3(-0.35f, 1, z) * _jumpDeadPower, ForceMode.Impulse);
        //StartCoroutine(AfterDead());
    }

    void SetStartPosAndStats()
    {
        _playerCollision.SetStartStats();
        _isWheelsRotating = false;
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        _rb.velocity = new Vector3(0,0,0);
        transform.position = new Vector3(0,0,0);
        transform.rotation = Quaternion.identity;
        _targetPos = transform.position;
        _currentLine = 2;
        //StopAllCoroutines();
        _animator.SetTrigger("RestartGame");
    }
}