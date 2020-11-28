using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyL : Enemy
{
    void Start()
    {
        Type = EnemyType.LARGE;
        Speed = 0.5f;
        BeforeHP = CurHP = Health = 10;
    }
}
