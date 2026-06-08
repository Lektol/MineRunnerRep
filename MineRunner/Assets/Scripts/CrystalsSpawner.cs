using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject CrystalParent;
    [Range(0f,1f)]
    [SerializeField] private float chanceToSpawn = 0.5f;

    void Start()
    {
        float chance = UnityEngine.Random.Range(0f, 1f);
        if(chanceToSpawn >= chance)
        {
            CrystalParent.SetActive(true);
        }
    }
}
