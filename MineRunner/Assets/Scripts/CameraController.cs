using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform MenuPos;
    [SerializeField] private Transform MainPos;
    private Vector3 TargetPos;
    [SerializeField] private Vector3 MenuRotation;
    [SerializeField] private Vector3 MainRotation;
    [SerializeField] private float cameraSpeed;

    void Start()
    {
        //TargetPos.position = MenuPos.position;
    } 
    void OnEnable()
    {
        EventManager.OnRestartGame += SetMenuPos;
        EventManager.OnStartGame += SetMainPos;
    }

    void OnDisable()
    {
        EventManager.OnRestartGame -= SetMenuPos;
        EventManager.OnStartGame -= SetMainPos;
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, TargetPos, cameraSpeed*Time.deltaTime);
    }

    void SetMainPos()
    {
        transform.eulerAngles = MainRotation;
        TargetPos = MainPos.position; 
    }

    void SetMenuPos()
    {
        transform.eulerAngles = MenuRotation;
        TargetPos = MenuPos.position; 
    }
}
