using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : Bullet
{
    public ChainArea ChainArea;
    public GameObject LinePref;

    GameObject Target;
    Rigidbody2D Rig;
    int DeathCount;
    float Speed;

    void Awake()
    {
        Type = BulletType.CHAIN;
        GetComponent<SpriteRenderer>().color = Color.white;
        DeathCount = (int)GameManager.Inst().UpgManager.GetBData((int)Type).GetDuration();
        Speed = GameManager.Inst().UpgManager.GetBData((int)Type).GetSpeed();
        Rig = gameObject.GetComponent<Rigidbody2D>();
        ChainArea.MaxEnemy = DeathCount;
    }

    void Update()
    {
        if (Target != null)
        {
            if (!Target.activeSelf)
                Die();
        }
    }

    public void HitEnemy()
    {
        DeathCount--;

        Rig.velocity = Vector2.zero;
        Rig.angularVelocity = 0.0f;

        if (DeathCount < 0)
            Die();

        SwitchTarget();
        if (Target == null || !Target.activeSelf)
        {
            Die();
            return;
        }
        
        //Vector2 dir = Vector3.Normalize(Target.transform.position - transform.position);
        //Rig.AddForce(dir * Speed * 3.0f, ForceMode2D.Impulse);
        DrawLine();
    }

    void DrawLine()
    {
        Vector3 v = Target.transform.position - transform.position;
        v = Vector3.Normalize(v);
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        Vector3 mid = (Target.transform.position + transform.position) / 2.0f;
        Vector3 scale = new Vector3(1.0f, 1.0f, 1.0f);
        scale.x = Vector3.Distance(transform.position, Target.transform.position);
        scale.y = 0.5f;
        Color glowColor = gameObject.GetComponent<SpriteRenderer>().material.GetColor("_GlowColor");
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        GameObject line = Instantiate(LinePref);
        line.transform.position = mid;
        line.transform.right = v;
        line.transform.localScale = scale;
        line.GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", glowColor);
        line.GetComponent<ActivationTimer>().IsStart = true;

        transform.position = Target.transform.position;
    }

    //void SwitchTarget()
    //{
    //    if (ChainArea.Target == null || Target == ChainArea.Target)
    //        Target = null;
    //    else
    //        Target = ChainArea.Target;

    //    ChainArea.Target = null;
    //}

    void SwitchTarget()
    {
        if (ChainArea.Targets[3 - DeathCount] == null || Target == ChainArea.Targets[3 - DeathCount])
            Target = null;
        else
            Target = ChainArea.Targets[3 - DeathCount];
    }

    public void Die()
    {
        DeathCount = (int)GameManager.Inst().UpgManager.GetBData((int)Type).GetDuration();
        Target = null;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        //ChainArea.Target = null;
        ChainArea.ResetTargets();
        gameObject.SetActive(false);
    }
}
