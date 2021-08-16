using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : Bullet
{
    public int Damage;
    public float StopTime;

    Animator Anim; 
    bool IsStopped;

    void Awake()
    {
        Type = BulletType.LASER;
        GetComponent<SpriteRenderer>().color = Color.white;
        Damage = 20 * GameManager.Inst().StgManager.Stage;
        IsStopped = false;
    }

    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!IsStopped && Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
        {
            Anim.speed = 0.0f;
            IsStopped = true;

            Invoke("Restart", StopTime);
        }

        if (Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            Destroy();
    }

    void Restart()
    {
        Anim.speed = 1.0f;
    }

    void Destroy()
    {
        gameObject.SetActive(false);
        IsStopped = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SubWeapon")
        {
            HitEffect(collision.gameObject);
            collision.gameObject.GetComponent<SubWeapon>().Damage(Damage);
        }
        else if (collision.gameObject.tag == "Player")
        {
            HitEffect(collision.gameObject);
            collision.gameObject.GetComponent<Player>().Damage(Damage);
        }
    }

    void HitEffect(GameObject obj)
    {
        GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
        hit.transform.position = obj.transform.position;
    }
}
