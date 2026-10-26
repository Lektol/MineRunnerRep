using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBouldersRoad : MonoBehaviour
{
    [Serializable]
    public struct BoulderPoint
    {
        public Transform[] SpawnPointsJumpBoulder;
        public Transform[] SpawnPointsBigBoulder;
    }

    [SerializeField] private BoulderPoint[] _boulderPoints;
    [SerializeField] private float _secBetweenPoints;
    [SerializeField] private ObjectPool _jumpBouldersPool;
    [SerializeField] private ObjectPool _bigBouldersPool;
    private Coroutine _coroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartIterarionCoroutine();
        }
    }

    private void OnDisable()
    {
        _jumpBouldersPool.DisableAll();
        _bigBouldersPool.DisableAll();
    }

    private void StartIterarionCoroutine()
    {
        if(_coroutine == null) _coroutine = StartCoroutine(IterationOfBoulderPoints());
    }

    private IEnumerator IterationOfBoulderPoints()
    {
        foreach (var point in _boulderPoints)
        {
            yield return new WaitForSeconds(_secBetweenPoints);
            float chance = UnityEngine.Random.value;
            if (chance >= 0.6f)
            {
                int indexPos = UnityEngine.Random.Range(0, point.SpawnPointsJumpBoulder.Length);
                if (_jumpBouldersPool.TryGetObject(out GameObject newBoulder, point.SpawnPointsJumpBoulder[indexPos].position, Quaternion.identity))
                {
                    continue;
                }
            }
            int indexPos2 = UnityEngine.Random.Range(0, point.SpawnPointsBigBoulder.Length);
            if (_bigBouldersPool.TryGetObject(out GameObject newBoulder2, point.SpawnPointsBigBoulder[indexPos2].position, Quaternion.identity))
            {
                continue;
            }
            int indexPos3 = UnityEngine.Random.Range(0, point.SpawnPointsJumpBoulder.Length);
            if (_jumpBouldersPool.TryGetObject(out GameObject newBoulder3, point.SpawnPointsJumpBoulder[indexPos3].position, Quaternion.identity))
            {
                continue;
            }
        }
        _coroutine = null;
    }

}
