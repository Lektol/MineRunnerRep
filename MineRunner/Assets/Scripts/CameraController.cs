using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _menuPos;
    [SerializeField] private Transform _mainPos;
    private Vector3 _targetPos;
    private Vector3 _targetRotate;
    [SerializeField] private Vector3 _menuRotation;
    [SerializeField] private Vector3 _mainRotation;
    [SerializeField] private int _smoothSpeed;
    [SerializeField] private float _cameraSmoothSpeed;

    void Start()
    {
        _targetPos = _menuPos.position;
        _targetRotate = _menuRotation;
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
        transform.position = Vector3.Lerp(transform.position, _targetPos, _cameraSmoothSpeed*Time.deltaTime);

        Quaternion targetRotation = Quaternion.Euler(_targetRotate);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _smoothSpeed * Time.deltaTime);
    }

    void SetMainPos()
    {
        _targetPos = _mainPos.position; 
        _targetRotate = _mainRotation;
    }

    void SetMenuPos()
    {
        _targetPos = _menuPos.position; 
        _targetRotate = _menuRotation;
    }
}
