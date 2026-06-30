using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private bool isInvincible = false;
    public bool IsInvincible
    {
        get{ return isInvincible; }
        set
        {
            isInvincible = value;
            SphereInvincible.SetActive(value);
        }
    }
    
    public bool IsFlying = false;
    private bool isDown = false;
    public bool IsDown
    {
        get { return isDown; }
        set
        {
            isDown = value;
            animator.SetBool("IsDown", value);
        }
    }
    public bool RequestToDown = false;
    [SerializeField] GameObject SphereInvincible;
    private Animator animator;
    private PlayerController playerController;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
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
                playerController.StartCoroutine(playerController.Down());
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
