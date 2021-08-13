using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossExplosion : Bullet
{
    public BossExplode Explosion;

    float Count;


    void Awake()
    {
        Type = BulletType.EXPLOSION;
        Explosion.Damage = 10 * GameManager.Inst().StgManager.Stage;
        Explosion.gameObject.SetActive(false);
    }

    void Update()
    {
        Count += Time.deltaTime;

        if (Count >= 2.0f)
            Explode();
    }

    void Explode()
    {
        if (Explosion.gameObject.activeSelf)
            return;

        Explosion.gameObject.SetActive(true);
        Invoke("Disappear", 0.15f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SubWeapon" ||
            collision.gameObject.tag == "Player")
            Explode();
    }

    public void Disappear()
    {
        Count = 0.0f;
        Explosion.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
