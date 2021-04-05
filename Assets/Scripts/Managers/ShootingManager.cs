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
        GameManager.Inst().ShtManager = gameObject.GetComponent<ShootingManager>();

        Objs = new GameObject[10];
        NormalPos = new GameObject[5];
        SpreadPos = new GameObject[7];
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

        int rarity = GameManager.Inst().UpgManager.BData[(int)Type].GetRarity();

        int colorIndex = ColorSelection[ID];
        if (Shooter.tag == "SubWeapon" && ID > 1)
            colorIndex = ColorSelection[ID + 1];

        switch (Type)
        {
            case Bullet.BulletType.NORMAL:
                Normal(rarity, colorIndex);
                break;

            case Bullet.BulletType.SPREAD:
                Spread(rarity, colorIndex);
                break;

            case Bullet.BulletType.MISSILE:
                Missile(rarity, colorIndex);
                break;

            case Bullet.BulletType.LASER:
                Laser(rarity, colorIndex);
                break;

            case Bullet.BulletType.CHARGE:
                Charge(rarity, colorIndex);
                break;

            case Bullet.BulletType.BOOMERANG:
                Boomerang(rarity, colorIndex);
                break;

            case Bullet.BulletType.CHAIN:
                Chain(rarity, colorIndex);
                break;
        }
    }

    void SetPos(GameObject Shooter, int ID)
    {
        if(Shooter.tag == "Player")
        {
            Player player = GameManager.Inst().Player;
            for(int i = 0; i < 5; i++)
                NormalPos[i] = player.NormalPos[i];
            for (int i = 0; i < 7; i++)
                SpreadPos[i] = player.SpreadPos[i];

            LaserPos = player.LaserPos;
            ChargePos = player.ChargePos;
        }
        else if (Shooter.tag == "SubWeapon")
        {
            SubWeapon sub;
            //if (ID > 1)
            //    sub = GameManager.Inst().GetSubweapons(ID - 1);
            //else
            sub = GameManager.Inst().GetSubweapons(ID);
            
            for (int j = 0; j < 5; j++)
                NormalPos[j] = sub.NormalPos[j];
            for (int j = 0; j < 7; j++)
                SpreadPos[j] = sub.SpreadPos[j];

            LaserPos = sub.LaserPos;
            ChargePos = sub.ChargePos;
        }
    }

    void Normal(int Rarity, int Index)
    {
        Normal[] bullets = new Normal[5];

        switch (Rarity)
        {
            case 0:
            case 2:
            case 4:
                for (int i = 0; i <= Rarity; i++)
                {
                    Objs[i] = GameManager.Inst().ObjManager.MakeBullet("Normal", Index);
                    Objs[i].transform.position = NormalPos[i].transform.position;
                    Objs[i].transform.rotation = NormalPos[i].transform.rotation;
                    Objs[i].transform.localScale = NormalPos[i].transform.localScale;

                    bullets[i] = Objs[i].gameObject.GetComponent<Normal>();
                    bullets[i].Shoot(NormalPos[0].transform.up);
                }
                break;

            case 1:
            case 3:
                for(int i = 1; i <= Rarity + 1; i++)
                {
                    Objs[i] = GameManager.Inst().ObjManager.MakeBullet("Normal", Index);
                    Objs[i].transform.position = NormalPos[i].transform.position;
                    Objs[i].transform.rotation = NormalPos[i].transform.rotation;
                    Objs[i].transform.localScale = NormalPos[i].transform.localScale;

                    bullets[i] = Objs[i].gameObject.GetComponent<Normal>();
                    bullets[i].Shoot(NormalPos[0].transform.up);
                }
                break;
        }
        
    }

    void Spread(int Rarity, int Index)
    {
        Spread[] bullets = new Spread[7];
        float duration = GameManager.Inst().UpgManager.BData[(int)Bullet.BulletType.SPREAD].GetDuration();

        switch (Rarity)
        {
            case 0:
            case 2:
            case 4:
                for (int i = 0; i < Rarity + 3; i++)
                {
                    Objs[i] = GameManager.Inst().ObjManager.MakeBullet("Spread", Index);
                    Objs[i].transform.position = SpreadPos[i].transform.position;
                    Objs[i].transform.rotation = SpreadPos[i].transform.rotation;

                    bullets[i] = Objs[i].gameObject.GetComponent<Spread>();
                    bullets[i].Shoot(SpreadPos[i].transform.up);
                    bullets[i].Invoke("Deactivate", duration);
                }
                break;

            case 1:
            case 3:
                for (int i = 1; i <= Rarity + 3; i++)
                {
                    Objs[i] = GameManager.Inst().ObjManager.MakeBullet("Spread", Index);
                    Objs[i].transform.position = SpreadPos[i].transform.position;
                    Objs[i].transform.rotation = SpreadPos[i].transform.rotation;

                    bullets[i - 1] = Objs[i].gameObject.GetComponent<Spread>();
                    bullets[i - 1].Shoot(SpreadPos[i].transform.up);
                    bullets[i - 1].Invoke("Deactivate", duration);
                }
                break;
        }
    }

    void Missile(int Rarity, int Index)
    {
        Missile[] bullets = new Missile[5];
        float rad = GameManager.Inst().UpgManager.BData[(int)Bullet.BulletType.MISSILE].GetDuration();

        switch (Rarity)
        {
            case 0:
                Objs[0] = GameManager.Inst().ObjManager.MakeBullet("Missile", Index);
                Objs[0].transform.position = SpreadPos[0].transform.position;
                Objs[0].transform.rotation = SpreadPos[0].transform.rotation;

                bullets[0] = Objs[0].gameObject.GetComponent<Missile>();
                bullets[0].ResetTarget();
                
                bullets[0].SearchArea.GetComponent<SearchArea>().SetArea(rad);
                bullets[0].Shoot(SpreadPos[0].transform.up);
                break;

            case 1:
            case 2:
                for (int i = 0; i < 2; i++)
                {
                    Objs[i] = GameManager.Inst().ObjManager.MakeBullet("Missile", Index);
                    Objs[i].transform.position = SpreadPos[i + 1].transform.position;
                    Objs[i].transform.rotation = SpreadPos[i + 1].transform.rotation;

                    bullets[i] = Objs[0].gameObject.GetComponent<Missile>();
                    bullets[i].ResetTarget();
                    
                    bullets[i].SearchArea.GetComponent<SearchArea>().SetArea(rad);
                    bullets[i].Shoot(SpreadPos[0].transform.up);
                }
                break;

            case 3:
            case 4:
                for (int i = 0; i < 3; i++)
                {
                    Objs[i] = GameManager.Inst().ObjManager.MakeBullet("Missile", Index);
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

    void Laser(int Rarity, int Index)
    {
        Objs[0] = GameManager.Inst().ObjManager.MakeBullet("Laser", Index);
        Vector3 scale = Objs[0].gameObject.transform.localScale;
        scale.x = (0.25f * (Rarity + 1));
        Objs[0].transform.localScale = scale;
        Objs[0].transform.position = LaserPos.transform.position;
        Objs[0].transform.rotation = LaserPos.transform.rotation;

        GameManager.Inst().IptManager.SetIsAbleControl(false);
    }

    void Charge(int Rarity, int Index)
    {
        Objs[0] = GameManager.Inst().ObjManager.MakeBullet("Charge", Index);
        Objs[0].transform.position = ChargePos.transform.position;
        Objs[0].transform.rotation = ChargePos.transform.rotation;

        Charge bullet = Objs[0].gameObject.GetComponent<Charge>();
        bullet.SetChargePos(ChargePos);
        bullet.StartCharge(GameManager.Inst().UpgManager.BData[(int)Bullet.BulletType.CHARGE].GetDuration());
    }

    void Boomerang(int Rarity, int Index)
    {
        Objs[0] = GameManager.Inst().ObjManager.MakeBullet("Boomerang", Index);
        Objs[0].transform.position = NormalPos[0].transform.position;
        Objs[0].transform.rotation = NormalPos[0].transform.rotation;
        Objs[0].transform.localScale = Vector3.one;
        if (Rarity == 4)
            Objs[0].transform.localScale = Vector3.one * 2.0f;
        else if(Rarity > 1)
            Objs[0].transform.localScale = Vector3.one * 1.5f;

        Boomerang bullet = Objs[0].gameObject.GetComponent<Boomerang>();
        bullet.SetStartPos(Objs[0].transform.position);
        bullet.SetTargetpos(Objs[0].transform.position + NormalPos[0].transform.up/*GameManager.Inst().IptManager.MousePosition*/);
        bullet.SetStart();
    }

    void Chain(int Rarity, int Index)
    {
        Objs[0] = GameManager.Inst().ObjManager.MakeBullet("Chain", Index);
        Objs[0].transform.position = NormalPos[0].transform.position;
        Objs[0].transform.rotation = NormalPos[0].transform.rotation;

        Chain bullet = Objs[0].gameObject.GetComponent<Chain>();
        bullet.ResetData();
        bullet.Shoot(NormalPos[0].transform.up);
    }
}
