using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllIn1SpriteShader;

public class EnemyB : Enemy
{
    public GameObject[] SpreadPoses;
    public GameObject LaserPos;
    public GameObject BigBulletPos;
    public GameObject[] SummonPoses;
    public Animator Anim;

    bool IsReady;
    bool IsAbleAttack;
    bool IsAttacking;
    Vector3 NowPosition;
    float Gyesu;
    int WayDir;
    int GatlingCount;
    int SummonPhase;


    void Start()
    {
        Type = EnemyType.BOSS;
        Gyesu = 0.1f;
        IsReady = false;
        WayDir = 0;
        GatlingCount = 0;
        SummonPhase = 0;
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
            Vector3 pos = gameObject.transform.position;
            pos.y -= Speed;
            gameObject.transform.position = pos;
            
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
            if (SummonPhase == 0 && CurHP / Health <= 0.5f)
                Summon();
            else if (SummonPhase == 1 && CurHP / Health <= 0.3f)
                Summon();
            else
            {
                int rand = Random.Range(0, 3);
                switch (rand)
                {
                    case 0:
                        Attack1();
                        break;
                    case 1:
                        Attack2();
                        break;
                    case 2:
                        Attack3();
                        break;
                }
            }
        }
        else
        {
            if(SummonPhase == 2)
                Summon();
            else
                FinalAttack();
        }

        Invoke("AbleAttack", 4.0f);
        Invoke("FinishAttacking", 2.25f);
    }

    void Attack1()
    {
        switch(GameManager.Inst().StgManager.Stage)
        {
            case 1:
                WayDir = Random.Range(0, 3);
                switch (WayDir)
                {
                    case 0:
                        Anim.SetTrigger("Attack1R");
                        break;
                    case 1:
                        Anim.SetTrigger("Attack1L");
                        break;
                    case 2:
                        Anim.SetTrigger("Attack1");
                        break;
                }
                break;
            case 2:
                Anim.SetTrigger("Attack1");
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }

    void Attack2()
    {
        switch (GameManager.Inst().StgManager.Stage)
        {
            case 1:
                Anim.SetTrigger("Attack2");
                break;
            case 2:
                Anim.SetTrigger("Attack2");
                break;
            case 3:
                Anim.SetTrigger("Attack2");
                break;
            case 4:
                break;
        }
    }

    void Attack3()
    {
        switch (GameManager.Inst().StgManager.Stage)
        {
            case 1:
                Anim.SetTrigger("Attack3");
                break;
            case 2:
                Anim.SetTrigger("Attack3");
                break;
            case 3:
                Anim.SetTrigger("Attack3");
                break;
            case 4:
                break;
        }
    }
    
    void FinalAttack()
    {
        switch (GameManager.Inst().StgManager.Stage)
        {
            case 1:
                Anim.SetTrigger("Attack3");
                break;
            case 2:
                Anim.SetTrigger("Attack3");
                break;
            case 3:
                Anim.SetTrigger("Attack3");
                break;
            case 4:
                break;
        }
    }

    void Gatling()
    {
        GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossNormal");
        obj.transform.position = BigBulletPos.transform.position;
        obj.transform.rotation = BigBulletPos.transform.rotation;

        BossNormal bullet = obj.gameObject.GetComponent<BossNormal>();
        bullet.Shoot(BigBulletPos.transform.up);

        GatlingCount++;
        if (GatlingCount > 1)
        {
            GatlingCount = 0;
            return;
        }

        Invoke("Gatling", 0.1f);
    }

    void Spread()
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossNormal");
            obj.transform.position = SpreadPoses[i].transform.position;
            obj.transform.rotation = SpreadPoses[i].transform.rotation;
            //obj.GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", new Color(0.5f, 0.5f, 0.5f));

            BossNormal bullet = obj.gameObject.GetComponent<BossNormal>();
            bullet.Shoot(SpreadPoses[i].transform.up);
        }
    }

    void Laser()
    {
        GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossLaser");
        obj.transform.position = LaserPos.transform.position;
        obj.transform.rotation = LaserPos.transform.rotation;
        //obj.GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", new Color(0.5f, 0.5f, 0.5f));
    }

    void ShotOneWay()
    {
        switch(WayDir)
        {
            case 0:
                for(int i = -1; i < 2; i++)
                {
                    int index = 4 + i * 2;

                    GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossOneWay");
                    obj.transform.position = SpreadPoses[index].transform.position;
                    obj.transform.rotation = SpreadPoses[index].transform.rotation;
                    //obj.GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", new Color(0.5f, 0.5f, 0.5f));

                    BossOneWay bullet = obj.gameObject.GetComponent<BossOneWay>();
                    bullet.Shoot(SpreadPoses[index].transform.up);
                }
                break;
            case 1:
                for (int i = -1; i < 2; i++)
                {
                    int index = 15 + i * 2;

                    GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossOneWay");
                    obj.transform.position = SpreadPoses[index].transform.position;
                    obj.transform.rotation = SpreadPoses[index].transform.rotation;
                    //obj.GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", new Color(0.5f, 0.5f, 0.5f));

                    BossOneWay bullet = obj.gameObject.GetComponent<BossOneWay>();
                    bullet.Shoot(SpreadPoses[index].transform.up);
                }
                break;
            case 2:
                for(int j = 0; j < 2; j++)
                {
                    for (int i = -1; i < 2; i++)
                    {
                        int index = 4 + i * 2 + j * 11;

                        GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossOneWay");
                        obj.transform.position = SpreadPoses[index].transform.position;
                        obj.transform.rotation = SpreadPoses[index].transform.rotation;
                        //obj.GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", new Color(0.5f, 0.5f, 0.5f));

                        BossOneWay bullet = obj.gameObject.GetComponent<BossOneWay>();
                        bullet.Shoot(SpreadPoses[index].transform.up);
                    }
                }
                break;
        }
    }

    void BigBullet()
    {
        GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossBigBullet");
        obj.transform.position = BigBulletPos.transform.position;
        obj.transform.rotation = BigBulletPos.transform.rotation;
        //obj.GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", new Color(0.5f, 0.5f, 0.5f));

        BossBigBullet bbb = obj.GetComponent<BossBigBullet>();
        bbb.SetHP(Health * 0.1f);
        bbb.SetTargetPos(new Vector3(0.0f, 3.0f, 0.0f));
    }

    void Bounce()
    {
        int index = 4 + 11 * WayDir;
        switch (WayDir)
        {
            case 0:
                GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossBounce");
                obj.transform.position = SpreadPoses[index].transform.position;
                obj.transform.rotation = SpreadPoses[index].transform.rotation;

                BossBounce bullet = obj.gameObject.GetComponent<BossBounce>();
                bullet.SetStartPos(obj.transform.position);
                bullet.Shoot(obj.transform.up);
                break;

            case 1:
                obj = GameManager.Inst().ObjManager.MakeObj("BossBounce");
                obj.transform.position = SpreadPoses[index].transform.position;
                obj.transform.rotation = SpreadPoses[index].transform.rotation;

                bullet = obj.gameObject.GetComponent<BossBounce>();
                bullet.SetStartPos(obj.transform.position);
                bullet.Shoot(obj.transform.up);
                break;

            case 2:
                for (int i = 0; i < 2; i++)
                {
                    obj = GameManager.Inst().ObjManager.MakeObj("BossBounce");
                    index = 4 + 11 * i;

                    obj.transform.position = SpreadPoses[index].transform.position;
                    obj.transform.rotation = SpreadPoses[index].transform.rotation;

                    //float z = Random.Range(13, 23) * 10.0f;
                    //obj.transform.rotation = Quaternion.Euler(0.0f, 0.0f, z);

                    bullet = obj.gameObject.GetComponent<BossBounce>();
                    bullet.SetStartPos(obj.transform.position);
                    bullet.Shoot(obj.transform.up);
                }
                break;
        }
    }

    void Summon()
    {
        Anim.SetTrigger("Summon");
        //Invoke("SummonEnemies", 0.25f);

        SummonPhase++;
    }

    void SummonEnemies()
    {
        //int rand = Random.Range(0, 5);
        if (GameManager.Inst().StgManager.BossDeathCounts[GameManager.Inst().StgManager.Stage - 1] < 2)
        {
            for (int i = 1; i <= 3; i++)
                GameManager.Inst().StgManager.SpawnSmall(SummonPoses[i].transform.position);
        }
        else if (GameManager.Inst().StgManager.BossDeathCounts[GameManager.Inst().StgManager.Stage - 1] < 4)
        {
            for (int i = 1; i <= 3; i++)
                GameManager.Inst().StgManager.SpawnMedium(SummonPoses[i].transform.position);
        }
        else if (GameManager.Inst().StgManager.BossDeathCounts[GameManager.Inst().StgManager.Stage - 1] < 6)
        {
            GameManager.Inst().StgManager.SpawnLarge(SummonPoses[1].transform.position);
            GameManager.Inst().StgManager.SpawnLarge(SummonPoses[3].transform.position);
        }
        else if (GameManager.Inst().StgManager.BossDeathCounts[GameManager.Inst().StgManager.Stage - 1] < 8)
        {
            for (int i = 1; i <= 3; i++)
                GameManager.Inst().StgManager.SpawnSmall(SummonPoses[i].transform.position);

            GameManager.Inst().StgManager.SpawnMedium(SummonPoses[0].transform.position);
            GameManager.Inst().StgManager.SpawnMedium(SummonPoses[4].transform.position);
        }
        else
        {
            GameManager.Inst().StgManager.SpawnSmall(SummonPoses[1].transform.position);
            GameManager.Inst().StgManager.SpawnSmall(SummonPoses[3].transform.position);

            GameManager.Inst().StgManager.SpawnMedium(SummonPoses[0].transform.position);
            GameManager.Inst().StgManager.SpawnMedium(SummonPoses[4].transform.position);

            GameManager.Inst().StgManager.SpawnLarge(SummonPoses[2].transform.position);
        }
    }

    void AbleAttack()
    {
        IsAbleAttack = true;
    }

    void FinishAttacking()
    {
        IsAttacking = false;
    }

    public void Die()
    {
        Anim.SetTrigger("Die");
        Invoke("Deactivate", 1.0f);
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void ResetData()
    {
        IsReady = false;
        IsAbleAttack = false;
        IsAttacking = false;
        SummonPhase = 0;

        CurHP = Health = (Health + Health * 0.05f * GameManager.Inst().StgManager.BossDeathCounts[GameManager.Inst().StgManager.Stage - 1]);

        GameManager.Inst().UiManager.BossHPBar.fillAmount = CurHP / Health;
        GameManager.Inst().UiManager.BossHPBarText.text = CurHP.ToString() + "/" + Health.ToString();
    }


    public void HitEffect()
    {
        Anim.SetTrigger("Damage");
    }
}
