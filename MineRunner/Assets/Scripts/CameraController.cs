using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform MenuPos;
    [SerializeField] private Transform MainPos;
    private Vector3 TargetPos;
    private Vector3 TargetRotate;
    [SerializeField] private Vector3 MenuRotation;
    [SerializeField] private Vector3 MainRotation;
    [SerializeField] private int smoothSpeed;
    [SerializeField] private float cameraSmoothSpeed;

    void Start()
    {
        TargetPos = MenuPos.position;
        TargetRotate = MenuRotation;
    } 
    void OnEnable()
    {
        EventManager.OnResetGame += SetMenuPos;
        EventManager.OnStartGame += SetMainPos;
    }

    void OnDisable()
    {
        EventManager.OnResetGame -= SetMenuPos;
        EventManager.OnStartGame -= SetMainPos;
    }
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, TargetPos, cameraSmoothSpeed*Time.deltaTime);

        Quaternion targetRotation = Quaternion.Euler(TargetRotate);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
    }

    void SetMainPos()
    {
        TargetPos = MainPos.position; 
        TargetRotate = MainRotation;
    }

    void SetMenuPos()
    {
        TargetPos = MenuPos.position; 
        TargetRotate = MenuRotation;
    }
}
