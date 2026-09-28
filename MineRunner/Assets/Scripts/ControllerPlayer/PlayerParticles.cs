using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    [SerializeField] private GameObject _particleLanding;

    public void OnParticleLanding()
    {
        _particleLanding.SetActive(true);
    }
}
