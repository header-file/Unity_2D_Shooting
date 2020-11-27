using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyL : Enemy
{
    void Start()
    {
        Speed = 6;
        BeforeHP = CurHP = Health = 5;
    }
}
