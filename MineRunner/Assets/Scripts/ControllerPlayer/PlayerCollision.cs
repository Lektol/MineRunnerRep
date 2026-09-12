using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private bool _isInvincible = false;
    public bool IsInvincible
    {
        get{ return _isInvincible; }
        set
        {
            _isInvincible = value;
            _sphereInvincible.SetActive(value);
        }
    }
    
    public bool IsFlying = false;
    private bool _isDown = false;
    public bool IsDown
    {
        get { return _isDown; }
        set
        {
            _isDown = value;
            _animator.SetBool("IsDown", value);
        }
    }
    public bool RequestToDown = false;
    [SerializeField] private GameObject _sphereInvincible;
    private Animator _animator;
    private PlayerController _playerController;

    void Awake()
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

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rails"))
        {
            IsFlying = false;
            if (RequestToDown)
            {
                _playerController.StartCoroutine(_playerController.Down());
                RequestToDown = false;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rails"))
        {
            IsFlying = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Barrier") && IsInvincible == false)
        {
            EventManager.OnLoseGameInvoke();
        }

        if (other.gameObject.CompareTag("BarrierDown") && !IsDown && IsInvincible == false)
        {
            EventManager.OnLoseGameInvoke();
        }

        if (other.gameObject.CompareTag("Crystal"))
        {
            EventManager.OnGetCrystalInvoke();
            Destroy(other.gameObject);
        }
    }
}