using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFlower : Bullet
{
    public float RotGyesu;

    Rigidbody2D Rig;
    int Damage;


    void Start()
    {
        Rig = GetComponent<Rigidbody2D>();
        RotGyesu = 1.0f;
        Damage = 10 * GameManager.Inst().StgManager.Stage;
    }

    void Update()
    {
        transform.Rotate(0.0f, 0.0f, 2.0f * RotGyesu);

        if (Random.Range(0, 10) == 1)
            RotGyesu *= -1.0f;

        Rig.velocity = transform.up * 4.5f;
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
    }

    void HitEffect(Vector2 pos)
    {
        GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
        hit.transform.position = pos;
    }
}
