using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyS : Enemy
{
    void Start()
    {
        Speed = 20;
        BeforeHP = CurHP = Health = 2;
    }
}
