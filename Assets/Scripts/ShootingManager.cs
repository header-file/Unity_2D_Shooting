using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    GameObject[] NormalPos;
    GameObject[] SpreadPos;
    GameObject LaserPos;
    GameObject ChargePos;

    GameObject[] Objs;
    

    void Awake()
    {
        Objs = new GameObject[5];
        NormalPos = new GameObject[5];
        SpreadPos = new GameObject[5];
        LaserPos = new GameObject();
        ChargePos = new GameObject();
    }

    public void Shoot(Bullet.BulletType Type, GameObject Shooter)
    {
        SetPos(Shooter);

        int power = GameManager.Inst().UpgManager.GetBData((int)Type).GetPowerLevel();
        switch(Type)
        {
            case Bullet.BulletType.NORMAL:
                Normal(power);
                break;

            case Bullet.BulletType.SPREAD:
                Spread(power);
                break;

            case Bullet.BulletType.MISSILE:
                Missile(power);
                break;

            case Bullet.BulletType.LASER:
                Laser(power);
                break;

            case Bullet.BulletType.CHARGE:
                Charge(power);
                break;
        }
    }

    void SetPos(GameObject Shooter)
    {
        if(Shooter.gameObject.tag == "Player")
        {
            Player player = GameManager.Inst().Player;
            for(int i = 0; i < 5; i++)
            {
                NormalPos[i] = player.NormalPos[i];
                SpreadPos[i] = player.SpreadPos[i];
            }

            LaserPos = player.LaserPos;
            ChargePos = player.ChargePos;
        }
        else if (Shooter.gameObject.tag == "SubWeapon")
        {
            for(int i = 0; i < 4; i++)
            {
                SubWeapon sub = GameManager.Inst().GetSubweapons(i);
                if (Shooter.gameObject.GetInstanceID() == GameManager.Inst().SubWID[i])
                {
                    for (int j = 0; j < 5; j++)
                    {
                        NormalPos[j] = sub.NormalPos[j];
                        SpreadPos[j] = sub.SpreadPos[j];
                    }

                    LaserPos = sub.LaserPos;
                    ChargePos = sub.ChargePos;
                }
            }
        }
    }

    void Normal(int Power)
    {
        Normal[] bullets = new Normal[5];

        switch (Power)
        {
            case 1:
            case 2:
                Objs[0] = GameManager.Inst().ObjManager.MakeObj("Normal");
                Objs[0].transform.position = NormalPos[0].transform.position;
                Objs[0].transform.rotation = NormalPos[0].transform.rotation;

                Normal bullet = Objs[0].gameObject.GetComponent<Normal>();
                bullet.Shoot(NormalPos[0].transform.up);
                break;

            case 3:
            case 4:
                for(int i = 0; i < 3; i++)
                {
                    Objs[i] = GameManager.Inst().ObjManager.MakeObj("Normal");
                    Objs[i].transform.position = NormalPos[i].transform.position;
                    Objs[i].transform.rotation = NormalPos[i].transform.rotation;
                    Objs[i].transform.localScale = NormalPos[i].transform.localScale;

                    bullets[i] = Objs[i].gameObject.GetComponent<Normal>();
                    bullets[i].Shoot(NormalPos[0].transform.up);
                }
                break;

            case 5:
                for (int i = 0; i < 5; i++)
                {
                    Objs[i] = GameManager.Inst().ObjManager.MakeObj("Normal");
                    Objs[i].transform.position = NormalPos[i].transform.position;
                    Objs[i].transform.rotation = NormalPos[i].transform.rotation;
                    Objs[i].transform.localScale = NormalPos[i].transform.localScale;

                    bullets[i] = Objs[i].gameObject.GetComponent<Normal>();
                    bullets[i].Shoot(NormalPos[0].transform.up);
                }
                break;
        }
        
    }

    void Spread(int Power)
    {
        Spread[] bullets = new Spread[5];
        float duration = GameManager.Inst().UpgManager.GetBData((int)Bullet.BulletType.SPREAD).GetDuration();

        switch (Power)
        {
            case 1:
            case 2:
            case 3:
                for(int i = 0; i < 3; i++)
                {
                    Objs[i] = GameManager.Inst().ObjManager.MakeObj("Spread");
                    Objs[i].transform.position = SpreadPos[i].transform.position;
                    Objs[i].transform.rotation = SpreadPos[i].transform.rotation;

                    bullets[i] = Objs[i].gameObject.GetComponent<Spread>();
                    bullets[i].Shoot(SpreadPos[i].transform.up);
                    bullets[i].Invoke("Deactivate", duration);
                }
                break;

            case 4:
            case 5:
                for (int i = 0; i < 5; i++)
                {
                    Objs[i] = GameManager.Inst().ObjManager.MakeObj("Spread");
                    Objs[i].transform.position = SpreadPos[i].transform.position;
                    Objs[i].transform.rotation = SpreadPos[i].transform.rotation;

                    bullets[i] = Objs[i].gameObject.GetComponent<Spread>();
                    bullets[i].Shoot(SpreadPos[i].transform.up);
                    bullets[i].Invoke("Deactivate", duration);
                }
                break;
        }
    }

    void Missile(int Power)
    {
        Missile[] bullets = new Missile[5];
        float rad = GameManager.Inst().UpgManager.GetBData((int)Bullet.BulletType.MISSILE).GetDuration();

        switch (Power)
        {
            case 1:
            case 2:
                Objs[0] = GameManager.Inst().ObjManager.MakeObj("Missile");
                Objs[0].transform.position = SpreadPos[0].transform.position;
                Objs[0].transform.rotation = SpreadPos[0].transform.rotation;

                bullets[0] = Objs[0].gameObject.GetComponent<Missile>();
                bullets[0].ResetTarget();
                
                bullets[0].SearchArea.GetComponent<SearchArea>().SetArea(rad);
                bullets[0].Shoot(SpreadPos[0].transform.up);
                break;
            case 3:
            case 4:
                for (int i = 0; i < 2; i++)
                {
                    Objs[i] = GameManager.Inst().ObjManager.MakeObj("Missile");
                    Objs[i].transform.position = SpreadPos[i + 1].transform.position;
                    Objs[i].transform.rotation = SpreadPos[i + 1].transform.rotation;

                    bullets[i] = Objs[0].gameObject.GetComponent<Missile>();
                    bullets[i].ResetTarget();
                    
                    bullets[i].SearchArea.GetComponent<SearchArea>().SetArea(rad);
                    bullets[i].Shoot(SpreadPos[0].transform.up);
                }
                break;
            case 5:
                for (int i = 0; i < 3; i++)
                {
                    Objs[i] = GameManager.Inst().ObjManager.MakeObj("Missile");
                    Objs[i].transform.position = SpreadPos[i].transform.position;
                    Objs[i].transform.rotation = SpreadPos[i].transform.rotation;

                    bullets[i] = Objs[i].gameObject.GetComponent<Missile>();
                    bullets[i].ResetTarget();

                    bullets[i].SearchArea.GetComponent<SearchArea>().SetArea(rad);
                    bullets[i].Shoot(SpreadPos[i].transform.up);
                }
                break;
        }
    }

    void Laser(int Power)
    {
        switch (Power)
        {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                Objs[0] = GameManager.Inst().ObjManager.MakeObj("Laser");
                Vector3 scale = Objs[0].gameObject.transform.localScale;
                scale.x = (0.5f * Power);
                Objs[0].transform.localScale = scale;
                Objs[0].transform.position = LaserPos.transform.position;
                Objs[0].transform.rotation = LaserPos.transform.rotation;

                Laser bullet = Objs[0].gameObject.GetComponent<Laser>();
                break;
        }
    }

    void Charge(int Power)
    {
        switch (Power)
        {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                Objs[0] = GameManager.Inst().ObjManager.MakeObj("Charge");
                Objs[0].transform.position = LaserPos.transform.position;
                Objs[0].transform.rotation = LaserPos.transform.rotation;

                Charge bullet = Objs[0].gameObject.GetComponent<Charge>();
                bullet.StartCharge(GameManager.Inst().UpgManager.GetBData((int)Bullet.BulletType.CHARGE).GetDuration());
                break;
        }
    }
}
