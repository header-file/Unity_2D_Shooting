using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : Bullet
{
    public GameObject Sparkle;
    public float Speed;
    public SpriteRenderer SpriteRenderer;
    public AnimationCurve Curve;

    Rigidbody2D Rig;
    bool IsAttach;
    Enemy AttachedObj;
    float Timer;
    float TickTimer;
    Bullet SelfBullet;


    void Awake()
    {
        Type = BulletType.DOT;
        Rig = GetComponent<Rigidbody2D>();
        IsAttach = false;
        TickTimer = 0.0f;
        SelfBullet = gameObject.GetComponent<Bullet>();
    }

    void Update()
    {
        if (IsAttach)
        {
            transform.position = AttachedObj.transform.position;

            TickDmg();

            CheckDie();
        }
        else
        {
            Speed = Curve.Evaluate(TickTimer);
            TickTimer += Time.deltaTime;
            if (TickTimer > 0.5f)
                TickTimer = 0.5f;
            //if (Speed < GameManager.Inst().UpgManager.BData[(int)Type].GetSpeed())
            //    Speed = GameManager.Inst().UpgManager.BData[(int)Type].GetSpeed();

            Rig.velocity = transform.up * Speed;
        }
    }

    void TickDmg()
    {
        TickTimer += Time.fixedDeltaTime;
        if (TickTimer < 1.0f)
            return;

        TickTimer = 0.0f;

        if (SelfBullet == null)
            SelfBullet = transform.parent.GetComponent<Bullet>();
        float damage = GameManager.Inst().UpgManager.BData[SelfBullet.GetBulletType()].GetDamage();
        float atk = GameManager.Inst().UpgManager.BData[SelfBullet.GetBulletType()].GetAtk();
        float dmg = damage + atk;
        if (SelfBullet.IsReinforce)
            dmg *= 2;
        SelfBullet.BloodSuck(dmg);

        GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
        hit.transform.position = SelfBullet.transform.position;

        AttachedObj.OnHit(dmg, SelfBullet.IsReinforce, AttachedObj.transform.position);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            if (IsAttach)
                return;

            TickTimer = 0.0f;
            IsAttach = true;
            AttachedObj = collision.GetComponent<Enemy>();

            SpriteRenderer.gameObject.SetActive(false);
            Sparkle.SetActive(true);

            Timer = GameManager.Inst().UpgManager.BData[(int)Type].GetDuration();
            Invoke("Disappear", Timer);
        }
    }

    void CheckDie()
    {
        if (!AttachedObj.gameObject.activeSelf)
            Disappear();
    }

    void Disappear()
    {
        if (!gameObject.activeSelf)
            return;

        Sparkle.SetActive(false);
        IsAttach = false;
        AttachedObj = null;
        TickTimer = 0.0f;
        SpriteRenderer.gameObject.SetActive(true);

        gameObject.SetActive(false);
    }
}
