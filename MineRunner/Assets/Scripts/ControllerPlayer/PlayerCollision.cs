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

            if (_view != null)
                _view.SetInvincible(value);
        }
    }

    public bool RequestToDown { get; set; }

    public bool IsDown { get; private set; }

    private PlayerMovement _movement;
    private PlayerView _view;

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        _view = GetComponent<PlayerView>();
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

        if (_view != null)
            _view.SetDown(value);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Barrier") && !IsInvincible)
        {
            EventManager.OnLoseGameInvoke();
            return;
        }

        if (other.CompareTag("BarrierDown") && !IsDown && !IsInvincible)
        {
            EventManager.OnLoseGameInvoke();
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

            _movement.Jump(1.7f);
        }
    }
}