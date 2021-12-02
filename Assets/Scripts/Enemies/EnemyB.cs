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
    float FinTimer;
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
        FinTimer = 0.0f;
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
        FinTimer = 4.0f;
        
        if (CurHP / Health > 0.1f)
        {
            if (SummonPhase == 0 && CurHP / Health <= 0.5f)
                Summon();
            else if (SummonPhase == 1 && CurHP / Health <= 0.3f)
                Summon();
            else
            {
                int rand = Random.Range(0, 2);
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
            if (SummonPhase == 2)
                Summon();
            else
            {
                FinTimer = 7.0f;
                FinalAttack();
            }
        }

        Invoke("AbleAttack", FinTimer);
        Invoke("FinishAttacking", 2.25f);
    }

    void Attack1()
    {
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
    }

    void Attack2()
    {
        Anim.SetTrigger("Attack2");
    }

    void Attack3()
    {
        Anim.SetTrigger("Attack3");
    }
    
    void FinalAttack()
    {
        Anim.SetTrigger("Attack4");
    }

    void Summon()
    {
        Anim.SetTrigger("Summon");

        SummonPhase++;
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

        GameManager.Inst().SodManager.PlayEffect("Bs_Normal");
    }

    void Spread()
    {
        switch (WayDir)
        {
            case 0:
                for (int i = 0; i < 5; i++)
                {
                    GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossOneWay");
                    obj.transform.position = SpreadPoses[i * 2].transform.position;
                    obj.transform.rotation = SpreadPoses[i * 2].transform.rotation;

                    BossNormal bullet = obj.gameObject.GetComponent<BossNormal>();
                    bullet.Shoot(SpreadPoses[i].transform.up, 5);
                }
                break;
            case 1:
                for (int i = 6; i < 10; i++)
                {
                    GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossOneWay");
                    obj.transform.position = SpreadPoses[i * 2].transform.position;
                    obj.transform.rotation = SpreadPoses[i * 2].transform.rotation;

                    BossNormal bullet = obj.gameObject.GetComponent<BossNormal>();
                    bullet.Shoot(SpreadPoses[i].transform.up, 5);
                }
                break;
            case 2:
                for (int i = 0; i < 10; i++)
                {
                    GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossOneWay");
                    obj.transform.position = SpreadPoses[i * 2].transform.position;
                    obj.transform.rotation = SpreadPoses[i * 2].transform.rotation;

                    BossNormal bullet = obj.gameObject.GetComponent<BossNormal>();
                    bullet.Shoot(SpreadPoses[i].transform.up, 5);
                }
                break;
        }

        GameManager.Inst().SodManager.PlayEffect("Bs_OneWay");
    }

    void Laser()
    {
        GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossLaser");
        obj.transform.position = LaserPos.transform.position;
        obj.transform.rotation = LaserPos.transform.rotation;

        BossLaser bullet = obj.GetComponent<BossLaser>();
        bullet.StopTime = GameManager.Inst().UpgManager.BData[(int)Type].GetDuration();

        GameManager.Inst().SodManager.PlayEffect("Bs_Laser");
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

                        BossOneWay bullet = obj.gameObject.GetComponent<BossOneWay>();
                        bullet.Shoot(SpreadPoses[index].transform.up);
                    }
                }
                break;
        }

        GameManager.Inst().SodManager.PlayEffect("Bs_OneWay");
    }

    void BigBullet()
    {
        GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossBigBullet");
        obj.transform.position = BigBulletPos.transform.position;
        obj.transform.rotation = BigBulletPos.transform.rotation;

        BossBigBullet bbb = obj.GetComponent<BossBigBullet>();
        bbb.SetHP(Health * 0.05f);
        bbb.SetTargetPos(new Vector3(0.0f, 3.0f, 0.0f));

        GameManager.Inst().SodManager.PlayEffect("Bs_Bigbullet");
    }

    void Bounce()
    {
        int index = 4 + 11 * WayDir;
        switch (WayDir)
        {
            case 0:
            case 1:
                for (int i = -1; i <= 1; i+=2)
                {
                    GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossOneWay");
                    index += i;

                    obj.transform.position = SpreadPoses[index].transform.position;
                    obj.transform.rotation = SpreadPoses[index].transform.rotation;

                    BossBounce bullet = obj.gameObject.GetComponent<BossBounce>();
                    bullet.SetStartPos(obj.transform.position);
                    bullet.Shoot(obj.transform.up);
                }                
                break;

            case 2:
                for (int i = 0; i < 2; i++)
                {
                    GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossOneWay");
                    index = 4 + 11 * i;

                    obj.transform.position = SpreadPoses[index].transform.position;
                    obj.transform.rotation = SpreadPoses[index].transform.rotation;

                    BossBounce bullet = obj.gameObject.GetComponent<BossBounce>();
                    bullet.SetStartPos(obj.transform.position);
                    bullet.Shoot(obj.transform.up);
                }
                break;
        }

        GameManager.Inst().SodManager.PlayEffect("Bs_OneWay");
    }

    void Boomerang()
    {
        GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossNormal");
        obj.transform.position = BigBulletPos.transform.position;
        obj.transform.rotation = BigBulletPos.transform.rotation;

        BossBoomerang bullet = obj.gameObject.GetComponent<BossBoomerang>();
        bullet.StartMove(obj.transform.position);

        GameManager.Inst().SodManager.PlayEffect("Bs_Normal");
    }

    void Missile()
    {
        GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossNormal");
        obj.transform.position = BigBulletPos.transform.position;
        obj.transform.rotation = BigBulletPos.transform.rotation;

        BossMissile bullet = obj.gameObject.GetComponent<BossMissile>();
        bullet.Shoot(BigBulletPos.transform.up);

        GameManager.Inst().SodManager.PlayEffect("Bs_Normal");
    }

    void Explosion()
    {
        GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossNormal");
        obj.transform.position = BigBulletPos.transform.position;
        obj.transform.rotation = BigBulletPos.transform.rotation;

        BossExplosion bullet = obj.gameObject.GetComponent<BossExplosion>();
        bullet.Shoot(obj.transform.up);

        GameManager.Inst().SodManager.PlayEffect("Bs_Normal");
    }

    void Charge()
    {
        GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossLaser");
        obj.transform.position = BigBulletPos.transform.position;
        obj.transform.rotation = BigBulletPos.transform.rotation;

        BossCharge bullet = obj.gameObject.GetComponent<BossCharge>();
        bullet.StartCharge(BigBulletPos);

        GameManager.Inst().SodManager.PlayEffect("Bs_Laser");
    }

    void Slow()
    {
        GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossNormal");
        obj.transform.position = BigBulletPos.transform.position;
        obj.transform.rotation = BigBulletPos.transform.rotation;

        BossSlow bullet = obj.gameObject.GetComponent<BossSlow>();
        bullet.Shoot(BigBulletPos.transform.up, 1);

        GameManager.Inst().SodManager.PlayEffect("Bs_Normal");
    }

    void Mirror()
    {
        GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossLaser");
        obj.transform.position = BigBulletPos.transform.position;
        obj.transform.rotation = BigBulletPos.transform.rotation;

        BossMirror bullet = obj.gameObject.GetComponent<BossMirror>();
        bullet.Rotate();

        GameManager.Inst().SodManager.PlayEffect("Bs_Normal");
    }

    void Shield()
    {
        GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossLaser");
        obj.transform.position = BigBulletPos.transform.position;
        obj.transform.rotation = BigBulletPos.transform.rotation;

        BossShield bullet = obj.gameObject.GetComponent<BossShield>();
        bullet.Fall();

        GameManager.Inst().SodManager.PlayEffect("Bs_Normal");
    }

    void Rock()
    {
        int dir = WayDir;
        if (WayDir == 2)
            dir = Random.Range(0, 2);

        switch (dir)
        {
            case 0:
                GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossOneWay");
                Vector3 newPos = new Vector3(-3.5f, GameManager.Inst().Player.transform.position.y, 0.0f);
                obj.transform.position = newPos;
                obj.transform.rotation = Quaternion.Euler(Vector3.zero);

                BossRock bullet = obj.gameObject.GetComponent<BossRock>();
                bullet.StartCount();
                bullet.Shoot(obj.transform.right, 5.0f);
                break;

            case 1:
                obj = GameManager.Inst().ObjManager.MakeObj("BossOneWay");
                newPos = new Vector3(3.5f, GameManager.Inst().Player.transform.position.y, 0.0f);
                obj.transform.position = newPos;
                obj.transform.rotation = Quaternion.Euler(Vector3.zero);

                bullet = obj.gameObject.GetComponent<BossRock>();
                bullet.StartCount();
                bullet.Shoot(-obj.transform.right, 5.0f);
                break;
        }

        GameManager.Inst().SodManager.PlayEffect("Bs_OneWay");
    }

    void Flower()
    {
        int index = 4 + 11 * WayDir;
        switch (WayDir)
        {
            case 0:
                for (int j = -2; j < 3; j += 2)
                {
                    GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossOneWay");
                    obj.transform.position = SpreadPoses[index + j].transform.position;
                    obj.transform.rotation = SpreadPoses[index + j].transform.rotation;

                    BossFlower bullet = obj.gameObject.GetComponent<BossFlower>();
                    bullet.RotGyesu = 1.0f;
                    bullet.Shoot(obj.transform.up);
                }
                break;

            case 1:
                for (int j = -2; j < 3; j += 2)
                {
                    GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossOneWay");
                    obj.transform.position = SpreadPoses[index + j].transform.position;
                    obj.transform.rotation = SpreadPoses[index + j].transform.rotation;

                    BossFlower bullet = obj.gameObject.GetComponent<BossFlower>();
                    bullet.RotGyesu = -1.0f;
                    bullet.Shoot(obj.transform.up);
                }
                break;

            case 2:
                for (int i = 0; i < 2; i++)
                {
                    for (int j = -2; j < 3; j += 2)
                    {
                        GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossOneWay");
                        index = 4 + 11 * i;

                        obj.transform.position = SpreadPoses[index + j].transform.position;
                        obj.transform.rotation = SpreadPoses[index + j].transform.rotation;

                        BossFlower bullet = obj.gameObject.GetComponent<BossFlower>();
                        bullet.RotGyesu = 1.0f;
                        if (i == 1)
                            bullet.RotGyesu = -1.0f;
                        bullet.Shoot(obj.transform.up);
                    }
                }
                break;
        }

        GameManager.Inst().SodManager.PlayEffect("Bs_Normal");
    }
    
    void Lasers()
    {
        for (int i = 0; i < 3; i++)
        {
            int rand = Random.Range(0, 2);

            GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossLaser");
            BossLasers bullet = obj.GetComponent<BossLasers>();
            bullet.StartLine(rand);
        }
        GameManager.Inst().SodManager.PlayEffect("Bs_Laser");
    }

    void Dot()
    {
        switch (WayDir)
        {
            case 0:
            case 1:
                GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossOneWay");
                obj.transform.position = SpreadPoses[WayDir].transform.position;
                obj.transform.rotation = SpreadPoses[WayDir].transform.rotation;

                BossLaser bullet = obj.GetComponent<BossLaser>();
                bullet.StopTime = 0.0f;
                break;

            case 2:
                for(int i = 0; i < WayDir; i++)
                {
                    obj = GameManager.Inst().ObjManager.MakeObj("BossOneWay");
                    obj.transform.position = SpreadPoses[i].transform.position;
                    obj.transform.rotation = SpreadPoses[i].transform.rotation;

                    bullet = obj.GetComponent<BossLaser>();
                    bullet.StopTime = 0.0f;
                }
                break;
        }

        GameManager.Inst().SodManager.PlayEffect("Bs_Laser");
    }

    void SummonEnemies()
    {
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

        GameManager.Inst().SodManager.PlayEffect("Boss summon");
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

        GameManager.Inst().UiManager.MainUI.BossHPBar.fillAmount = CurHP / Health;
        GameManager.Inst().UiManager.MainUI.BossHPBarText.text = CurHP.ToString() + "/" + Health.ToString();
    }


    public void HitEffect()
    {
        Anim.SetTrigger("Damage");
    }

    void AppearSound()
    {
        GameManager.Inst().SodManager.PlayEffect("Boss appear");
    }

    void DeathSound()
    {
        GameManager.Inst().SodManager.PlayEffect("Boss die");
    }
}
