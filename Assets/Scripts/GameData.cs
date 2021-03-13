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
    }

    public enum SWData
    {
        LEVEL = 0,
        CURHP = 1,
        MAXHP = 2,
        BULLETTYPE = 3,
    }

    static int INVDATASIZE = 4;
    static int SWDATASIZE = 4;

    public int Coin;
    public int[] Resources;
    public int CurrentStage;
    public int ReachedStage;
    public int[,,] SubWeaponDatas;
    public UpgradeManager.BulletData[] Weapons;
    public int[,] Inventories;

    public int[] BossGauges;

    public void MakeData()
    {
        //Resources = new int[StageManager.MAXSTAGES];
        SubWeaponDatas = new int[StageManager.MAXSTAGES, 4, SWDATASIZE];
        Weapons = new UpgradeManager.BulletData[Bullet.MAXBULLETS];
        Inventories = new int[GameManager.Inst().Player.MAXINVENTORY, INVDATASIZE];
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

        for (int i = 0; i < StageManager.MAXSTAGES; i++)
        {
            Resources[i] = GameManager.Inst().Resources[i];
            BossGauges[i] = GameManager.Inst().StgManager.BossCount[i];

            for (int j = 0; j < 4; j++)
            {
                SubWeaponDatas[i, j, (int)SWData.LEVEL] = GameManager.Inst().UpgManager.GetSubWeaponLevel(i, j);

                if(SubWeaponDatas[i, j, (int)SWData.LEVEL] > 0)
                {
                    SubWeaponDatas[i, j, (int)SWData.CURHP] = GameManager.Inst().GetSubweapons(j).GetCurHP();
                    SubWeaponDatas[i, j, (int)SWData.MAXHP] = GameManager.Inst().GetSubweapons(j).GetMaxHP();
                    SubWeaponDatas[i, j, (int)SWData.BULLETTYPE] = GameManager.Inst().GetSubweapons(j).GetBulletType();
                }
            }
        }

        for(int i = 0; i < GameManager.Inst().Player.MAXINVENTORY; i++)
        {
            Player.EqData eq = GameManager.Inst().Player.GetItem(i);
            if(eq != null)
            {
                Inventories[i, (int)InvData.TYPE] = eq.Type;
                Inventories[i, (int)InvData.GRADE] = eq.Rarity;
                Inventories[i, (int)InvData.VALUE] = (int)eq.Value;
                Inventories[i, (int)InvData.TYPE] = eq.Quantity;
            }
        }

        for (int i = 0; i < Bullet.MAXBULLETS; i++)
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

        if (Resources.Length == StageManager.MAXSTAGES)
            for (int i = 0; i < StageManager.MAXSTAGES; i++)
                GameManager.Inst().Resources[i] = Resources[i];
        else
            Resources = new int[StageManager.MAXSTAGES];

        if (BossGauges.Length == StageManager.MAXSTAGES)
            for (int i = 0; i < StageManager.MAXSTAGES; i++)
                GameManager.Inst().StgManager.BossCount[i] = BossGauges[i];
        else
            BossGauges = new int[StageManager.MAXSTAGES];

        //if (SubWeaponDatas.Length == StageManager.MAXSTAGES * 4 * SWDATASIZE)
        if(SubWeaponDatas != null)
        {
            for (int i = 0; i < StageManager.MAXSTAGES; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    GameManager.Inst().UpgManager.SetSubWeaponLevel(i, j, SubWeaponDatas[i, j, (int)SWData.LEVEL]);

                    if (i == GameManager.Inst().StgManager.Stage && 
                        GameManager.Inst().UpgManager.GetSubWeaponLevel(i, j) > 0)
                    {
                        GameManager.Inst().UpgManager.AddSW(j);

                        GameManager.Inst().GetSubweapons(j).SetCurHP(SubWeaponDatas[i, j, (int)SWData.CURHP]);
                        GameManager.Inst().GetSubweapons(j).SetMaxHP(SubWeaponDatas[i, j, (int)SWData.MAXHP]);
                        GameManager.Inst().GetSubweapons(j).SetBulletType(SubWeaponDatas[i, j, (int)SWData.BULLETTYPE]);
                    }
                }
            }
        }
        else
            SubWeaponDatas = new int[StageManager.MAXSTAGES, 4, SWDATASIZE];

        if(Weapons != null)
        {
            for (int i = 0; i < Bullet.MAXBULLETS; i++)
                GameManager.Inst().UpgManager.BData[i] = Weapons[i];
        }
        else
            Weapons = new UpgradeManager.BulletData[Bullet.MAXBULLETS];
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