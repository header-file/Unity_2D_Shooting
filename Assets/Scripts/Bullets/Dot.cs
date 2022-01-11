using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : Bullet
{
    public float Speed;
    public AnimationCurve Curve;
    public CircleCollider2D Col;

    Rigidbody2D Rig;
    bool IsAttach;
    Enemy AttachedObj;
    float Timer;
    float TickTimer;
    Bullet SelfBullet;
    int DmgCount;
    Vector3 AttachPoint;


    void Awake()
    {
        Type = BulletType.DOT;
        Rig = GetComponent<Rigidbody2D>();
        IsAttach = false;
        TickTimer = 0.0f;
        DmgCount = 0;
        SelfBullet = gameObject.GetComponent<Bullet>();
    }

    void Update()
    {
        if (IsAttach)
        {
            transform.localPosition = AttachPoint;

            TickDmg();
        }
        else
        {
            Speed = Curve.Evaluate(TickTimer);
            TickTimer += Time.deltaTime;
            if (TickTimer > 0.5f)
                TickTimer = 0.5f;

            Rig.velocity = transform.up * Speed;
        }
    }

    void TickDmg()
    {
        TickTimer += Time.fixedDeltaTime;
        if (TickTimer < 0.5f)
            return;

        TickTimer = 0.0f;
        if (DmgCount <= 0)
        {
            Die();
            return;
        }

        if (SelfBullet == null)
            SelfBullet = transform.parent.GetComponent<Bullet>();
        float damage = GameManager.Inst().UpgManager.BData[SelfBullet.GetBulletType()].GetDamage();
        float atk = GameManager.Inst().UpgManager.BData[SelfBullet.GetBulletType()].GetAtk();
        float dmg = damage + atk;
        if (SelfBullet.IsReinforce)
            dmg *= 2;
        SelfBullet.BloodSuck(dmg);

        GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
        //hit.transform.position = Col.ClosestPoint(AttachedObj.transform.position);
        hit.transform.position = AttachedObj.transform.position + AttachPoint;

        AttachedObj.OnHit(dmg, SelfBullet.IsReinforce, hit.transform.position);
        DmgCount--;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            if (IsAttach)
                return;

            AttachedObj = collision.GetComponent<Enemy>();
            if (AttachedObj != null && AttachedObj.IsDot)
            {
                Die();
                return;
            }

            TickTimer = 0.5f;
            IsAttach = true;
            AttachedObj.IsDot = true;
            Vector2 hitPoint = collision.ClosestPoint(transform.position);
            transform.SetParent(AttachedObj.gameObject.transform);
            AttachPoint = transform.localPosition;
            Rig.velocity = Vector3.zero;

            //Timer = GameManager.Inst().UpgManager.BData[(int)Type].GetDuration();
            //DmgCount = (int)(Timer / 0.5f);
            DmgCount = (int)GameManager.Inst().UpgManager.BData[(int)Type].GetDuration();
        }
        else if (collision.tag == "Border")
        {
            if (!IsAttach)
                Die();
        }            
    }

    public void Die()
    {
        IsAttach = false;
        transform.SetParent(GameManager.Inst().ObjManager.PBulletPool.transform);
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        if (AttachedObj != null)
            AttachedObj.IsDot = false;
        AttachedObj = null;
        TickTimer = 0.0f;
    }
}
