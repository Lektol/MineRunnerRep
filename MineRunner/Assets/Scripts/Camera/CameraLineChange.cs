using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс для объекта GameplayTransform, чтобы камера следовала по горизонтали за игроком.
/// </summary>
/// 
public class CameraLineChange : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _cameraSmoothSpeed;
    private Vector3 _targetPos;
    public bool IsGameplay;

    private void Update()
    {
        if(IsGameplay) 
        {
            _targetPos = new Vector3(-_playerTransform.position.z, transform.localPosition.y , transform.localPosition.z);
        }
        else
        {
            _targetPos = Vector3.zero;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, _targetPos, _cameraSmoothSpeed*Time.deltaTime);
    }
}
