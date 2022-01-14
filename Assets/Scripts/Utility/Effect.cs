using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    ParticleSystem PS;

    void Awake()
    {
        PS = GetComponent<ParticleSystem>();
    }

    void Start()
    {
        //if (gameObject.tag == "ForTest")
        //    Debug.Log("Eff Start" + transform.position);
    }

    void Update()
    {
        if (PS.isStopped)
        {
            //if (gameObject.tag == "ForTest")
            //    Debug.Log("Eff End" + transform.position);
            gameObject.SetActive(false);
        }
    }
}
