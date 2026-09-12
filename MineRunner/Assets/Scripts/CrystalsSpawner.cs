using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _crystalParent;
    [Range(0f,1f)]
    [SerializeField] private float _chanceToSpawn = 0.5f;

    void Start()
    {
        float chance = UnityEngine.Random.Range(0f, 1f);
        if(_chanceToSpawn >= chance)
        {
            _crystalParent.SetActive(true);
        }
    }
}
