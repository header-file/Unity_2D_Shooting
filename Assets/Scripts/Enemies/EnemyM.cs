using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyM : Enemy
{
    void Start()
    {
        Type = EnemyType.MEDIUM;
        Speed = 1;
        BeforeHP = CurHP = Health = 15;
    }
}
