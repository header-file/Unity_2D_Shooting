using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : Bullet
{
    public GameObject Sparkle;

    SpriteRenderer SpriteRenderer;
    bool IsAttach;
    Enemy AttachedObj;
    float Timer;
    Color Origin;
    float TickTimer;

    void Awake()
    {
        Type = BulletType.DOT;
        GetComponent<SpriteRenderer>().color = Color.white;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        IsAttach = false;
        Origin = SpriteRenderer.color;
        TickTimer = 0.0f;
    }

    void Update()
    {
        if (IsAttach)
        {
            transform.position = AttachedObj.transform.position;

            TickDmg();
        }
    }

    void TickDmg()
    {
        TickTimer += Time.fixedDeltaTime;
        if (TickTimer < 0.5f)
            return;

        TickTimer = 0.0f;

        Bullet bullet = gameObject.GetComponent<Bullet>();
        if (bullet == null)
            bullet = transform.parent.GetComponent<Bullet>();
        float damage = GameManager.Inst().UpgManager.BData[bullet.GetBulletType()].GetDamage();
        float atk = GameManager.Inst().UpgManager.BData[bullet.GetBulletType()].GetAtk();
        float dmg = damage + atk;
        if (bullet.IsReinforce)
            dmg *= 2;
        bullet.BloodSuck(dmg);

        GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
        hit.transform.position = bullet.transform.position;

        AttachedObj.OnHit(dmg, bullet.IsReinforce);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            if (IsAttach)
                return;

            IsAttach = true;
            AttachedObj = collision.GetComponent<Enemy>();

            SpriteRenderer.color = Color.clear;
            Sparkle.SetActive(true);

            Timer = GameManager.Inst().UpgManager.BData[(int)Type].GetDuration();
            Invoke("Disappear", Timer);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && collision.gameObject == AttachedObj)
            Disappear();
    }

    void Disappear()
    {
        if (!gameObject.activeSelf)
            return;

        Sparkle.SetActive(false);
        IsAttach = false;
        AttachedObj = null;
        SpriteRenderer.color = Origin;
        TickTimer = 0.0f;

        gameObject.SetActive(false);
    }
}
