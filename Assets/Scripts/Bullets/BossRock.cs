using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRock : Bullet
{
    int Damage;
    bool IsDisappear;


    void Awake()
    {
        Type = BulletType.NORMAL;
        Damage = 10 * GameManager.Inst().StgManager.Stage;
        IsDisappear = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SubWeapon")
        {
            HitEffect(collision.ClosestPoint(transform.position));
            collision.gameObject.GetComponent<SubWeapon>().Damage(Damage);
            IsDisappear = false;

            gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Player")
        {
            HitEffect(collision.ClosestPoint(transform.position));
            collision.gameObject.GetComponent<Player>().Damage(Damage);
            IsDisappear = false;

            gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Border")
            if(IsDisappear)
            {
                IsDisappear = false;
                gameObject.SetActive(false);
            }
    }

    void HitEffect(Vector2 pos)
    {
        GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
        hit.transform.position = pos;
    }

    public void StartCount()
    {
        Invoke("CanDisappear", 0.1f);
    }

    void CanDisappear()
    {
        IsDisappear = true;
    }
}
