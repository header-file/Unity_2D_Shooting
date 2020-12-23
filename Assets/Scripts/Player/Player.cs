﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public int MAXINVENTORY = 40;

    public class EqData
    {
        public Sprite Icon;
        public int Type;
        public int Rarity;
        public float Value;

        public EqData()
        {
            Icon = null;
            Type = -1;
            Rarity = -1;
            Value = 0.0f;
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
    public EqData GetItem(int index) { return Inventory[index] != null ? Inventory[index] : null; }

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

    public int AddItem(Item_Equipment item)
    {
        for(int i = 1; i <= MAXINVENTORY; i++)
        {
            if (Inventory[i] == null)
            {
                Inventory[i] = new EqData();
                Inventory[i].Icon = item.GetIcon();
                Inventory[i].Type = item.GetEqType();
                Inventory[i].Rarity = item.GetRarity();
                Inventory[i].Value = item.GetEqValue();

                return i;
            }   
        }

        return -1;
    }

    public void RemoveItem(int index)
    {
        Inventory[index] = null;
    }

    public void DragItem(int count)
    {
        for (int n = 1; n <= count; n++)
        {
            for (int i = 1; i < MAXINVENTORY; i++)
            {
                if (Inventory[i] != null)
                    continue;
                else 
                {
                    for (int j = i; j < MAXINVENTORY; j++)
                        Inventory[j] = Inventory[j + 1];

                    Inventory[MAXINVENTORY] = null;
                }
                
            }
        }
    }

    void Awake()
    {
        Coin = 9999999;
        
        IsReload = true;
        BulletType = 0;

        SubWeapons = new GameObject[4];
        MAXINVENTORY = 40;
        Inventory = new EqData[MAXINVENTORY + 1];
        for (int i = 1; i <= MAXINVENTORY; i++)
            Inventory[i] = null;
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
