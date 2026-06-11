using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int currentLine = 2;
    private Vector3 targetPos;
    [SerializeField] private float lineChangeSpeed = 30f;
    [SerializeField] private float jumpPower = 20f;
    [SerializeField] private float jumpDeadPower = 25f;
    [SerializeField] private float Gravity = -40f;
    [SerializeField] private float timeDead = 3f;
    private bool IsFlying = false;
    private bool isDown = false;
    private bool IsDown
    {
        get { return isDown; }
        set
        {
            isDown = value;
            animator.SetBool("IsDown", value);
        }
    }
    private bool IsWheelsRotating = false;
    private bool CanControll = false;
    [SerializeField] private float secToDown = 2;
    [SerializeField] private Transform[] Wheels;
    private Rigidbody rb;
    private Animator animator;
    private Coroutine coroutineDown;
    private IControllable Controllable;
    public bool IsPc = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        Physics.gravity = new Vector3(0,Gravity,0);
        targetPos = transform.position;
        Controllable = IsPc ? gameObject.AddComponent<PcController>() : gameObject.AddComponent<MobileController>();
    }

    void OnEnable()
    {
        //EventManager.OnLooseGame += SetStartPosAndStats;
        EventManager.OnLooseGame += Dead;
        EventManager.OnStartGame += StartPlayer;
    }

    void OnDisable()
    {
        //EventManager.OnLooseGame -= SetStartPosAndStats;
        EventManager.OnLooseGame -= Dead;
        EventManager.OnStartGame -= StartPlayer;
    }

    void Update()
    {
        if(CanControll == true)
        {
            targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
            if (Controllable.IsLeft() && currentLine > 1)
            {
                targetPos = new Vector3(transform.position.x, transform.position.y, targetPos.z += 9);
                currentLine--;
            }
            if (Controllable.IsRight() && currentLine < 3)
            {
                targetPos = new Vector3(transform.position.x, transform.position.y, targetPos.z -= 9);
                currentLine++;
            }
            if (Controllable.IsUp() && !IsFlying)  //&& transform.position.z % 9 == 0
            {
                Jump();
            }
            if (Controllable.IsDown() && !IsDown && !IsFlying) 
            {
                coroutineDown = StartCoroutine(Down());
            }
            else if(Controllable.IsDown() && IsFlying)
            {
                MoveDown();
            }
        }
        else
        {
            targetPos = transform.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPos, lineChangeSpeed * Time.deltaTime); 

        if (IsWheelsRotating)
        {
            foreach (var wheel in Wheels)
            {
                wheel.Rotate(0, 0 , RoadGenerator.Instance.maxSpeed * -10 * Time.deltaTime);
                if(wheel.rotation.z <= -360) wheel.rotation = Quaternion.identity;;
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rails"))
        {
            IsFlying = false;
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
        if (other.gameObject.CompareTag("Barrier"))
        {
            EventManager.OnLooseGameInvoke();
        }

        if (other.gameObject.CompareTag("BarrierDown") && !IsDown)
        {
            EventManager.OnLooseGameInvoke();
        }
    }

    IEnumerator Down()
    {
        IsDown = true;
        yield return new WaitForSeconds(secToDown);
        IsDown = false;
    }

    void MoveDown()
    {
        rb.AddForce(Vector3.down * jumpPower, ForceMode.Impulse);
    }


    void Jump()
    {
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        if(coroutineDown != null)
        {
            StopCoroutine(coroutineDown);
        }
        IsDown = false;
    }

    void StartPlayer()
    {
        IsWheelsRotating = true;
        CanControll = true;
        animator.SetTrigger("StartGame");
    }

    void Dead()
    {
        CanControll = false;
        rb.constraints &= ~RigidbodyConstraints.FreezeRotationX & ~RigidbodyConstraints.FreezeRotationY;
        float z = (currentLine <= 2) ? 1f : -1f;
        rb.AddForce(new Vector3(-0.4f, 1, z) * jumpDeadPower, ForceMode.Impulse);
        StartCoroutine(AfterDead());
    }

    IEnumerator AfterDead()
    {
        yield return new WaitForSeconds(timeDead);
        SetStartPosAndStats();
        EventManager.OnRestartGameInvoke();
    }

    void SetStartPosAndStats()
    {
        IsWheelsRotating = false;
        IsDown = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        transform.position = new Vector3(0,0,0);
        transform.rotation = Quaternion.identity;
        targetPos = transform.position;
        currentLine = 2;
        StopAllCoroutines();
        animator.SetTrigger("RestartGame");
    }
}
