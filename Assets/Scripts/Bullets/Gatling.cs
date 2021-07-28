using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatling : Bullet
{
    void Awake()
    {
        Type = BulletType.GATLING;
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
