using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyS : Enemy
{
    void Start()
    {
        Type = EnemyType.SMALL;
        Speed = 2;
        BeforeHP = CurHP = Health = 6;
    }
}
