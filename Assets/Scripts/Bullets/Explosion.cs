using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Bullet
{
    public GameObject Explode;

    void Awake()
    {
        Type = BulletType.EXPLOSION;
        GetComponent<SpriteRenderer>().color = Color.white;
        Explode.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            //for (int i = 0; i < ExplodeArea.HitObjects.Length; i++)
            //{
            //    Enemy enemy = ExplodeArea.HitObjects[i].GetComponent<Enemy>();
            //    if (enemy)
            //    {

            //        enemy.OnHit(, false)
            //    }
            //}

            Explode.SetActive(true);
            Invoke("Disappear", 0.15f);
        }
    }

    public void Disappear()
    {
        Explode.SetActive(false);
        gameObject.SetActive(false);
    }
}
