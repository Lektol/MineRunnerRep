using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _crystalParent;
    [Range(0f,1f)]
    [SerializeField] private float _chanceToSpawn = 0.5f;

    private void Start()
    {
        float chance = UnityEngine.Random.Range(0f, 1f);
        if(_chanceToSpawn >= chance)
        {
            _crystalParent.SetActive(true);
            SetActiveCrystals();
        }
    }

    private void OnDisable()
    {
        _crystalParent.SetActive(false);
    }

    /// <summary>
    /// Чтобы переиспользовать чанк, нужно при его создании снова каждый кристал включить, так как его мог собрать игрок и отключить
    /// </summary>
    private void SetActiveCrystals()
    {
        foreach (Transform item in _crystalParent.transform)
        {
            item.gameObject.SetActive(true);
        }
    }
}
