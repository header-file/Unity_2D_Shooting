using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : Bullet
{
    void Awake()
    {
        Damage = 1.0f;
        Type = BulletType.SPLIT;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    void Update()
    {
        
    }
}
