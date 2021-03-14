using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    ParticleSystem PS_Explosion;

    void Awake()
    {
        PS_Explosion = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (PS_Explosion.isStopped)
            gameObject.SetActive(false);
    }
}
