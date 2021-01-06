using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Player : MonoBehaviour
{
    public int MAXINVENTORY = 40;

    public class EqData //: IComparable
    {
        public Sprite Icon;
        public int Type;
        public int Rarity;
        public float Value;
        public int UID;

        
        public EqData()
        {
            Icon = null;
            Type = -1;
            Rarity = -1;
            Value = 0.0f;
            UID = 0;
        }
        
    };

    public GameObject[] NormalPos;
    public GameObject[] SpreadPos;
    public GameObject LaserPos;
    public GameObject ChargePos;
    public GameObject[] BoomerangPos;

    public int InputGrade;

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
        for(int i = 0; i < MAXINVENTORY; i++)
        {
            if (Inventory[i] == null)
            {
                Inventory[i] = new EqData();
                Inventory[i].Icon = item.GetIcon();
                Inventory[i].Type = item.GetEqType();
                Inventory[i].Rarity = item.GetRarity();
                Inventory[i].Value = item.GetEqValue();
                Inventory[i].UID = item.GetUID();

                return i;
            }   
        }

        return -1;
    }

    public int AddItem(EqData item)
    {
        for (int i = 0; i < MAXINVENTORY; i++)
        {
            if (Inventory[i] == null)
            {
                Inventory[i] = new EqData();
                Inventory[i].Icon = item.Icon;
                Inventory[i].Type = item.Type;
                Inventory[i].Rarity = item.Rarity;
                Inventory[i].Value = item.Value;
                Inventory[i].UID = item.UID;

                return i;
            }
        }
        
        return -1;
    }

    //public void Sort()
    //{
    //    InputGrade = -1;
    //    Array.Sort(Inventory);
    //    Array.Reverse(Inventory);
    //}

    //public void Sort(int Rarity)
    //{
    //    //for(int i = 0; i < MAXINVENTORY; i++)
    //    //{
    //    //    if (Inventory[i] == null)
    //    //        return;

    //    //    if(Inventory[i].Rarity == Rarity)
    //    //    {
    //    //        for(int j = i; j > 0; j--)
    //    //        {
    //    //            if(Inventory[j - 1].Rarity != Rarity)
    //    //                Swap(Inventory[j], Inventory[j - 1], j);
    //    //        }

    //    //    }
    //    //}

    //    InputGrade = Rarity;
    //    Array.Sort(Inventory);
    //    Array.Reverse(Inventory);
    //}

    void Swap(int i, int j)
    {
        if (Inventory[i] == null)
        {
            Inventory[i] = Inventory[j];
            Inventory[j] = null;
        }
        else if(Inventory[j] == null)
        {
            Inventory[j] = Inventory[i];
            Inventory[i] = null;
        }
        else if(Inventory[i] != null && Inventory[j] != null)
        {
            EqData temp = new EqData();
            temp = Inventory[i];
            Inventory[i] = Inventory[j];
            Inventory[j] = temp;
        }

        //temp.Icon = a.Icon;
        //temp.Type = a.Type;
        //temp.Rarity = a.Rarity;
        //temp.Value = a.Value;

        //a.Icon = b.Icon;
        //a.Type = b.Type;
        //a.Rarity = b.Rarity;
        //a.Value = b.Value;

        //b.Icon = temp.Icon;
        //b.Type = temp.Type;
        //b.Rarity = temp.Rarity;
        //b.Value = temp.Value;
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

    public void DragItem(int count)
    {
        for (int n = 1; n <= count; n++)
        {
            for (int i = 0; i < MAXINVENTORY; i++)
            {
                if (Inventory[i] == null)
                    continue;
                else 
                {
                    InventoryScroll inven = GameManager.Inst().Inventory.GetComponent<InventoryScroll>();
                    for (int j = i; j > 0; j--)
                    {
                        if(Inventory[j - 1] == null)
                        {
                            Swap(j - 1, j);

                            //inven.MoveFront(j);
                            //InventorySlot slot = GameManager.Inst().Inventory.GetComponent<InventoryScroll>().GetSlot(j);
                            //if (slot.Selected.activeSelf)
                            //{
                            //    slot.Selected.SetActive(false);
                            //    GameManager.Inst().Inventory.GetComponent<InventoryScroll>().GetSlot(j - 1).Selected.SetActive(true);
                            //}
                            //if (slot.EMark.activeSelf)
                            //{
                            //    slot.EMark.SetActive(false);
                            //    GameManager.Inst().Inventory.GetComponent<InventoryScroll>().GetSlot(j - 1).EMark.SetActive(true);
                            //}
                        }
                    }

                    //Inventory[MAXINVENTORY] = null;
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
        Inventory = new EqData[MAXINVENTORY];
        for (int i = 0; i < MAXINVENTORY; i++)
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
        if (!GameManager.Inst().IptManager.GetIsAbleControl())
            return;

        GameManager.Inst().UiManager.OnClickManageBtn(2);
    }
}
