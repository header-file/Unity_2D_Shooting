using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOneWay : Bullet
{
    int Damage;

    void Awake()
    {
        Type = BulletType.NORMAL;
        Damage = 10;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SubWeapon")
        {
            collision.gameObject.GetComponent<SubWeapon>().Damage(Damage);
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Player")
        {
            GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
            hit.transform.position = collision.gameObject.transform.position;

            gameObject.SetActive(false);
        }
    }
}
