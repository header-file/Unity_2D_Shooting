using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBounce : Bullet
{
    public Rigidbody2D Rig;
    public int Damage;

    Vector3 StartPos;
    int BounceCount;

    public void SetStartPos(Vector3 pos) { StartPos = pos; }

    void Start()
    {
        Type = BulletType.MISSILE;
        Rig = gameObject.GetComponent<Rigidbody2D>();
        Damage = 10 * GameManager.Inst().StgManager.Stage;
        BounceCount = 3;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Border")
        {
            BounceCount--;

            if(BounceCount >= 0)
            {
                Rig.velocity = Vector2.zero;
                Rig.angularVelocity = 0.0f;

                Vector3 dir = gameObject.transform.position - StartPos;
                Shoot(Vector3.Reflect(dir, -collision.gameObject.transform.up).normalized);
                StartPos = gameObject.transform.position;
            }
            else
            {
                BounceCount = 3;
                Rig.velocity = Vector2.zero;
                Rig.angularVelocity = 0.0f;
                gameObject.SetActive(false);
            }
        }
        else if (collision.gameObject.tag == "SubWeapon")
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
