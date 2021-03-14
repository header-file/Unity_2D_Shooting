using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GameData
{
    public enum InvData
    {
        TYPE = 0,
        GRADE = 1,
        VALUE = 2,
        AMOUNT = 3,
        ID = 4,
    }

    public enum SWData
    {
        LEVEL = 0,
        CURHP = 1,
        MAXHP = 2,
        BULLETTYPE = 3,
        COLOR = 4,
    }

    public int Coin;
    public int[] Resources;
    public int CurrentStage;
    public int ReachedStage;
    public int[] SubWeaponDatas;
    public UpgradeManager.BulletData[] Weapons;
    public int[] Inventories;

    public int[] BossGauges;

    public void MakeData()
    {
        //Resources = new int[StageManager.MAXSTAGES];
        //SubWeaponDatas = new int[Constants.MAXSTAGES * Constants.MAXSUBWEAPON * Constants.SWDATASIZE];
        Weapons = new UpgradeManager.BulletData[Constants.MAXBULLETS];
        Inventories = new int[Constants.MAXINVENTORY * Constants.INVDATASIZE];
        //BossGauges = new int[StageManager.MAXSTAGES];

        //for (int i = 0; i < StageManager.MAXSTAGES; i++)
        //{
        //    Resources[i] = 0;
        //    BossGauges[i] = 0;
        //}

        //for (int i = 0; i < GameManager.Inst().Player.MAXINVENTORY; i++)
        //    for(int j = 0; j < INVDATASIZE; j++)
        //        Inventories[i]
    }

    public void SaveData()
    {
        Coin = GameManager.Inst().Player.GetCoin();
        CurrentStage = GameManager.Inst().StgManager.Stage;

        for (int i = 0; i < Constants.MAXSTAGES; i++)
        {
            Resources[i] = GameManager.Inst().Resources[i];
            BossGauges[i] = GameManager.Inst().StgManager.BossCount[i];

            for (int j = 0; j < Constants.MAXSUBWEAPON; j++)
            {
                SubWeaponDatas[Constants.MAXSUBWEAPON * Constants.SWDATASIZE * i + Constants.SWDATASIZE * j + (int)SWData.LEVEL] = GameManager.Inst().UpgManager.GetSubWeaponLevel(i, j);

                if(SubWeaponDatas[Constants.MAXSUBWEAPON * Constants.SWDATASIZE * i + Constants.SWDATASIZE * j + (int)SWData.LEVEL] > 0)
                {
                    SubWeaponDatas[Constants.MAXSUBWEAPON * Constants.SWDATASIZE * i + Constants.SWDATASIZE * j + (int)SWData.CURHP] = GameManager.Inst().GetSubweapons(j).GetCurHP();
                    SubWeaponDatas[Constants.MAXSUBWEAPON * Constants.SWDATASIZE * i + Constants.SWDATASIZE * j + (int)SWData.MAXHP] = GameManager.Inst().GetSubweapons(j).GetMaxHP();
                    SubWeaponDatas[Constants.MAXSUBWEAPON * Constants.SWDATASIZE * i + Constants.SWDATASIZE * j + (int)SWData.BULLETTYPE] = GameManager.Inst().GetSubweapons(j).GetBulletType();
                    int id = j;
                    if (id > 1)
                        id++;
                    SubWeaponDatas[Constants.MAXSUBWEAPON * Constants.SWDATASIZE * i + Constants.SWDATASIZE * j + (int)SWData.COLOR] = GameManager.Inst().ShtManager.GetColorSelection(id);
                }
            }
        }

        for (int i = 0; i < Constants.MAXINVENTORY; i++)
        {
            Player.EqData eq = GameManager.Inst().Player.GetItem(i);
            if (eq != null)
            {
                Inventories[Constants.INVDATASIZE * i + (int)InvData.TYPE] = eq.Type;
                Inventories[Constants.INVDATASIZE * i + (int)InvData.GRADE] = eq.Rarity;
                Inventories[Constants.INVDATASIZE * i + (int)InvData.VALUE] = (int)eq.Value;
                Inventories[Constants.INVDATASIZE * i + (int)InvData.AMOUNT] = eq.Quantity;
                Inventories[Constants.INVDATASIZE * i + (int)InvData.ID] = eq.UID;
            }
        }

        for (int i = 0; i < Constants.MAXBULLETS; i++)
            Weapons[i] = GameManager.Inst().UpgManager.BData[i];
    }

    public void LoadData()
    {
        if (Coin > 0)
            GameManager.Inst().Player.AddCoin(Coin);

        if (GameManager.Inst().StgManager.Stage < CurrentStage)
            GameManager.Inst().StgManager.Stage = CurrentStage;

        if (GameManager.Inst().StgManager.Stage < ReachedStage)
            GameManager.Inst().StgManager.UnlockStages(ReachedStage);

        if (Resources.Length == Constants.MAXSTAGES)
            for (int i = 0; i < Constants.MAXSTAGES; i++)
                GameManager.Inst().Resources[i] = Resources[i];
        else
            Resources = new int[Constants.MAXSTAGES];

        if (BossGauges.Length == Constants.MAXSTAGES)
            for (int i = 0; i < Constants.MAXSTAGES; i++)
                GameManager.Inst().StgManager.BossCount[i] = BossGauges[i];
        else
            BossGauges = new int[Constants.MAXSTAGES];

        //if (SubWeaponDatas.Length == StageManager.MAXSTAGES * 4 * SWDATASIZE)
        if(SubWeaponDatas != null && SubWeaponDatas.Length == Constants.MAXSTAGES * Constants.MAXSUBWEAPON * Constants.SWDATASIZE)
        {
            for (int i = 0; i < Constants.MAXSTAGES; i++)
            {
                for (int j = 0; j < Constants.MAXSUBWEAPON; j++)
                {
                    GameManager.Inst().UpgManager.SetSubWeaponLevel(i, j, SubWeaponDatas[Constants.MAXSUBWEAPON * Constants.SWDATASIZE * i + Constants.SWDATASIZE * j + (int)SWData.LEVEL]);

                    if (i == GameManager.Inst().StgManager.Stage && 
                        GameManager.Inst().UpgManager.GetSubWeaponLevel(i, j) > 0)
                    {
                        GameManager.Inst().UpgManager.AddSW(j);

                        GameManager.Inst().GetSubweapons(j).SetCurHP(SubWeaponDatas[Constants.MAXSUBWEAPON * Constants.SWDATASIZE * i + Constants.SWDATASIZE * j + (int)SWData.CURHP]);
                        GameManager.Inst().GetSubweapons(j).SetMaxHP(SubWeaponDatas[Constants.MAXSUBWEAPON * Constants.SWDATASIZE * i + Constants.SWDATASIZE * j + (int)SWData.MAXHP]);
                        GameManager.Inst().GetSubweapons(j).SetBulletType(SubWeaponDatas[Constants.MAXSUBWEAPON * Constants.SWDATASIZE * i + Constants.SWDATASIZE * j + (int)SWData.BULLETTYPE]);
                        int id = j;
                        if (id > 1)
                            id++;
                        GameManager.Inst().ShtManager.SetColorSelection(id, SubWeaponDatas[Constants.MAXSUBWEAPON * Constants.SWDATASIZE * i + Constants.SWDATASIZE * j + (int)SWData.COLOR]);
                        GameManager.Inst().GetSubweapons(j).SetSkinColor(SubWeaponDatas[Constants.MAXSUBWEAPON * Constants.SWDATASIZE * i + Constants.SWDATASIZE * j + (int)SWData.COLOR]);

                        GameManager.Inst().UpgManager.SWUiInteract(j);
                    }
                }
            }
        }
        else
            SubWeaponDatas = new int[Constants.MAXSTAGES * Constants.MAXSUBWEAPON * Constants.SWDATASIZE];

        if (Inventories != null && Inventories.Length == Constants.MAXINVENTORY)
        {
            for (int i = 0; i < Constants.MAXINVENTORY; i++)
            {
                if (Inventories[Constants.INVDATASIZE * i + (int)InvData.VALUE] > 0)
                {
                    Player.EqData eq = new Player.EqData();
                    eq.Type = Inventories[Constants.INVDATASIZE * i + (int)InvData.TYPE];
                    eq.Rarity = Inventories[Constants.INVDATASIZE * i + (int)InvData.GRADE];
                    eq.Value = Inventories[Constants.INVDATASIZE * i + (int)InvData.VALUE];
                    eq.Quantity = Inventories[Constants.INVDATASIZE * i + (int)InvData.AMOUNT];
                    eq.UID = Inventories[Constants.INVDATASIZE * i + (int)InvData.ID];
                    eq.Icon = GameManager.Inst().UiManager.FoodImages[eq.Type];

                    GameManager.Inst().Player.AddItem(eq);
                }
            }
        }
        else
            Inventories = new int[Constants.MAXINVENTORY * Constants.INVDATASIZE];
        

        if (Weapons != null)
        {
            for (int i = 0; i < Constants.MAXBULLETS; i++)
                GameManager.Inst().UpgManager.BData[i] = Weapons[i];
        }
        else
            Weapons = new UpgradeManager.BulletData[Constants.MAXBULLETS];
    }
}


//public struct InventoryData
//{
//    int Type;
//    int Grade;
//    int Value;
//    int Amount;

//    public int GetItemType() { return Type; }
//    public int GetGrade() { return Grade; }
//    public int GetValue() { return Value; }
//    public int GetAmount() { return Amount; }

//    public void SetData(int type, int grade, int value, int amount)
//    {
//        Type = type;
//        Grade = grade;
//        Value = value;
//        Amount = amount;
//    }
//}

//public struct SubWeaponData
//{
//    int Level;
//    int CurHP;
//    int MaxHP;
//    int BulletType;

//    public int GetLevel() { return Level; }
//    public int GetCurHP() { return CurHP; }
//    public int GetMaxHP() { return MaxHP; }
//    public int GetBulletType() { return BulletType; }

//    public void SetData(int level, int curHP, int maxHP, int bType)
//    {
//        Level = level;
//        CurHP = curHP;
//        MaxHP = maxHP;
//        BulletType = bType;
//    }
//}