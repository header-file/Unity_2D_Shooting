using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Bullet
{
    void Awake()
    {
        Type = BulletType.EXPLOSION;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            GameObject explode = GameManager.Inst().ObjManager.MakeObj("Explosive");
            Explosive explosive = explode.GetComponent<Explosive>();
            explosive.SetData(gameObject.GetComponent<Bullet>());
            explode.transform.position = transform.position;
        }
    }
}
