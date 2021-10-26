using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFlower : Bullet
{
    Rigidbody2D Rig;
    float RotGyesu;


    void Start()
    {
        Rig = GetComponent<Rigidbody2D>();
        RotGyesu = 1.0f;
        Shoot(-transform.up, 1.0f);
    }

    void Update()
    {
        if (transform.rotation.eulerAngles.z > 60.0f &&
                    transform.rotation.eulerAngles.z < 300.0f)
            RotGyesu *= -1.0f;

        transform.Rotate(0.0f, 0.0f, 1.5f * RotGyesu);

        Rig.velocity = -transform.up * 2.5f;
    }
}
