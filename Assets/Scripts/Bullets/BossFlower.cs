using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFlower : Bullet
{
    public float RotGyesu;

    Rigidbody2D Rig;


    void Start()
    {
        Rig = GetComponent<Rigidbody2D>();
        RotGyesu = 1.0f;
    }

    void Update()
    {
        //if ((transform.rotation.eulerAngles.z > 60.0f && transform.rotation.eulerAngles.z < 120.0f) || 
        //            (transform.rotation.eulerAngles.z < -180.0f && transform.rotation.eulerAngles.z > -240.0f))
        //    RotGyesu *= -1.0f;

        transform.Rotate(0.0f, 0.0f, 1.0f * RotGyesu);

        if (Random.Range(0, 10) == 1)
            RotGyesu *= -1.0f;

        Rig.velocity = transform.up * 4.5f;
    }
}
