using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : Bullet
{
    public ChainArea ChainArea;

    GameObject Target;
    Rigidbody2D Rig;
    int DeathCount;
    float Speed;
    float Timer;
    float HitTimer;

    void Awake()
    {
        Type = BulletType.CHAIN;
        GetComponent<SpriteRenderer>().color = Color.white;
        DeathCount = (int)GameManager.Inst().UpgManager.GetBData((int)Type).GetDuration();
        Speed = GameManager.Inst().UpgManager.GetBData((int)Type).GetSpeed();
        Timer = 0.0f;
        HitTimer = 0.0f;
        Rig = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Timer += Time.deltaTime * 0.5f;
        HitTimer -= Time.deltaTime;
        
        if (Target != null)
        {
            //transform.position = Vector3.Lerp(transform.position, Target.transform.position, Timer);
            //transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Timer);
            //Vector2 dir = Target.transform.position - transform.position;
            //Rig.AddForce(dir * Speed, ForceMode2D.Impulse);

            if (!Target.activeSelf)
                Die();
        }
            
    }

    public void HitEnemy()
    {
        DeathCount--;
        HitTimer = 0.05f;

        Rig.velocity = Vector2.zero;
        Rig.angularVelocity = 0.0f;

        if (DeathCount <= 0)
            Die();

        SwitchTarget();
        if (Target == null || !Target.activeSelf)
        {
            Die();
            return;
        }

        Timer = 0.0f;

        Vector2 dir = Vector3.Normalize(Target.transform.position - transform.position);
        Rig.AddForce(dir * Speed, ForceMode2D.Impulse);
        
    }

    void SwitchTarget()
    {
        if (ChainArea.Target == null || Target == ChainArea.Target)
            Target = null;
        else
            Target = ChainArea.Target;

        ChainArea.Target = null;
    }

    public void Die()
    {
        DeathCount = (int)GameManager.Inst().UpgManager.GetBData((int)Type).GetDuration();
        Target = null;
        Timer = 0.0f;
        HitTimer = 0.0f;
        ChainArea.Target = null;
        gameObject.SetActive(false);
    }
}
