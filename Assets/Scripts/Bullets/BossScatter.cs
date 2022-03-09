using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScatter : Bullet
{
    public float RotSpeed;

    Rigidbody2D Rig;
    int Damage;


    void Awake()
    {
        Rig = GetComponent<Rigidbody2D>();
        Type = BulletType.NORMAL;
        Damage = 10 * GameManager.Inst().StgManager.Stage;
        RotSpeed = 0.0f;
    }

    void Update()
    {
        transform.Rotate(0.0f, 0.0f, RotSpeed);
        Rig.velocity = transform.up * 6.0f;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SubWeapon")
        {
            HitEffect(collision.ClosestPoint(transform.position));
            collision.gameObject.GetComponent<SubWeapon>().Damage(Damage);

            gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Player")
        {
            HitEffect(collision.ClosestPoint(transform.position));
            collision.gameObject.GetComponent<Player>().Damage(Damage);

            gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Border")
            gameObject.SetActive(false);
    }

    void HitEffect(Vector2 pos)
    {
        GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
        hit.transform.position = pos;
    }
}
