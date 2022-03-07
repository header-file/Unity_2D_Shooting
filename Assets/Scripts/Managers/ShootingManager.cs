using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingManager : MonoBehaviour
{
    public int MAXCOLOR = 8;
    public int[] BaseColor;
    public GameObject EnemyBottom;

    GameObject[] NormalPos;
    GameObject[] SpreadPos;
    GameObject LaserPos;
    GameObject ChargePos;
    GameObject[] GatlingPos;

    GameObject[] Objs;
    Color[] Colors;

    int[] ColorSelection;
    int DotCount;
    int[] DotDirs;


    public Color GetColors(int index) { return Colors[index]; }
    public int GetColorSelection(int index) { return ColorSelection[index]; }
        
    public void SetColorSelection(int index, int val) { ColorSelection[index] = val; }

    void Awake()
    {
        GameManager.Inst().ShtManager = gameObject.GetComponent<ShootingManager>();

        Objs = new GameObject[10];
        NormalPos = new GameObject[7];
        SpreadPos = new GameObject[7];
        LaserPos = new GameObject();
        ChargePos = new GameObject();
        GatlingPos = new GameObject[2];

        Colors = new Color[MAXCOLOR];
        ColorSelection = new int[5];

        for (int i = 0; i < MAXCOLOR; i++)
            Colors[i] = GameManager.Inst().UiManager.MainUI.Bottom.Colors[i].color;

        for (int i = 0; i < 5; i++)
            ColorSelection[i] = 0;

        DotDirs = new int[8];
    }

    void Start()
    {
        SetColor();

        SetDotDirs();
    }

    void SetColor()
    {
        for (int i = 0; i < 5; i++)
        {
            if (i == 2)
                ColorSelection[i] = GameManager.Inst().Player.GetColorIndex();
            else if (i > 2)
            {
                if (GameManager.Inst().GetSubweapons(i - 1) != null)
                    ColorSelection[i] = GameManager.Inst().GetSubweapons(i - 1).GetColorIndex();
            }
            else if(i < 2)
                if (GameManager.Inst().GetSubweapons(i) != null)
                    ColorSelection[i] = GameManager.Inst().GetSubweapons(i).GetColorIndex();
        }
    }

    void SetDotDirs()
    {
        DotDirs[0] = 4;
        DotDirs[1] = 2;
        DotDirs[2] = 0;
        DotDirs[3] = 1;
        DotDirs[4] = 3;
        DotDirs[5] = 1;
        DotDirs[6] = 0;
        DotDirs[7] = 2;
    }

    public void Shoot(Bullet.BulletType Type, GameObject Shooter, int ID, bool IsVamp, bool IsReinfoce, int index)
    {
        SetPos(Shooter, ID);

        int rarity = 0;
        if ((int)Type < 100)
            rarity = GameManager.Inst().UpgManager.BData[(int)Type].GetRarity();

        int colorIndex = ColorSelection[ID];
        if (Shooter.tag == "SubWeapon" && ID > 1)
            colorIndex = ColorSelection[ID + 1];

        switch (Type)
        {
            case Bullet.BulletType.NORMAL:
                Normal(Shooter, rarity, colorIndex, IsVamp, IsReinfoce);
                break;

            case Bullet.BulletType.SPREAD:
                Spread(Shooter, rarity, colorIndex, IsVamp, IsReinfoce);
                break;

            case Bullet.BulletType.MISSILE:
                Missile(Shooter, rarity, colorIndex, IsVamp, IsReinfoce);
                break;

            case Bullet.BulletType.LASER:
                Laser(Shooter, rarity, colorIndex, IsVamp, IsReinfoce);
                break;

            case Bullet.BulletType.CHARGE:
                Charge(Shooter, rarity, colorIndex, IsVamp, IsReinfoce);
                break;

            case Bullet.BulletType.BOOMERANG:
                Boomerang(Shooter, rarity, colorIndex, IsVamp, IsReinfoce);
                break;

            case Bullet.BulletType.CHAIN:
                Chain(Shooter, rarity, colorIndex, IsVamp, IsReinfoce);
                break;

            case Bullet.BulletType.GATLING:
                Gatling(Shooter, rarity, colorIndex, IsVamp, IsReinfoce);
                break;

            case Bullet.BulletType.EXPLOSION:
                Explosion(Shooter, rarity, colorIndex, IsVamp, IsReinfoce);
                break;

            case Bullet.BulletType.DOT:
                Dot(Shooter, rarity, colorIndex, IsVamp, IsReinfoce);
                break;

            case Bullet.BulletType.EQUIP_MISSILE:
                Equip_Missile(index);
                break;

            case Bullet.BulletType.EQUIP_KNOCKBACK:
                Equip_Knockback(index);
                break;

            case Bullet.BulletType.EQUIP_SLOW:
                Equip_Slow();
                break;
        }
    }

    void SetPos(GameObject Shooter, int ID)
    {
        if(Shooter.tag == "Player")
        {
            Player player = GameManager.Inst().Player;
            for(int i = 0; i < 7; i++)
                NormalPos[i] = player.NormalPos[i];
            for (int i = 0; i < 7; i++)
                SpreadPos[i] = player.SpreadPos[i];
            for (int i = 0; i < 2; i++)
                GatlingPos[i] = player.GatlingPoses[i];

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
            
            for (int j = 0; j < 7; j++)
                NormalPos[j] = sub.NormalPos[j];
            for (int j = 0; j < 7; j++)
                SpreadPos[j] = sub.SpreadPos[j];
            for (int i = 0; i < 2; i++)
                GatlingPos[i] = sub.GatlingPoses[i];

            LaserPos = sub.LaserPos;
            ChargePos = sub.ChargePos;
        }
    }

    void Normal(GameObject shooter, int Rarity, int Index, bool isVamp, bool IsReinforce)
    {
        Normal[] bullets = new Normal[7];
        Vector3 scale = Vector3.one;
        if (GameManager.Inst().Player.GetBossMode())
            scale *= 0.5f;
        if (IsReinforce)
            scale *= 1.5f;

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
                    Objs[i].transform.localScale = scale * NormalPos[i].transform.localScale.x;

                    bullets[i] = Objs[i].gameObject.GetComponent<Normal>();
                    bullets[i].IsVamp = isVamp;
                    bullets[i].Vamp = shooter;
                    bullets[i].IsReinforce = IsReinforce;
                    bullets[i].Shoot(NormalPos[0].transform.up);
                }
                break;

            case 1:
            case 3:
                for(int i = 6 - Rarity; i <= 6; i++)
                {
                    Objs[i] = GameManager.Inst().ObjManager.MakeBullet("Normal", Index);
                    Objs[i].transform.position = NormalPos[i].transform.position;
                    Objs[i].transform.rotation = NormalPos[i].transform.rotation;
                    Objs[i].transform.localScale = scale * NormalPos[i].transform.localScale.x;

                    bullets[i] = Objs[i].gameObject.GetComponent<Normal>();
                    bullets[i].IsVamp = isVamp;
                    bullets[i].Vamp = shooter;
                    bullets[i].IsReinforce = IsReinforce;
                    bullets[i].Shoot(NormalPos[i].transform.up);
                }
                break;
        }

        GameManager.Inst().SodManager.PlayEffect("Wp_Normal");
    }

    void Spread(GameObject shooter, int Rarity, int Index, bool isVamp, bool IsReinforce)
    {
        Spread[] bullets = new Spread[7];
        Vector3 scale = Vector3.one;
        if (GameManager.Inst().Player.GetBossMode())
            scale *= 0.5f;
        if (IsReinforce)
            scale *= 1.5f;
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
                    Objs[i].transform.localScale = scale;

                    bullets[i] = Objs[i].gameObject.GetComponent<Spread>();
                    bullets[i].IsVamp = isVamp;
                    bullets[i].Vamp = shooter;
                    bullets[i].IsReinforce = IsReinforce;
                    bullets[i].SetDuration(duration);
                    bullets[i].Shoot(SpreadPos[i].transform.up);
                    //bullets[i].Invoke("Deactivate", duration);
                }
                break;

            case 1:
            case 3:
                for (int i = 1; i <= Rarity + 3; i++)
                {
                    Objs[i] = GameManager.Inst().ObjManager.MakeBullet("Spread", Index);
                    Objs[i].transform.position = SpreadPos[i].transform.position;
                    Objs[i].transform.rotation = SpreadPos[i].transform.rotation;
                    Objs[i].transform.localScale = scale;

                    bullets[i - 1] = Objs[i].gameObject.GetComponent<Spread>();
                    bullets[i - 1].IsVamp = isVamp;
                    bullets[i - 1].Vamp = shooter;
                    bullets[i - 1].IsReinforce = IsReinforce;
                    bullets[i - 1].SetDuration(duration);
                    bullets[i - 1].Shoot(SpreadPos[i].transform.up);
                    //bullets[i - 1].Invoke("Deactivate", duration);
                }
                break;
        }

        GameManager.Inst().SodManager.PlayEffect("Wp_Spread");
    }

    void Missile(GameObject shooter, int Rarity, int Index, bool isVamp, bool IsReinforce)
    {
        Missile[] bullets = new Missile[5];
        Vector3 scale = Vector3.one;
        if (GameManager.Inst().Player.GetBossMode())
            scale *= 0.5f;
        if (IsReinforce)
            scale *= 1.5f;
        float rad = GameManager.Inst().UpgManager.BData[(int)Bullet.BulletType.MISSILE].GetDuration();

        switch (Rarity)
        {
            case 0:
                Objs[0] = GameManager.Inst().ObjManager.MakeBullet("Missile", Index);
                Objs[0].transform.position = SpreadPos[0].transform.position;
                Objs[0].transform.rotation = SpreadPos[0].transform.rotation;
                Objs[0].transform.localScale = scale;

                bullets[0] = Objs[0].gameObject.GetComponent<Missile>();
                bullets[0].IsVamp = isVamp;
                bullets[0].Vamp = shooter;
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
                    Objs[0].transform.localScale = scale;

                    bullets[i] = Objs[0].gameObject.GetComponent<Missile>();
                    bullets[i].IsVamp = isVamp;
                    bullets[i].Vamp = shooter;
                    bullets[i].IsReinforce = IsReinforce;
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
                    Objs[0].transform.localScale = scale;

                    bullets[i] = Objs[i].gameObject.GetComponent<Missile>();
                    bullets[i].IsVamp = isVamp;
                    bullets[i].Vamp = shooter;
                    bullets[i].IsReinforce = IsReinforce;
                    bullets[i].ResetTarget();

                    bullets[i].SearchArea.GetComponent<SearchArea>().SetArea(rad);
                    bullets[i].Shoot(SpreadPos[i].transform.up);
                }
                break;
        }

        GameManager.Inst().SodManager.PlayEffect("Wp_Missile");
    }

    void Laser(GameObject shooter, int Rarity, int Index, bool isVamp, bool IsReinforce)
    {
        Vector3 scale = Vector3.one;
        if (GameManager.Inst().Player.GetBossMode())
            scale *= 0.5f;
        if (IsReinforce)
            scale *= 1.5f;

        Objs[0] = GameManager.Inst().ObjManager.MakeBullet("Laser", Index);
        Objs[0].transform.localScale = scale;

        Laser bullet = Objs[0].GetComponent<Laser>();
        bullet.IsVamp = isVamp;
        bullet.Vamp = shooter;
        bullet.IsReinforce = IsReinforce;
        bullet.SetStartPos(LaserPos);

        GameManager.Inst().SodManager.PlayEffect("Wp_Laser");
    }

    void Charge(GameObject shooter, int Rarity, int Index, bool isVamp, bool IsReinforce)
    {
        Vector3 scale = Vector3.one;
        if (GameManager.Inst().Player.GetBossMode())
            scale *= 0.5f;
        if (IsReinforce)
            scale *= 1.5f;

        Objs[0] = GameManager.Inst().ObjManager.MakeBullet("Charge", Index);
        Objs[0].transform.position = ChargePos.transform.position;
        Objs[0].transform.rotation = ChargePos.transform.rotation;
        Objs[0].transform.localScale = scale;

        Charge bullet = Objs[0].gameObject.GetComponent<Charge>();
        bullet.IsVamp = isVamp;
        bullet.Vamp = shooter;
        bullet.IsReinforce = IsReinforce;
        bullet.SetChargePos(ChargePos);
        bullet.StartCharge(GameManager.Inst().UpgManager.BData[(int)Bullet.BulletType.CHARGE].GetDuration());

        GameManager.Inst().SodManager.PlayEffect("Wp_Charge");
    }

    void Boomerang(GameObject shooter, int Rarity, int Index, bool isVamp, bool IsReinforce)
    {
        Vector3 scale = Vector3.one;
        if (GameManager.Inst().Player.GetBossMode())
            scale *= 0.5f;
        if (IsReinforce)
            scale *= 1.5f;

        Objs[0] = GameManager.Inst().ObjManager.MakeBullet("Boomerang", Index);
        Objs[0].transform.position = NormalPos[0].transform.position;
        Objs[0].transform.rotation = NormalPos[0].transform.rotation;
        Objs[0].transform.localScale = scale;

        Boomerang bullet = Objs[0].gameObject.GetComponent<Boomerang>();
        bullet.IsVamp = isVamp;
        bullet.Vamp = shooter;
        bullet.IsReinforce = IsReinforce;
        bullet.SetStart();

        GameManager.Inst().SodManager.PlayEffect("Wp_Boomerang");
    }

    void Chain(GameObject shooter, int Rarity, int Index, bool isVamp, bool IsReinforce)
    {
        Vector3 scale = Vector3.one;
        if (GameManager.Inst().Player.GetBossMode())
            scale *= 0.5f;
        if (IsReinforce)
            scale *= 1.5f;

        Objs[0] = GameManager.Inst().ObjManager.MakeBullet("Chain", Index);
        Objs[0].transform.position = NormalPos[0].transform.position;
        Objs[0].transform.rotation = NormalPos[0].transform.rotation;
        Objs[0].transform.localScale = scale;

        Chain bullet = Objs[0].gameObject.GetComponent<Chain>();
        bullet.IsVamp = isVamp;
        bullet.Vamp = shooter;
        bullet.IsReinforce = IsReinforce;
        bullet.ResetData();
        bullet.Shoot(NormalPos[0].transform.up);

        GameManager.Inst().SodManager.PlayEffect("Wp_Chain");
    }

    void Gatling(GameObject shooter, int Rarity, int Index, bool isVamp, bool IsReinforce)
    {
        Vector3 scale = Vector3.one;
        if (GameManager.Inst().Player.GetBossMode())
            scale *= 0.5f;
        if (IsReinforce)
            scale *= 1.5f;

        Gatling[] bullets = new Gatling[2];
        for (int i = 0; i < 2; i++)
        {
            Objs[i] = GameManager.Inst().ObjManager.MakeBullet("Gatling", Index);
            Objs[i].transform.position = GatlingPos[i].transform.position;
            Objs[i].transform.rotation = GatlingPos[i].transform.rotation;
            Objs[i].transform.localScale = scale;

            bullets[i] = Objs[i].gameObject.GetComponent<Gatling>();
            bullets[i].IsVamp = isVamp;
            bullets[i].Vamp = shooter;
            bullets[i].IsReinforce = IsReinforce;
            bullets[i].Shoot(GatlingPos[i].transform.up);
        }

        GameManager.Inst().SodManager.PlayEffect("Wp_Gatling");
    }

    void Explosion(GameObject shooter, int Rarity, int Index, bool isVamp, bool IsReinforce)
    {
        Vector3 scale = Vector3.one;
        if (GameManager.Inst().Player.GetBossMode())
            scale *= 0.5f;
        if (IsReinforce)
            scale *= 1.5f;

        Objs[0] = GameManager.Inst().ObjManager.MakeBullet("Explosion", Index);
        Objs[0].transform.position = NormalPos[0].transform.position;
        Objs[0].transform.rotation = NormalPos[0].transform.rotation;
        Objs[0].transform.localScale = scale;

        Explosion bullet = Objs[0].gameObject.GetComponent<Explosion>();
        bullet.IsVamp = isVamp;
        bullet.Vamp = shooter;
        bullet.IsReinforce = IsReinforce;
        bullet.Shoot(NormalPos[0].transform.up);

        GameManager.Inst().SodManager.PlayEffect("Wp_Explosion");
    }

    void Dot(GameObject shooter, int Rarity, int Index, bool isVamp, bool IsReinforce)
    {
        Vector3 scale = Vector3.one;
        if (GameManager.Inst().Player.GetBossMode())
            scale *= 0.5f;
        if (IsReinforce)
            scale *= 1.5f;

        Objs[0] = GameManager.Inst().ObjManager.MakeBullet("Dot", Index);
        Objs[0].transform.position = SpreadPos[DotDirs[DotCount]].transform.position;
        Objs[0].transform.rotation = SpreadPos[DotDirs[DotCount]].transform.rotation;
        Objs[0].transform.localScale = scale;

        Dot bullet = Objs[0].gameObject.GetComponent<Dot>();
        bullet.IsVamp = isVamp;
        bullet.Vamp = shooter;
        bullet.IsReinforce = IsReinforce;
        bullet.Speed = GameManager.Inst().UpgManager.BData[(int)bullet.GetBulletType()].GetSpeed() * 2.0f;
        bullet.Shoot(SpreadPos[DotDirs[DotCount]].transform.up);
        DotCount++;
        if (DotCount >= DotDirs.Length)
            DotCount = 0;

        GameManager.Inst().SodManager.PlayEffect("Wp_Dot");
    }

    void Equip_Missile(int index)
    {
        Missile bullet;
        float rad = 2.0f;

        Vector3 scale = Vector3.one;
        if (GameManager.Inst().Player.GetBossMode())
            scale *= 0.5f;

        Objs[0] = GameManager.Inst().ObjManager.MakeObj("EqMissile");
        Objs[0].transform.position = LaserPos.transform.position;
        Objs[0].transform.rotation = LaserPos.transform.rotation;
        Objs[0].transform.localScale = scale;

        bullet = Objs[0].gameObject.GetComponent<Missile>();
        bullet.SetBulletType((int)Bullet.BulletType.EQUIP_MISSILE);
        bullet.InventoryIndex = index;
        bullet.ResetTarget();
        bullet.StartTraceTimer();

        bullet.SearchArea.GetComponent<SearchArea>().SetArea(rad);
        bullet.ShootEquip(LaserPos.transform.up, (int)Bullet.BulletType.MISSILE);

        GameManager.Inst().SodManager.PlayEffect("Wp_Missile");
    }

    void Equip_Knockback(int index)
    {
        KnockBack bullet;
        Vector3 scale = Vector3.one;
        if (GameManager.Inst().Player.GetBossMode())
            scale *= 0.5f;

        Objs[0] = GameManager.Inst().ObjManager.MakeObj("EqKnockback");
        Objs[0].transform.position = NormalPos[0].transform.position;
        Objs[0].transform.rotation = NormalPos[0].transform.rotation;
        Objs[0].transform.localScale = scale;

        bullet = Objs[0].gameObject.GetComponent<KnockBack>();
        bullet.SetBulletType((int)Bullet.BulletType.EQUIP_KNOCKBACK);
        bullet.InventoryIndex = index;
        bullet.Direction = NormalPos[0].transform.up;
        Objs[0].GetComponent<ActivationTimer>().IsStart = true;

        bullet.ShootEquip(NormalPos[0].transform.up, (int)Bullet.BulletType.KNOCKBACK);

        GameManager.Inst().SodManager.PlayEffect("Eq_Wave");
    }

    void Equip_Slow()
    {
        Slow bullet;
        Vector3 scale = Vector3.one;
        if (GameManager.Inst().Player.GetBossMode())
            scale *= 0.5f;

        Objs[0] = GameManager.Inst().ObjManager.MakeObj("EqSlow");
        Objs[0].transform.position = NormalPos[0].transform.position;
        Objs[0].transform.rotation = NormalPos[0].transform.rotation;
        Objs[0].transform.localScale = scale;

        bullet = Objs[0].gameObject.GetComponent<Slow>();
        bullet.SetBulletType((int)Bullet.BulletType.EQUIP_SLOW);
        bullet.Col.radius = 1.2f;
        bullet.Area.transform.localScale = Vector3.one * 1.2f;

        bullet.ShootEquip(NormalPos[0].transform.up, (int)Bullet.BulletType.SLOW);
        Objs[0].gameObject.GetComponent<ActivationTimer>().IsStart = true;

        GameManager.Inst().SodManager.PlayEffect("Eq_Distort");
    }
}
