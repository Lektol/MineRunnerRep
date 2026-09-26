using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private bool _isInvincible;
    public bool IsInvincible
    {
        get => _isInvincible;
        set
        {
            _isInvincible = value;
            _sphereInvincible.SetActive(value);
        }
    }

    // Оставлены для совместимости с аниматором и контроллером.
    // IsFlying больше не определяется коллизиями — его ставит PlayerController.
    public bool IsFlying { get; set; }
    public bool RequestToDown { get; set; }

    private bool _isDown;
    public bool IsDown
    {
        get => _isDown;
        set
        {
            _isDown = value;
            _animator.SetBool("IsDown", value);
        }
    }

    [SerializeField] private GameObject _sphereInvincible;
    private Animator _animator;
    private PlayerController _playerController;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerController = GetComponent<PlayerController>();
    }

    public void SetStartStats()
    {
        IsFlying = false;
        IsDown = false;
        RequestToDown = false;
        IsInvincible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Barrier") && !IsInvincible)
        {
            EventManager.OnLoseGameInvoke();
        }

        if (other.CompareTag("BarrierDown") && !IsDown && !IsInvincible)
        {
            EventManager.OnLoseGameInvoke();
        }

        if (other.CompareTag("Crystal"))
        {
            EventManager.OnGetCrystalInvoke();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("JumpSpring") && RequestToDown)
        {
            _playerController.Jump(1.5f);
            RequestToDown = false;
        }
    }
}