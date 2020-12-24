using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal : Bullet
{
    void Awake()
    {
        //Damage = 1.0f;
        Type = BulletType.NORMAL;
        GetComponent<SpriteRenderer>().color = Color.white;
        //Damage *= (float)GameManager.Inst().UpgManager.GetBData((int)Type).GetPowerLevel();
    }

    void Update()
    {
        
    }
}
