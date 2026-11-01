using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс для объекта GameplayTransform, чтобы камера следовала по горизонтали и вертикали за игроком.
/// </summary>
/// 
public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _cameraSmoothSpeed;
    [SerializeField] private float _yPositionOffset;
    private Vector3 _targetPos;
    public bool IsGameplay;

    private void Update()
    {
        if(IsGameplay) 
        {
            _targetPos = new Vector3(-_playerTransform.position.z, _playerTransform.position.y + _yPositionOffset, transform.localPosition.z);
        }
        else
        {
            _targetPos = Vector3.zero;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, _targetPos, _cameraSmoothSpeed*Time.deltaTime);
    }
}
