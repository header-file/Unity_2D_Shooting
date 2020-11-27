using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyM : Enemy
{
    void Start()
    {
        Speed = 12;
        BeforeHP = CurHP = Health = 3;
    }

    void FixedUpdate()
    {
        transform.RotateAround(MidPoint, Vector3.forward, -Time.deltaTime * Speed);
    }
}
