using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyB : Enemy
{
    public GameObject[] SpreadPoses;
    public GameObject[] Arms;
    public GameObject LaserPos;
    public GameObject BigBulletPos;
    public Animator Anim;

    bool IsReady;
    bool IsAbleAttack;
    bool IsAttacking;
    Vector3 NowPosition;
    float Gyesu;
    int OneWayCount;
    int WayDir;

    void Start()
    {
        Type = EnemyType.BOSS;
        Gyesu = 0.1f;
        IsReady = false;
        OneWayCount = 0;
        WayDir = 0;
    }

    void FixedUpdate()
    {
        if (IsAbleAttack || IsAttacking)
            return;

        if(IsReady)
        {
            Vector3 pos = transform.position;
            pos.x += Speed * Gyesu;
            transform.position = pos;

            if (transform.position.x >= 1.0f || transform.position.x <= -1.0f)
                Gyesu *= -1.0f;
        }
        else
        {
            Vector3 pos = transform.position;
            pos.y -= Speed;
            transform.position = pos;

            if (transform.position.y < 8.9f)
            {
                Invoke("AbleAttack", 1.0f);
                IsReady = true;
            }
        }
    }

    void Update()
    {
        if (!IsAbleAttack)
            return;

        IsAbleAttack = false;
        IsAttacking = true;
        
        if (CurHP / Health > 0.1f)
        {
            int rand = Random.Range(0, 3);
            switch (rand)
            {
                case 0:
                    Anim.SetTrigger("Attack1");
                    Invoke("Spread", 0.38f);
                    //Spread();
                    break;
                case 1:
                    Anim.SetTrigger("Attack2");
                    Invoke("Laser", 0.34f);
                    //Laser();
                    break;
                case 2:
                    WayDir = Random.Range(0, 3);
                    ShotOneWay();
                    break;
                case 3:
                    //BigBullet();
                    break;
            }
        }
        else
        {
            Anim.SetTrigger("Attack3");
            Invoke("BigBullet", 0.9f);
            //BigBullet();
        }

        Invoke("AbleAttack", 4.0f);
        Invoke("FinishAttacking", 2.0f);
    }

    void Spread()
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossNormal");
            obj.transform.position = SpreadPoses[i].transform.position;
            obj.transform.rotation = SpreadPoses[i].transform.rotation;
            obj.GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", new Color(0.5f, 0.5f, 0.5f));

            BossNormal bullet = obj.gameObject.GetComponent<BossNormal>();
            bullet.Shoot(SpreadPoses[i].transform.up);
        }
    }

    void Laser()
    {
        GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossLaser");
        obj.transform.position = LaserPos.transform.position;
        obj.transform.rotation = LaserPos.transform.rotation;
        obj.GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", new Color(0.5f, 0.5f, 0.5f));
    }

    void ShotOneWay()
    {
        int index = 4 + 11 * WayDir;
        if(index > 20)
        {
            for(int i = 1; i <= 2; i++)
            {
                GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossOneWay");
                obj.transform.position = SpreadPoses[index - 11 * i].transform.position;
                obj.transform.rotation = SpreadPoses[index - 11 * i].transform.rotation;
                obj.GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", new Color(0.5f, 0.5f, 0.5f));

                BossOneWay bullet = obj.gameObject.GetComponent<BossOneWay>();
                bullet.Shoot(SpreadPoses[index - 11 * i].transform.up);
            }
        }
        else
        {
            GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossOneWay");
            obj.transform.position = SpreadPoses[index].transform.position;
            obj.transform.rotation = SpreadPoses[index].transform.rotation;
            obj.GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", new Color(0.5f, 0.5f, 0.5f));

            BossOneWay bullet = obj.gameObject.GetComponent<BossOneWay>();
            bullet.Shoot(SpreadPoses[index].transform.up);
        }

        OneWayCount++;
        if (OneWayCount < 3)
            Invoke("ShotOneWay", 0.1f);
        else
            OneWayCount = 0;
    }

    void BigBullet()
    {
        GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossBigBullet");
        obj.transform.position = BigBulletPos.transform.position;
        obj.transform.rotation = BigBulletPos.transform.rotation;
        obj.GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", new Color(0.5f, 0.5f, 0.5f));

        BossBigBullet bbb = obj.GetComponent<BossBigBullet>();
        bbb.SetHP(Health * 0.1f);
        bbb.SetTargetPos(new Vector3(0.0f, 3.0f, 0.0f));
    }

    void LastSpurt()
    {
        Debug.Log("LastSpurt");
    }

    void AbleAttack()
    {
        IsAbleAttack = true;
    }

    void FinishAttacking()
    {
        IsAttacking = false;
    }
}
