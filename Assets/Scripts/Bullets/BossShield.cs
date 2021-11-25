using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShield : Bullet
{
    Rigidbody2D Rig;
    int Damage;

    void Start()
    {
        Type = BulletType.NORMAL;
        Damage = 10 * GameManager.Inst().StgManager.Stage;
        Rig = GetComponent<Rigidbody2D>();
    }

    public void Fall()
    {
        Invoke("Falldown", 0.75f);
    }

    void Falldown()
    {
        Shoot(transform.up, 1.0f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BlockBullet" ||
            collision.gameObject.tag == "PierceBullet" ||
            collision.gameObject.tag == "Chain")
        {
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "SubWeapon")
        {
            HitEffect(collision.gameObject);
            collision.gameObject.GetComponent<SubWeapon>().Damage(Damage);

            gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Player")
        {
            HitEffect(collision.gameObject);
            collision.gameObject.GetComponent<Player>().Damage(Damage);

            gameObject.SetActive(false);
        }
    }

    void HitEffect(GameObject obj)
    {
        GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
        hit.transform.position = obj.transform.position;
    }
}
