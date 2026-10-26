using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    private float _verticalVelocity;
    [SerializeField] private float _gravity;
    [SerializeField] private float _maxFallSpeed;
    [SerializeField] private float _magnitudeForCameraShake;
    [SerializeField] private float _durationForCameraShake;
    private bool _isFalled;
    [Header("Луч")]
    [SerializeField] private float _maxDistance;
    [SerializeField] private LayerMask _groundMask;

    private void OnEnable()
    {
        _verticalVelocity = 0;
        _isFalled = false;
    }
    private void Update()
    {
        Vector3 direction = Vector3.down;
        bool hit = Physics.Raycast(transform.position, direction, out RaycastHit hitInfo, _maxDistance, _groundMask);
        Debug.DrawRay(
                transform.position,
                Vector3.down * _maxDistance,
                Color.green
            );

        if(hit)
        {
            ChangeStateToFalled();
            return;
        }
        _verticalVelocity += _gravity * Time.deltaTime;

        if (_verticalVelocity < _maxFallSpeed)
            _verticalVelocity = _maxFallSpeed;

        transform.position += new Vector3(0,_verticalVelocity * Time.deltaTime, 0);
    }

    private void ChangeStateToFalled()
    {
        if(_isFalled) return;

        EventManager.RequestCameraShake(_durationForCameraShake, _magnitudeForCameraShake);
        _isFalled = true;
    }
}
