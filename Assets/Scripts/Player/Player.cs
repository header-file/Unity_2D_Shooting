using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;
using System;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    public class EqData
    {
        public Sprite Icon;
        public int Type;
        public int Rarity;
        public float Value;
        public int UID;
        public int Quantity;
        public float CoolTime;

        public EqData()
        {
            Icon = null;
            Type = -1;
            Rarity = -1;
            Value = 0.0f;
            UID = 0;
            Quantity = 0;
            CoolTime = 0.0f;
        }

    };

    const float DEATHTIME = 5.0f;

    public SpriteResolver Skin;
    public string[] Types;

    public GameObject[] NormalPos;
    public GameObject[] SpreadPos;
    public GameObject LaserPos;
    public GameObject ChargePos;
    public GameObject[] BoomerangPos;
    public GameObject[] GatlingPoses;
    public GameObject[] BossSubPoses;

    public GameObject Shield;

    public GameObject UI;
    public Image TimerImage;
    public Text TimerText;
    public GameObject HPUI;
    public Image HPBar;
    public GameObject EquipUI;
    public Image EquipBar;
    public Image EquipIcon;
    public Sprite QuestionMark;

    public ShieldPart[] ShieldParts;

    public ObjectShake Shaker;

    public int MaxInventory;
    public int CurInventory;
    public int SortOption;
    public int ShootCount;

    //Cheat
    public bool IsGodMode;

    GameObject[] SubWeapons;
    EqData[] Inventory;
    EqData[] ReinforceInventory;
    Vector3 OriginalPos;
    CanvasGroup canvasGroup;
    Vector3 NormalSize;
    Vector3 BossSize;
    

    Vector3 PlayerPos;
    Vector3 UIOriPos;
    bool IsReload;
    int Coin;
    int BulletType;
    bool IsMovable;
    bool IsShield;
    int MaxHP;
    int CurHP;
    bool IsDead;
    bool IsInvincible;
    float DeathTimer;
    float GatlingGyesu;
    int ColorIndex;
    bool IsAutoShot;
    bool IsBossMode;
    

    public GameObject GetSubWeapon(int index) { return SubWeapons[index]; }
    public GameObject GetChargePos() { return ChargePos; }
    public EqData GetItem(int index) { return Inventory[index] != null ? Inventory[index] : null; }
    public EqData GetReinforce(int index) { return ReinforceInventory[index] != null ? ReinforceInventory[index] : null; }
    public bool GetBossMode() { return IsBossMode; }

    public int GetCoin() { return Coin; }
    public int GetBulletType() { return BulletType; }
    public bool GetIsMovable() { return IsMovable; }
    public int GetMaxHP() { return MaxHP; }
    public int GetCurHP() { return CurHP; }
    public int GetColorIndex() { return ColorIndex; }


    public void SetMaxHP(int hp) { MaxHP = hp; }
    public void SetCurHP(int hp) { CurHP = hp; }
    public void SetSubWeapon(GameObject obj, int index) { SubWeapons[index] = obj; }
    public void SetBulletType(int type)
    {
        BulletType = type;
        SetSkin();
        SetSkinColor(GameManager.Inst().ShtManager.BaseColor[BulletType]);

        SetHPs();
    }

    public void SetHPs()
    {
        int dam = MaxHP - CurHP;
        MaxHP = GameManager.Inst().UpgManager.BData[BulletType].GetHealth() + GameManager.Inst().UpgManager.BData[BulletType].GetHp();
        CurHP = MaxHP - dam;
    }

    public void BossMode()
    {
        IsMovable = true;
        IsBossMode = true;

        gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);

        for (int i = 0; i < 4; i++)
        {
            if (GameManager.Inst().GetSubweapons(i) != null)
                GameManager.Inst().GetSubweapons(i).BossMode();
        }
    }

    public void EndBossMode()
    {
        IsMovable = false;
        IsInvincible = true;

        InvokeRepeating("MoveBack", 0.0f, Time.deltaTime);        
    }

    public void SetCoin(int c)
    {
        Coin = c;

        GameManager.Inst().UiManager.SetCoinText(Coin);
    }

    public void AddCoin(int c)
    {
        Coin += c;

        GameManager.Inst().UiManager.SetCoinText(Coin);
    }

    public void MinusCoin(int c)
    {
        Coin -= c;

        GameManager.Inst().UiManager.SetCoinText(Coin);
    }

    public int AddItem(Item_Equipment item)
    {
        if (ReinforceInventory[item.GetEqType()] != null)
            ReinforceInventory[item.GetEqType()].Quantity++;
        else
        {
            ReinforceInventory[item.GetEqType()] = new EqData();
            ReinforceInventory[item.GetEqType()].Icon = item.GetIcon();
            ReinforceInventory[item.GetEqType()].Type = item.GetEqType();
            ReinforceInventory[item.GetEqType()].Rarity = item.GetRarity();
            ReinforceInventory[item.GetEqType()].Value = item.GetEqValue();
            ReinforceInventory[item.GetEqType()].UID = item.GetUID();
        }

        return -1;
    }

    public int AddItem(Item_ZzinEquipment item)
    {
        for (int i = 0; i < MaxInventory; i++)
        {
            if (Inventory[i] == null)
            {
                Inventory[i] = new EqData();
                Inventory[i].Icon = item.GetIcon();
                Inventory[i].Type = item.GetEqType();
                Inventory[i].Rarity = item.GetRarity();
                Inventory[i].Value = item.GetValue();
                Inventory[i].UID = item.GetUID();
                Inventory[i].Quantity = 1;
                Inventory[i].CoolTime = item.GetCoolTime();

                CurInventory++;

                return i;
            }
        }

        GameManager.Inst().UiManager.InventoryFull();

        return -1;
    }

    public int AddItem(EqData item)
    {
        if (item.UID / 100 == 3)
        {
            if (ReinforceInventory[item.Type] != null)
                ReinforceInventory[item.Type].Quantity++;
            else
            {
                ReinforceInventory[item.Type] = new EqData();
                ReinforceInventory[item.Type].Icon = item.Icon;
                ReinforceInventory[item.Type].Type = item.Type;
                ReinforceInventory[item.Type].Rarity = item.Rarity;
                ReinforceInventory[item.Type].Value = item.Value;
                ReinforceInventory[item.Type].UID = item.UID;
                ReinforceInventory[item.Type].Quantity = item.Quantity;
            }
        }
        else
        {
            for (int i = 0; i < MaxInventory; i++)
            {
                if (Inventory[i] == null)
                {
                    Inventory[i] = new EqData();
                    Inventory[i].Icon = item.Icon;
                    Inventory[i].Type = item.Type;
                    Inventory[i].Rarity = item.Rarity;
                    Inventory[i].Value = item.Value;
                    Inventory[i].UID = item.UID;
                    if (item.Quantity > 0)
                        Inventory[i].Quantity = item.Quantity;
                    else
                        Inventory[i].Quantity = 1;
                    Inventory[i].CoolTime = item.CoolTime;

                    CurInventory++;

                    return i;
                }
            }
        }

        return -1;
    }

    public int FindQuantityAsGrade(int rarity)
    {
        for (int i = 0; i < MaxInventory; i++)
        {
            if(Inventory[i] != null)
                if (Inventory[i].UID / 100 == 3 &&
                    Inventory[i].Rarity == rarity)
                    return Inventory[i].Quantity;
        }

        return -1;
    }

    void Swap(int i, int j)
    {
        if (Inventory[i] == null)
        {
            Inventory[i] = Inventory[j];
            Inventory[j] = null;
        }
        else if (Inventory[j] == null)
        {
            Inventory[j] = Inventory[i];
            Inventory[i] = null;
        }
        else if (Inventory[i] != null && Inventory[j] != null)
        {
            EqData temp = new EqData();
            temp = Inventory[i];
            Inventory[i] = Inventory[j];
            Inventory[j] = temp;
        }
    }

    public void RemoveItem(int index, int quantity)
    {
        Inventory[index].Quantity -= quantity;

        if (Inventory[index].Quantity <= 0)
            DiscardItem(index);
    }

    public void RemoveItem(int index)
    {
        Inventory[index] = null;

        CurInventory--;
    }

    public EqData DiscardItem(int index)
    {
        EqData e = new EqData();
        e.Icon = Inventory[index].Icon;
        e.Type = Inventory[index].Type;
        e.Rarity = Inventory[index].Rarity;
        e.Value = Inventory[index].Value;

        Inventory[index] = null;

        CurInventory--;

        return e;
    }

    void Awake()
    {
        Player[] objs = FindObjectsOfType<Player>();
        if (objs.Length > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);

        MaxInventory = 40;

        Coin = 0;

        IsReload = true;
        BulletType = 0;
        SetSkin();

        SubWeapons = new GameObject[4];
        Inventory = new EqData[Constants.MAXINVENTORY];
        ReinforceInventory = new EqData[Constants.MAXREINFORCETYPE];
        for (int i = 0; i < Constants.MAXINVENTORY; i++)
            Inventory[i] = null;

        IsMovable = false;
        IsBossMode = false;
        IsShield = false;
        OriginalPos = new Vector3(0.0f, 1.2f, 0.0f);

        IsDead = false;
        IsInvincible = false;
        DeathTimer = 0.0f;
        CurHP = MaxHP = 0;

        ShootCount = 0;

        Shield.SetActive(false);

        for (int i = 0; i < ShieldParts.Length; i++)
            ShieldParts[i].gameObject.SetActive(false);

        GatlingGyesu = 1.0f;

        NormalSize = Vector3.one * 0.8f;
        BossSize = Vector3.one * 0.4f;

        IsAutoShot = false;
    }

    void Start()
    {
        GameManager.Inst().UiManager.SetCoinText(Coin);

        UISetting();
    }

    void Update()
    {
        if(IsBossMode || GameManager.Inst().UiManager.MainUI.Bottom.WeaponScroll.IsOpen)
            SetUIPos();

        EquipCount();

        if (BulletType == (int)Bullet.BulletType.GATLING)
            GatlingMove();

        if(IsMovable)
        {
            if(transform.localScale.x > BossSize.x)
            {
                Vector3 size = transform.localScale;
                size -= Vector3.one * 0.05f;
                transform.localScale = size;
            }
        }
        else
        {
            if(transform.localScale.x < NormalSize.x)
            {
                Vector3 size = transform.localScale;
                size += Vector3.one * 0.05f;
                transform.localScale = size;
            }
        }

        if (IsAutoShot || GameManager.Inst().IsFullPrice)
            Fire();
    }

    void SetUIPos()
    {
        PlayerPos = transform.position;
        PlayerPos.y += 3.0f;
        PlayerPos.z = 90.0f;
        UI.transform.position = PlayerPos;
    }

    public void SetUIPosOri()
    {
        UI.transform.position = UIOriPos;
        GameManager.Inst().UiManager.MainUI.Center.Turret.transform.localPosition = Vector3.zero;
        for (int i = 0; i < 4; i++)
            GameManager.Inst().UiManager.MainUI.Center.Turrets[i].Button.transform.localPosition = Vector3.zero;
    }

    void EquipCount()
    {
        if (IsDead || GameManager.Inst().UpgManager.BData[BulletType].GetEquipIndex() == -1 || CheckPassive())
            return;

        GameManager.Inst().EquManager.Count(gameObject, GameManager.Inst().UpgManager.BData[BulletType].GetEquipIndex(), 2);

        if (EquipUI.activeSelf)
            SetEquipUI();
    }

    void GatlingMove()
    {
        Vector2 gatPos = GatlingPoses[0].transform.localPosition;
        gatPos.x -= Time.fixedDeltaTime * GatlingGyesu;

        GatlingPoses[0].transform.localPosition = gatPos;
        GatlingPoses[1].transform.localPosition = -gatPos;

        if (Mathf.Abs(gatPos.x) >= 0.5f)
            GatlingGyesu *= -1.0f;
    }

    bool CheckPassive()
    {
        int type = GetItem(GameManager.Inst().UpgManager.BData[BulletType].GetEquipIndex()).Type;

        if (type == (int)Item_ZzinEquipment.EquipType.REINFORCE ||
            type == (int)Item_ZzinEquipment.EquipType.VAMP)
            return true;
        else
            return false;
    }

    public void UISetting()
    {
        TimerImage.gameObject.SetActive(false);
        HPUI.SetActive(false);
        HPBar.fillAmount = 0.415f;

        EquipUI.SetActive(false);
        EquipBar.fillAmount = 0.0f;
        QuestionMark = EquipIcon.sprite;

        UIOriPos = transform.position;
        UIOriPos.y += 3.0f;
        UIOriPos.z = 90.0f;
    }

    public void ShowEquipUI()
    {
        EquipUI.SetActive(true);

        SetEquipUI();
    }

    public void SetEquipUI()
    {
        if (GameManager.Inst().UpgManager.BData[BulletType].GetEquipIndex() > -1)
        {
            EqData e = GetItem(GameManager.Inst().UpgManager.BData[BulletType].GetEquipIndex());

            EquipIcon.sprite = e.Icon;

            if (GameManager.Inst().EquipDatas[e.Type, e.Rarity, 0] > 0)
            {
                EquipBar.fillAmount = 1.0f - (e.CoolTime / GameManager.Inst().EquipDatas[e.Type, e.Rarity, 0]);
            }
            else
            {
                if (e.Type == 3)
                    EquipBar.fillAmount = 1.0f;
                else if (e.Type == 6)
                    EquipBar.fillAmount = 1.0f - (ShootCount / e.Value);
            }
        }
        else
        {
            EquipIcon.sprite = QuestionMark;
            EquipBar.fillAmount = 0.0f;
        }
    }

    public void Rotate(Vector2 MousePos)
    {
        if (IsDead)
            return;

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 norm = (MousePos - pos) / Vector2.Distance(MousePos, pos);
        float angle = Vector2.Angle(Vector2.up, norm);
        if (MousePos.x > transform.position.x)
            angle *= -1;
        Quaternion rot = Quaternion.Euler(0.0f, 0.0f, angle);
        transform.rotation = rot;
    }

    public void Fire()
    {
        if(!IsReload || IsDead)
            return;
        
        IsReload = false;

        if(GameManager.Inst().UpgManager.BData[BulletType].GetIsReinforce())
            ShootCount++;

        if (GameManager.Inst().UpgManager.BData[BulletType].GetIsReinforce() &&
            ShootCount >= GetItem(GameManager.Inst().UpgManager.BData[BulletType].GetEquipIndex()).Value)
        {
            ShootCount = 0;

            //장비 효과 발동 이펙트
            GameObject EquipAction = GameManager.Inst().ObjManager.MakeObj("EquipAction");
            EquipAction.transform.position = transform.position;
            EquipAction.GetComponent<ActivationTimer>().IsStart = true;

            GameManager.Inst().ShtManager.Shoot((Bullet.BulletType)BulletType, gameObject, 2, false, true, -1);
        }
        else
            GameManager.Inst().ShtManager.Shoot((Bullet.BulletType)BulletType, gameObject, 2, GameManager.Inst().UpgManager.BData[BulletType].GetIsVamp(), false, -1);

        Invoke("Reload", GameManager.Inst().UpgManager.BData[BulletType].GetReloadTime());
    }

    void Reload()
    {
        IsReload = true;
    }

    void MoveBack()
    {
        transform.position = Vector3.MoveTowards(transform.position, OriginalPos, Time.deltaTime * 3.0f);

        if (Vector3.Distance(transform.position, OriginalPos) <= 0.0001f)
        {
            CancelInvoke("MoveBack");
            IsInvincible = false;
            IsBossMode = false;

            SetUIPosOri();

            for (int i = 0; i < 4; i++)
            {
                if (GameManager.Inst().GetSubweapons(i) != null)
                    GameManager.Inst().GetSubweapons(i).EndBossMode();
            }
        }
    }

    public void Damage(int damage)
    {
        if (IsGodMode)
            return;
        else if (IsInvincible)
            return;
        else if (IsDead)
            return;
        else if (IsShield)
        {
            IsShield = false;
            Shield.SetActive(false);
            return;
        }
        else
        {
            for (int i = 0; i < ShieldParts.Length; i++)
            {
                if (ShieldParts[i].gameObject.activeSelf)
                {
                    ShieldParts[i].gameObject.SetActive(false);
                    return;
                }
            }
        }

        CurHP -= damage;

        //DamageText
        GameManager.Inst().TxtManager.ShowDmgText(gameObject.transform.position, damage, (int)TextManager.DamageType.BYENEMY, false);

        //Shake
        GameManager.Inst().ShkManager.Damage();

        GameManager.Inst().SodManager.PlayEffect("Player hit");

        HPUI.SetActive(true);
        HPBar.fillAmount = (float)CurHP / MaxHP * 0.415f;
        Invoke("HideHPUI", 1.0f);

        if (CurHP <= 0)
            Dead();
    }

    public void Heal(int heal)
    {
        if (heal < 1)
            heal = 1;

        CurHP += heal;
        if (CurHP > MaxHP)
            CurHP = MaxHP;

        //DamageText
        GameManager.Inst().TxtManager.ShowDmgText(gameObject.transform.position, heal, (int)TextManager.DamageType.PLAYERHEAL, false);

        HPUI.SetActive(true);
        HPBar.fillAmount = (float)CurHP / MaxHP * 0.415f;
        Invoke("HideHPUI", 1.0f);
    }

    void HideHPUI()
    {
        HPUI.SetActive(false);
    }

    public void Dead()
    {
        if(GameManager.Inst().UpgManager.BData[BulletType].GetIsRevive())
        {
            GameManager.Inst().UpgManager.BData[BulletType].SetIsRevive(false);
            Revive();
            return;
        }

        IsDead = true;

        GetComponent<Animator>().SetInteger("Color", 0);

        DeathTimer = DEATHTIME;
        TimerText.text = ((int)DeathTimer).ToString();
        TimerImage.fillAmount = DeathTimer / DEATHTIME;
        TimerImage.gameObject.SetActive(true);
        InvokeRepeating("CheckDead", 1.0f, 0.1f);
    }

    void CheckDead()
    {
        DeathTimer -= 0.1f;

        TimerImage.fillAmount = DeathTimer / DEATHTIME;
        TimerText.text = ((int)DeathTimer + 1).ToString();

        if(DeathTimer <= 0.0f)
        {
            Revive();
            CancelInvoke("CheckDead");
        }
    }

    public void Revive()
    {
        IsDead = false;
        IsInvincible = true;
        CurHP = MaxHP;

        TimerImage.gameObject.SetActive(false);

        GetComponent<Animator>().SetTrigger("Revive");
        GameObject revive = GameManager.Inst().ObjManager.MakeObj("Revive");
        revive.transform.position = transform.position;

        Invoke("ReturnInvincible", 1.0f);
    }

    void LevelupSound()
    {
        GameManager.Inst().SodManager.PlayEffect("Level up");
    }

    void ReturnColor()
    {
        GetComponent<Animator>().SetInteger("Color", GameManager.Inst().ShtManager.GetColorSelection(2) + 1);
    }

    void ColorFlick()
    {
        int rand = (int)(Time.deltaTime * 100000.0f % 8.0f);
        GetComponent<Animator>().SetInteger("Color", rand);
    }

    void ReturnInvincible()
    {
        IsInvincible = false;
        //CancelInvoke("ColorFlick");
        GetComponent<Animator>().SetInteger("Color", GameManager.Inst().ShtManager.GetColorSelection(2) + 1);
    }

    public void ShieldOn()
    {
        if (IsDead)
            return;

        IsShield = true;
        Shield.SetActive(true);

        GameManager.Inst().SodManager.PlayEffect("Eq_Distort");

        for (int i = 0; i < 4; i++)
        {
            if (GameManager.Inst().GetSubweapons(i) != null)
                GameManager.Inst().GetSubweapons(i).ShowShield();
        }
    }

    public void SetSkin()
    {
        Skin.SetCategoryAndLabel("Skin", Types[BulletType]);
    }

    public void SetSkinColor(int index)
    {
        ColorIndex = index;
        GameManager.Inst().ShtManager.SetColorSelection(2, index);
        GetComponent<Animator>().SetInteger("Color", ++index);
    }

    public void RestoreShield(int amount)
    {
        if (IsDead)
            return;

        for (int i = 0; i < amount; i++)
            ShieldParts[i].gameObject.SetActive(true);
    }

    public void StartAutoShot()
    {
        IsAutoShot = true;
    }

    public void EndAutoShot()
    {
        IsAutoShot = false;
    }
    
    void OnMouseDown()
    {
        if (!GameManager.Inst().IptManager.GetIsAbleControl() || IsMovable)
            return;

        GameManager.Inst().UiManager.MainUI.Bottom.OnClickManageBtn(2);
    }
}


//if (Inventory[i] == null)
//{
//    Inventory[i] = new EqData();
//    Inventory[i].Icon = item.GetIcon();
//    Inventory[i].Type = item.GetEqType();
//    Inventory[i].Rarity = item.GetRarity();
//    Inventory[i].Value = item.GetEqValue();
//    Inventory[i].UID = item.GetUID();

//    return i;
//}