using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingManager : MonoBehaviour
{
    public int MAXCOLOR = 8;

    GameObject[] NormalPos;
    GameObject[] SpreadPos;
    GameObject LaserPos;
    GameObject ChargePos;

    GameObject[] Objs;
    Color[] Colors;

    int[] ColorSelection;

    public Color GetColors(int index) { return Colors[index]; }
    public int GetColorSelection(int index) { return ColorSelection[index]; }
        
    public void SetColorSelection(int index, int val) { ColorSelection[index] = val; }

    void Awake()
    {
        Objs = new GameObject[Bullet.MAXBULLETS];
        NormalPos = new GameObject[5];
        SpreadPos = new GameObject[5];
        LaserPos = new GameObject();
        ChargePos = new GameObject();

        Colors = new Color[MAXCOLOR];
        ColorSelection = new int[5];
        SetColor();
    }

    void SetColor()
    {
        for (int i = 0; i < MAXCOLOR; i++)
            Colors[i] = GameManager.Inst().UiManager.Colors[i].GetComponent<Image>().color;


        for (int i = 0; i < 5; i++)
            ColorSelection[i] = 0;
    }

    public void Shoot(Bullet.BulletType Type, GameObject Shooter, int ID)
    {
        SetPos(Shooter, ID);

        int power = GameManager.Inst().UpgManager.GetBData((int)Type).GetPowerLevel();
        int colorIndex = ColorSelection[ID];

        switch (Type)
        {
            case Bullet.BulletType.NORMAL:
                Normal(power, colorIndex);
                break;

            case Bullet.BulletType.SPREAD:
                Spread(power, colorIndex);
                break;

            case Bullet.BulletType.MISSILE:
                Missile(power, colorIndex);
                break;

            case Bullet.BulletType.LASER:
                Laser(power, colorIndex);
                break;

            case Bullet.BulletType.CHARGE:
                Charge(power, colorIndex);
                break;

            case Bullet.BulletType.BOOMERANG:
                Boomerang(power, colorIndex);
                break;

            case Bullet.BulletType.CHAIN:
                Chain(power, colorIndex, ID);
                break;
        }
    }

    void SetPos(GameObject Shooter, int ID)
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
            SubWeapon sub;
            if (ID > 1)
                sub = GameManager.Inst().GetSubweapons(ID - 1);
            else
                sub = GameManager.Inst().GetSubweapons(ID);
            
            for (int j = 0; j < 5; j++)
            {
                NormalPos[j] = sub.NormalPos[j];
                SpreadPos[j] = sub.SpreadPos[j];
            }

            LaserPos = sub.LaserPos;
            ChargePos = sub.ChargePos;
        }
    }

    void Normal(int Power, int Index)
    {
        Normal[] bullets = new Normal[5];

        switch (Power)
        {
            case 1:
            case 2:
                Objs[0] = GameManager.Inst().ObjManager.MakeBullet("Normal", Index);
                Objs[0].transform.position = NormalPos[0].transform.position;
                Objs[0].transform.rotation = NormalPos[0].transform.rotation;
                //Objs[0].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", Color);

                Normal bullet = Objs[0].gameObject.GetComponent<Normal>();
                bullet.Shoot(NormalPos[0].transform.up);
                break;

            case 3:
            case 4:
                for(int i = 0; i < 3; i++)
                {
                    Objs[i] = GameManager.Inst().ObjManager.MakeBullet("Normal", Index);
                    Objs[i].transform.position = NormalPos[i].transform.position;
                    Objs[i].transform.rotation = NormalPos[i].transform.rotation;
                    Objs[i].transform.localScale = NormalPos[i].transform.localScale;
                    //Objs[i].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", Color);

                    bullets[i] = Objs[i].gameObject.GetComponent<Normal>();
                    bullets[i].Shoot(NormalPos[0].transform.up);
                }
                break;

            case 5:
                for (int i = 0; i < 5; i++)
                {
                    Objs[i] = GameManager.Inst().ObjManager.MakeBullet("Normal", Index);
                    Objs[i].transform.position = NormalPos[i].transform.position;
                    Objs[i].transform.rotation = NormalPos[i].transform.rotation;
                    Objs[i].transform.localScale = NormalPos[i].transform.localScale;
                    //Objs[i].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", Color);

                    bullets[i] = Objs[i].gameObject.GetComponent<Normal>();
                    bullets[i].Shoot(NormalPos[0].transform.up);
                }
                break;
        }
        
    }

    void Spread(int Power, int Index)
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
                    Objs[i] = GameManager.Inst().ObjManager.MakeBullet("Spread", Index);
                    Objs[i].transform.position = SpreadPos[i].transform.position;
                    Objs[i].transform.rotation = SpreadPos[i].transform.rotation;
                    //Objs[i].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", Color);

                    bullets[i] = Objs[i].gameObject.GetComponent<Spread>();
                    bullets[i].Shoot(SpreadPos[i].transform.up);
                    bullets[i].Invoke("Deactivate", duration);
                }
                break;

            case 4:
            case 5:
                for (int i = 0; i < 5; i++)
                {
                    Objs[i] = GameManager.Inst().ObjManager.MakeBullet("Spread", Index);
                    Objs[i].transform.position = SpreadPos[i].transform.position;
                    Objs[i].transform.rotation = SpreadPos[i].transform.rotation;
                    //Objs[i].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", Color);

                    bullets[i] = Objs[i].gameObject.GetComponent<Spread>();
                    bullets[i].Shoot(SpreadPos[i].transform.up);
                    bullets[i].Invoke("Deactivate", duration);
                }
                break;
        }
    }

    void Missile(int Power, int Index)
    {
        Missile[] bullets = new Missile[5];
        float rad = GameManager.Inst().UpgManager.GetBData((int)Bullet.BulletType.MISSILE).GetDuration();

        switch (Power)
        {
            case 1:
            case 2:
                Objs[0] = GameManager.Inst().ObjManager.MakeBullet("Missile", Index);
                Objs[0].transform.position = SpreadPos[0].transform.position;
                Objs[0].transform.rotation = SpreadPos[0].transform.rotation;
                //Objs[0].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", Color);

                bullets[0] = Objs[0].gameObject.GetComponent<Missile>();
                bullets[0].ResetTarget();
                
                bullets[0].SearchArea.GetComponent<SearchArea>().SetArea(rad);
                bullets[0].Shoot(SpreadPos[0].transform.up);
                break;
            case 3:
            case 4:
                for (int i = 0; i < 2; i++)
                {
                    Objs[i] = GameManager.Inst().ObjManager.MakeBullet("Missile", Index);
                    Objs[i].transform.position = SpreadPos[i + 1].transform.position;
                    Objs[i].transform.rotation = SpreadPos[i + 1].transform.rotation;
                    //Objs[i].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", Color);

                    bullets[i] = Objs[0].gameObject.GetComponent<Missile>();
                    bullets[i].ResetTarget();
                    
                    bullets[i].SearchArea.GetComponent<SearchArea>().SetArea(rad);
                    bullets[i].Shoot(SpreadPos[0].transform.up);
                }
                break;
            case 5:
                for (int i = 0; i < 3; i++)
                {
                    Objs[i] = GameManager.Inst().ObjManager.MakeBullet("Missile", Index);
                    Objs[i].transform.position = SpreadPos[i].transform.position;
                    Objs[i].transform.rotation = SpreadPos[i].transform.rotation;
                    //Objs[i].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", Color);

                    bullets[i] = Objs[i].gameObject.GetComponent<Missile>();
                    bullets[i].ResetTarget();

                    bullets[i].SearchArea.GetComponent<SearchArea>().SetArea(rad);
                    bullets[i].Shoot(SpreadPos[i].transform.up);
                }
                break;
        }
    }

    void Laser(int Power, int Index)
    {
        switch (Power)
        {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                Objs[0] = GameManager.Inst().ObjManager.MakeBullet("Laser", Index);
                Vector3 scale = Objs[0].gameObject.transform.localScale;
                scale.x = (0.5f * Power);
                Objs[0].transform.localScale = scale;
                Objs[0].transform.position = LaserPos.transform.position;
                Objs[0].transform.rotation = LaserPos.transform.rotation;
                //Objs[0].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", Color);

                Laser bullet = Objs[0].gameObject.GetComponent<Laser>();
                break;
        }
    }

    void Charge(int Power, int Index)
    {
        switch (Power)
        {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                Objs[0] = GameManager.Inst().ObjManager.MakeBullet("Charge", Index);
                Objs[0].transform.position = ChargePos.transform.position;
                Objs[0].transform.rotation = ChargePos.transform.rotation;
                //Objs[0].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", Color);

                Charge bullet = Objs[0].gameObject.GetComponent<Charge>();
                bullet.SetChargePos(ChargePos);
                bullet.StartCharge(GameManager.Inst().UpgManager.GetBData((int)Bullet.BulletType.CHARGE).GetDuration());
                break;
        }
    }

    void Boomerang(int Power, int Index)
    {
        switch (Power)
        {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                Objs[0] = GameManager.Inst().ObjManager.MakeBullet("Boomerang", Index);
                Objs[0].transform.position = NormalPos[0].transform.position;
                Objs[0].transform.rotation = NormalPos[0].transform.rotation;
                //Objs[0].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", Color);

                Boomerang bullet = Objs[0].gameObject.GetComponent<Boomerang>();
                bullet.SetStartPos(Objs[0].transform.position);
                bullet.SetTargetpos(Objs[0].transform.position + NormalPos[0].transform.up/*GameManager.Inst().IptManager.MousePosition*/);
                bullet.SetStart();
                break;
        }
    }

    void Chain(int Power, int Index, int ShooterID)
    {
        switch (Power)
        {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                Objs[0] = GameManager.Inst().ObjManager.MakeBullet("Chain", Index);
                Objs[0].transform.position = NormalPos[0].transform.position;
                Objs[0].transform.rotation = NormalPos[0].transform.rotation;
                //Objs[0].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", Color);

                Chain bullet = Objs[0].gameObject.GetComponent<Chain>();
                bullet.ResetData();
                bullet.Shoot(NormalPos[0].transform.up);
                break;
        }
    }
}
