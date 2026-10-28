using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _menuPos;
    [SerializeField] private Transform _gameplayPos;
    private Vector3 _targetPos;
    private Vector3 _targetRotate;
    [SerializeField] private Vector3 _menuRotation;
    [SerializeField] private Vector3 _gameplayRotation;
    [SerializeField] private int _smoothSpeed;
    [SerializeField] private float _cameraSmoothSpeed;
    [SerializeField] private CameraLineChange _cameraLineChange;

    private void Start()
    {
        _targetPos = _menuPos.position;
        _targetRotate = _menuRotation;
    } 
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _targetPos, _cameraSmoothSpeed*Time.deltaTime);

        Quaternion targetRotation = Quaternion.Euler(_targetRotate);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _smoothSpeed * Time.deltaTime);
    }

    public void SetGameplayMode()
    {
        _targetPos = _gameplayPos.position; 
        _targetRotate = _gameplayRotation;
        ChangeLineMode(true);
    }

    public void SetMenuMode()
    {
        _targetPos = _menuPos.position; 
        _targetRotate = _menuRotation;
        ChangeLineMode(false);
    }

    public void ChangeLineMode(bool isGameplay)
    {
        _cameraLineChange.IsGameplay = isGameplay;
    }
}
