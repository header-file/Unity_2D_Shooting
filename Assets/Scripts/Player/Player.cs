using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public int MAXINVENTORY = 40;

    public struct EqData
    {
        public Sprite Icon;
        public int Type;
        public int Rarity;
        public float Value;

        public EqData(Sprite icon, int type, int rarity, float value)
        {
            Icon = icon;
            Type = type;
            Rarity = rarity;
            Value = value;
        }
    };

    public GameObject[] NormalPos;
    public GameObject[] SpreadPos;
    public GameObject LaserPos;
    public GameObject ChargePos;
    public GameObject[] BoomerangPos;

    GameObject[] SubWeapons;
    EqData[] Inventory;

    bool IsReload;
    int Coin;
    int BulletType;

    public GameObject GetSubWeapon(int index) { return SubWeapons[index]; }
    public GameObject GetChargePos() { return ChargePos; }
    public EqData GetItem(int index) { return Inventory[index];/* != null ? Inventory[index] : null; */}

    public int GetCoin() { return Coin; }
    public int GetBulletType() { return BulletType; }

    public void SetSubWeapon(GameObject obj, int index) { SubWeapons[index] = obj; }
    public void SetBulletType(int type) { BulletType = type; }

    public void AddCoin(int c)
    {
        Coin += c;

        GameManager.Inst().SetCoinText(Coin);
    }

    public void MinusCoin(int c)
    {
        Coin -= c;

        GameManager.Inst().SetCoinText(Coin);
    }

    public void AddItem(Item_Equipment item)
    {
        for(int i = 0; i < MAXINVENTORY; i++)
        {
            if (Inventory[i].Icon == null)
            {
                Inventory[i].Icon = item.GetIcon();
                Inventory[i].Type = item.GetEqType();
                Inventory[i].Rarity = item.GetRarity();
                Inventory[i].Value = item.GetEqValue();

                break;
            }   
        }
    }

    public void RemoveItem(int index)
    {
        for (int i = index + 1; i < MAXINVENTORY; i++)
        {
            if (Inventory[i].Icon == null)
            {
                Inventory[i] = new EqData();
                break;
            }
                

            Inventory[i - 1] = Inventory[i];
        }
    }

    void Awake()
    {
        Coin = 9999999;
        
        IsReload = true;
        BulletType = 0;

        SubWeapons = new GameObject[4];
        Inventory = new EqData[MAXINVENTORY];
        for (int i = 0; i < MAXINVENTORY; i++)
            Inventory[i] = new EqData(null, -1, -1, 0.0f);
    }

    void Start()
    {
        GameManager.Inst().SetCoinText(Coin);
    }

    void Update()
    {
        /*if (Input.GetMouseButtonUp(1))
        {
            BulletType++;
            //IsReload = true;
            
            if (BulletType >= 5)
                BulletType = 0;
            Debug.Log(BulletType);
        }*/
    }

    public void Rotate(Vector2 MousePos)
    {
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
        if(!IsReload)
            return;
        
        IsReload = false;
        
        GameManager.Inst().ShtManager.Shoot((Bullet.BulletType)BulletType, gameObject, 2);

        Invoke("Reload", GameManager.Inst().UpgManager.GetBData(BulletType).GetReloadTime());
    }

    void Reload()
    {
        IsReload = true;
    }

    void OnMouseDown()
    {
        GameManager.Inst().UiManager.OnClickManageBtn(2);
    }
}
