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
        Boulder.OnBoulderFalled += ShakeCamera;
    }

    private void OnDisable()
    {
        Boulder.OnBoulderFalled -= ShakeCamera;
        if(_coroutine == null) return;
        StopCoroutine(_coroutine);
        _coroutine = null;
    }


    private IEnumerator Shake(float duration, float magnitude)
    {
        _originalPosition = transform.position;
        _elapsedTime = 0f;

        while (_elapsedTime < duration)
        {
            float x = Random.Range(-magnitude, magnitude);
            float y = Random.Range(-magnitude, magnitude);

            transform.position = _originalPosition + new Vector3(x, y, 0);

            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = _originalPosition;
        _coroutine = null;
    }

    public void ShakeCamera(float duration, float magnitude)
    {
        if(_coroutine != null) return;
        _coroutine = StartCoroutine(Shake(duration, magnitude));
    }
}


