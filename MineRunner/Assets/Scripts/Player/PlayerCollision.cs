using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    //[SerializeField] private GameFlow _gameFlow;
    private bool _isInvincible;
    public bool IsInvincible
    {
        get => _isInvincible;

        set
        {
            _isInvincible = value;

            if (_playerView != null)
                _playerView.SetInvincible(value);
        }
    }

    public bool RequestToDown { get; set; }

    public bool IsDown { get; private set; }

    private PlayerMovement _playerMovement;
    private PlayerView _playerView;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerView = GetComponent<PlayerView>();
    }

    public void SetStartStats()
    {
        RequestToDown = false;
        SetDown(false);
        IsInvincible = false;
    }

    public void SetDown(bool value)
    {
        IsDown = value;

        if (_playerView != null)
            _playerView.SetDown(value);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Barrier") && !IsInvincible)
        {
            EventManager.OnPlayerDiedInvoke();
            return;
        }

        if (other.CompareTag("BarrierDown") && !IsDown && !IsInvincible)
        {
            EventManager.OnPlayerDiedInvoke();
            return;
        }

        if (other.CompareTag("FallingDeadZone") && !IsInvincible)
        {
            EventManager.OnPlayerDiedInvoke();
            return;
        }

        if (other.CompareTag("FallingDeadZone") && IsInvincible)
        {
            _playerMovement.ResetMovementAndPosition(Vector3.zero);
            _playerMovement.StartMovement();
            return;
        }

        if (other.CompareTag("Crystal"))
        {
            EventManager.OnGetCrystalInvoke();

            Destroy(other.gameObject);
            return;
        }

        if (other.CompareTag("JumpSpring") && RequestToDown)
        {
            RequestToDown = false;

            _playerMovement.Jump(_playerMovement.JumpMultiplySpring);
            other.GetComponent<Animator>().SetTrigger("Jump");
        }
    }
}