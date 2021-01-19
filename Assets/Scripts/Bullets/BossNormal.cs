using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNormal : Bullet
{
    public int Damage;

    void Awake()
    {
        Type = BulletType.NORMAL;
        Damage = 10;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "SubWeapon")
        {
            collision.gameObject.GetComponent<SubWeapon>().Damage(Damage);
            gameObject.SetActive(false);
        }
        else if(collision.gameObject.tag == "Player")
            gameObject.SetActive(false);
    }
}
