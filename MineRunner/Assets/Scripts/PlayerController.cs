using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int currentLine = 2;
    private Vector3 targetPos;
    [SerializeField] private float lineChangeSpeed = 30f;
    [SerializeField] private float jumpPower = 20f;
    [SerializeField] private float jumpGravity = -40f;
    public const float realGravity = 9.8f;
    [SerializeField] private bool IsFlying = false;
    [SerializeField] private bool IsDown = false;
    [SerializeField] private float secToDown = 2;
    private Rigidbody rb;
    private Coroutine coroutineDown;

    void Start()
    {
        Physics.gravity = new Vector3(0,jumpGravity,0);
        targetPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        EventManager.OnLooseGame += SetStartPosAndStats;
    }

    void OnDisable()
    {
        EventManager.OnLooseGame -= SetStartPosAndStats;
    }

    void Update()
    {
        targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
        if (Input.GetKeyDown(KeyCode.A) && currentLine > 1)
        {
            targetPos = new Vector3(transform.position.x, transform.position.y, targetPos.z += 9);
            currentLine--;
        }
        if (Input.GetKeyDown(KeyCode.D) && currentLine < 3)
        {
            targetPos = new Vector3(transform.position.x, transform.position.y, targetPos.z -= 9);
            currentLine++;
        }
        if (Input.GetKeyDown(KeyCode.Space) && !IsFlying)  //&& transform.position.z % 9 == 0
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.S) && !IsDown && !IsFlying) 
        {
            coroutineDown = StartCoroutine(Down());
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPos, lineChangeSpeed * Time.deltaTime);
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
            EventManager.OnLooseGame?.Invoke();
        }

        if (other.gameObject.CompareTag("BarrierDown") && !IsDown)
        {
            EventManager.OnLooseGame?.Invoke();
        }
    }

    IEnumerator Down()
    {
        IsDown = true;
        yield return new WaitForSeconds(secToDown);
        IsDown = false;
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

    void SetStartPosAndStats()
    {
        transform.position = new Vector3(0,0,0);
        targetPos = transform.position;
        currentLine = 2;
        StopAllCoroutines();
        IsDown = false;
    }
}
