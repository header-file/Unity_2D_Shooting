using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : Bullet
{
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
    }

    void Update()
    {
        if (Target != null)
        {
            if (!Target.activeSelf)
                Die();
        }
    }

    public void HitEnemy(GameObject enemy)
    {
        DeathCount--;

        Rig.velocity = Vector2.zero;
        Rig.angularVelocity = 0.0f;

        if (DeathCount <= 0)
        {
            Die();
            return;
        }

        Target = enemy;
        CheckNearestEnemy();
        if (Target == null || !Target.activeSelf)
        {
            Die();
            return;
        }
        
        //Vector2 dir = Vector3.Normalize(Target.transform.position - transform.position);
        //Rig.AddForce(dir * Speed * 3.0f, ForceMode2D.Impulse);
        DrawLine();
    }

    void CheckNearestEnemy()
    {
        GameObject[] enemies;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject closest = null;
        float dist = 3.0f;

        foreach(GameObject e in enemies)
        {
            if (Target == e)
                continue;

            float d = Vector3.Distance(transform.position, e.transform.position);
            if (d < dist)
            {
                dist = d;
                closest = e;
            }
        }

        Target = closest;
    }

    void DrawLine()
    {
        Vector3 v = Target.transform.position - transform.position;
        v = Vector3.Normalize(v);
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        Vector3 mid = (Target.transform.position + transform.position) / 2.0f;
        Vector3 scale = new Vector3(1.0f, 1.0f, 1.0f);
        scale.x = Vector3.Distance(transform.position, Target.transform.position);
        scale.y = 0.75f;
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

   public void Die()
    {
        DeathCount = (int)GameManager.Inst().UpgManager.GetBData((int)Type).GetDuration();
        Target = null;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        gameObject.SetActive(false);
    }

    public void ResetData()
    {
        DeathCount = (int)GameManager.Inst().UpgManager.GetBData((int)Type).GetDuration();
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Target = null;
    }
}
