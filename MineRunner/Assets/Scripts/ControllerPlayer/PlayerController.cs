using System.Collections;
using YG;
using UnityEngine;

[RequireComponent(typeof(PlayerCollision))]
public class PlayerController : MonoBehaviour
{
    private int currentLine = 2;
    private Vector3 targetPos;
    [SerializeField] private float lineChangeSpeed = 30f;
    [SerializeField] private float jumpPower = 20f;
    [SerializeField] private float jumpDeadPower = 25f;
    [SerializeField] private float Gravity = -40f;
    private bool IsWheelsRotating = false;
    private bool CanControll = false;
    [SerializeField] private float secToDown = 1;
    [SerializeField] private float secToInvincible = 1;
    [SerializeField] private Transform[] Wheels;
    private Rigidbody rb;
    private Animator animator;
    private Coroutine coroutineDown;
    private IControllable Controllable;
    private PlayerCollision playerCollision;
    //public bool IsPc = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerCollision = GetComponent<PlayerCollision>();
    }

    void Start()
    {
        Physics.gravity = new Vector3(0,Gravity,0);
        targetPos = transform.position;
        Controllable = YG2.envir.isDesktop ? gameObject.AddComponent<PcController>() : gameObject.AddComponent<MobileController>();
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
            if (Controllable.IsUp() && !playerCollision.IsFlying)  //&& transform.position.z % 9 == 0
            {
                Jump();
            }
            if (Controllable.IsDown() && !playerCollision.IsDown && !playerCollision.IsFlying) 
            {
                coroutineDown = StartCoroutine(Down());
            }
            else if(Controllable.IsDown() && playerCollision.IsFlying)
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
    

    void Rebirth()
    {
        SetStartPosAndStats();
        StartPlayer();
        StartCoroutine(Invincible());
    }

    IEnumerator Invincible()
    {
        playerCollision.IsInvincible = true;
        yield return new WaitForSeconds(secToInvincible);
        playerCollision.IsInvincible = false;
    }

    public IEnumerator Down()
    {
        playerCollision.IsDown = true;
        yield return new WaitForSeconds(secToDown);
        playerCollision.IsDown = false;
    }

    void MoveDown()
    {
        rb.AddForce(Vector3.down * jumpPower, ForceMode.Impulse);
        playerCollision.RequestToDown = true;
    }


    void Jump()
    {
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        if(coroutineDown != null)
        {
            StopCoroutine(coroutineDown);
        }
        playerCollision.IsDown = false;
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
        animator.SetTrigger("Dead");
        rb.constraints &= ~RigidbodyConstraints.FreezeRotationX & ~RigidbodyConstraints.FreezeRotationY & ~RigidbodyConstraints.FreezeRotationZ;
        float z = (currentLine <= 2) ? 1f : -1f;
        rb.AddForce(new Vector3(-0.35f, 1, z) * jumpDeadPower, ForceMode.Impulse);
        //StartCoroutine(AfterDead());
    }

    void SetStartPosAndStats()
    {
        playerCollision.SetStartStats();
        IsWheelsRotating = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        rb.velocity = new Vector3(0,0,0);
        transform.position = new Vector3(0,0,0);
        transform.rotation = Quaternion.identity;
        targetPos = transform.position;
        currentLine = 2;
        //StopAllCoroutines();
        animator.SetTrigger("RestartGame");
    }
}
