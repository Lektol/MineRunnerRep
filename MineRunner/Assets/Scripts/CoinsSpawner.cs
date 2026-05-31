using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsSpawner : MonoBehaviour
{
    [SerializeField] private Coin[] Coins;
    [Range(0f,1f)]
    [SerializeField] private float chanceToSpawn = 0.5f;

    void Start()
    {
        float chance = UnityEngine.Random.Range(0f, 1f);
        if(chanceToSpawn >= chance)
        {
            foreach(var coin in Coins)
            {
                coin.gameObject.SetActive(true);
            }
        }
    }
}
