using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Bullet
{
    public GameObject Explode;
    public GameObject Bullet;

    void Awake()
    {
        Type = BulletType.EXPLOSION;
        Bullet.GetComponent<SpriteRenderer>().color = Color.white;
        Explode.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            Explode.SetActive(true);
            Bullet.SetActive(false);
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            Invoke("Disappear", 0.5f);
        }
    }

    public void Disappear()
    {
        Explode.SetActive(false);
        Bullet.SetActive(true);
        gameObject.SetActive(false);
    }
}
