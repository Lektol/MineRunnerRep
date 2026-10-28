using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 _originalPosition; 
    private float _elapsedTime; 
    private Coroutine _coroutine;

    private void OnEnable()
    {
        EventManager.OnCameraShakeRequested += ShakeCamera;
    }

    private void OnDisable()
    {
        EventManager.OnCameraShakeRequested -= ShakeCamera;
        if(_coroutine == null) return;
        StopCoroutine(_coroutine);
        _coroutine = null;
    }


    private IEnumerator Shake(float duration, float magnitude)
    {
        _originalPosition = transform.localPosition;
        _elapsedTime = 0f;

        while (_elapsedTime < duration)
        {
            float x = Random.Range(-magnitude, magnitude);
            float y = Random.Range(-magnitude, magnitude);

            transform.localPosition = _originalPosition + new Vector3(x, y, 0);

            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = _originalPosition;
        _coroutine = null;
    }

    private void ShakeCamera(float duration, float magnitude)
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
            transform.localPosition = _originalPosition;
        }
        _coroutine = StartCoroutine(Shake(duration, magnitude));
    }
}


