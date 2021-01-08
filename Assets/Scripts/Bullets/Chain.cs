using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : Bullet
{
    public ChainArea ChainArea;

    GameObject Target;
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
        HitTimer = 0.1f;
    }

    void Update()
    {
        Timer += Time.deltaTime * Speed;
        HitTimer -= Time.deltaTime;
        
        {
            //transform.position = Vector3.Lerp(transform.position, new Vector3(0.0f, 10.0f, 0.0f), Timer);
            //SwitchTarget();
        }
        if (Target != null)
        {
            
            transform.position = Vector3.Lerp(transform.position, Target.transform.position, Timer);

            if (!Target.activeSelf)
                gameObject.SetActive(false);
        }
            
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            if (HitTimer < 0.0f)
            {
                DeathCount--;
                HitTimer = 0.1f;
                if (DeathCount <= 0)
                {
                    DeathCount = (int)GameManager.Inst().UpgManager.GetBData((int)Type).GetDuration();
                    Target = null;
                    gameObject.SetActive(false);
                }
            }

            SwitchTarget();
            Timer = 0.0f;
        }
    }

    void SwitchTarget()
    {
        if (ChainArea.Target != null)
            Target = ChainArea.Target;
    }
}
