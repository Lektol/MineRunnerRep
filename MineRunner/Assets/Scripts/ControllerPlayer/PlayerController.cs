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
    [SerializeField] private float Gravity = -40f;
    private bool IsFlying = false;
    private bool IsDown = false;
    private bool IsWheelsRotating = false;
    [SerializeField] private float secToDown = 2;
    [SerializeField] private Transform[] Wheels;
    private Rigidbody rb;
    private Coroutine coroutineDown;
    private IControllable Controllable;
    public bool IsPc = true;

    void Start()
    {
        Physics.gravity = new Vector3(0,Gravity,0);
        targetPos = transform.position;
        rb = GetComponent<Rigidbody>();
        Controllable = IsPc ? gameObject.AddComponent<PcController>() : gameObject.AddComponent<MobileController>();
    }

    void OnEnable()
    {
        EventManager.OnLooseGame += SetStartPosAndStats;
        EventManager.OnStartGame += WheelRotate;
    }

    void OnDisable()
    {
        EventManager.OnLooseGame -= SetStartPosAndStats;
        EventManager.OnStartGame -= WheelRotate;
    }

    void Update()
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
            Debug.Log("Летим вниз");
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

    void WheelRotate()
    {
        IsWheelsRotating = true;
    }

    void SetStartPosAndStats()
    {
        transform.position = new Vector3(0,0,0);
        IsWheelsRotating = false;
        targetPos = transform.position;
        currentLine = 2;
        StopAllCoroutines();
        IsDown = false;
    }
}
