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
    public int SortOption;
    public int ShootCount;

    //Cheat
    public bool IsGodMode;

    GameObject[] SubWeapons;
    EqData[] Inventory;
    Vector3 OriginalPos;
    CanvasGroup canvasGroup;
    

    Vector3 PlayerPos;
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
    

    public GameObject GetSubWeapon(int index) { return SubWeapons[index]; }
    public GameObject GetChargePos() { return ChargePos; }
    public EqData GetItem(int index) { return Inventory[index] != null ? Inventory[index] : null; }

    public int GetCoin() { return Coin; }
    public int GetBulletType() { return BulletType; }
    public bool GetIsMovable() { return IsMovable; }
    public int GetMaxHP() { return MaxHP; }
    public int GetCurHP() { return CurHP; }


    public void SetMaxHP(int hp) { MaxHP = hp; }
    public void SetCurHP(int hp) { CurHP = hp; }
    public void SetSubWeapon(GameObject obj, int index) { SubWeapons[index] = obj; }
    public void SetBulletType(int type)
    {
        BulletType = type;
        GameManager.Inst().UpgManager.SetHPData(BulletType);
        SetSkin();

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

        gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
        //Booster.SetActive(true);
        //Booster.GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", Color.red);
        //Booster.transform.GetChild(0).GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", Color.yellow);

        for (int i = 0; i < 4; i++)
        {
            if (GameManager.Inst().GetSubweapons(i) != null)
                GameManager.Inst().GetSubweapons(i).BossMode();
        }
    }

    public void EndBossMode()
    {
        IsMovable = false;

        InvokeRepeating("MoveBack", 0.0f, Time.deltaTime);

        for (int i = 0; i < 4; i++)
        {
            if (GameManager.Inst().GetSubweapons(i) != null)
                GameManager.Inst().GetSubweapons(i).EndBossMode();
        }
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
        for (int i = 0; i < MaxInventory; i++)
        {
            if (Inventory[i] != null)
            {
                if (Inventory[i].UID == item.GetUID())
                {
                    Inventory[i].Quantity++;

                    return i;
                }
            }
        }

        for (int i = 0; i < MaxInventory; i++)
        {
            if (Inventory[i] == null)
            {
                Inventory[i] = new EqData();
                Inventory[i].Icon = item.GetIcon();
                Inventory[i].Type = item.GetEqType();
                Inventory[i].Rarity = item.GetRarity();
                Inventory[i].Value = item.GetEqValue();
                Inventory[i].UID = item.GetUID();
                Inventory[i].Quantity = 1;
                Inventory[i].CoolTime = 0.0f;

                return i;
            }
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

                return i;
            }
        }

        return -1;
    }

    public int AddItem(EqData item)
    {
        for (int i = 0; i < MaxInventory; i++)
        {
            if (Inventory[i] != null)
            {
                if (Inventory[i].UID == item.UID)
                {
                    if (item.UID / 100 == 6)
                        continue;

                    Inventory[i].Quantity++;
                    return i;
                }
            }
        }

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

                return i;
            }
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
    }

    public EqData DiscardItem(int index)
    {
        EqData e = new EqData();
        e.Icon = Inventory[index].Icon;
        e.Type = Inventory[index].Type;
        e.Rarity = Inventory[index].Rarity;
        e.Value = Inventory[index].Value;

        Inventory[index] = null;

        return e;
    }

    //public void DragItem(int count)
    //{
    //    for (int n = 1; n <= count; n++)
    //    {
    //        for (int i = 0; i < MaxInventory; i++)
    //        {
    //            if (Inventory[i] == null)
    //                continue;
    //            else
    //            {
    //                InventoryScroll inven = GameManager.Inst().UiManager.InventoryScroll.GetComponent<InventoryScroll>();
    //                for (int j = i; j > 0; j--)
    //                {
    //                    if (Inventory[j - 1] == null)
    //                    {
    //                        Swap(j - 1, j);
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

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

        SubWeapons = new GameObject[4];
        Inventory = new EqData[Constants.MAXINVENTORY];
        for (int i = 0; i < Constants.MAXINVENTORY; i++)
            Inventory[i] = null;

        IsMovable = false;
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
    }

    void Start()
    {
        GameManager.Inst().UiManager.SetCoinText(Coin);

        UISetting();
    }

    void Update()
    {
        SetUIPos();

        EquipCount();
    }

    void SetUIPos()
    {
        PlayerPos = transform.position;
        PlayerPos.y += 3.0f;
        PlayerPos.z = 90.0f;
        UI.transform.position = PlayerPos;
    }

    void EquipCount()
    {
        if (IsDead || GameManager.Inst().UpgManager.BData[BulletType].GetEquipIndex() == -1 || CheckPassive())
            return;

        GameManager.Inst().EquManager.Count(gameObject, GameManager.Inst().UpgManager.BData[BulletType].GetEquipIndex(), 2);

        if (EquipUI.activeSelf)
            SetEquipUI();
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
            //Booster.SetActive(false);
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
        Invoke("ReturnInvincible", 1.0f);
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
        IsShield = true;
        Shield.SetActive(true);

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
        GetComponent<Animator>().SetInteger("Color", ++index);
    }

    public void RestoreShield(int amount)
    {
        for (int i = 0; i < amount; i++)
            ShieldParts[i].gameObject.SetActive(true);
    }
    
    void OnMouseDown()
    {
        if (!GameManager.Inst().IptManager.GetIsAbleControl() || IsMovable)
            return;

        GameManager.Inst().UiManager.OnClickManageBtn(2);
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