﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GameData
{
    public enum PlaData
    {
        LEVEL = 0,
        CURHP = 1,
        MAXHP = 2,
        BULLETTYPE = 3,
        COLOR = 4,
    }

    public enum InvData
    {
        TYPE = 0,
        GRADE = 1,
        VALUE = 2,
        AMOUNT = 3,
        ID = 4,
        COOLTIME = 5,
    }

    public enum SWData
    {
        LEVEL = 0,
        CURHP = 1,
        MAXHP = 2,
        BULLETTYPE = 3,
        COLOR = 4,
    }

    public enum WPData
    {
        POWERLEVEL = 0,
        BASEDAMAGE = 1,
        RARITY = 2,
        PRICE = 3,
        SPEED = 4,
        RELOAD = 5,
        DURATION = 6,
        ATK = 7,
        HP = 8,
        SPD = 9,
        EQUIP = 10,
    }

    public enum QSTData
    {
        ID = 0,
        COUNT = 1,
    }

    public enum TIMEData
    {
        YEAR = 0,
        MONTH = 1,
        DATE = 2,
        HOUR = 3,
        MINUTE = 4,
        SECOND = 5,
    }

    public int Coin;
    public int[] Resources;

    public int MaxInventory;

    public int CurrentStage;
    public int ReachedStage;

    public int[] PlayerDatas;
    public int[] SubWeaponDatas;
    public float[] Weapons;

    public int[] Inventories;

    public int[] Quests;

    public int[] BossGauges;

    public int[] CountStartTimes;

    public bool IsEraseData;


    public void SaveData()
    {
        if (IsEraseData)
            return;

        Coin = GameManager.Inst().Player.GetCoin();
        CurrentStage = GameManager.Inst().StgManager.Stage;

        MaxInventory = GameManager.Inst().Player.MaxInventory;

        PlayerDatas[(int)PlaData.LEVEL] = GameManager.Inst().UpgManager.GetPlayerLevel();
        PlayerDatas[(int)PlaData.CURHP] = GameManager.Inst().Player.GetCurHP();
        PlayerDatas[(int)PlaData.MAXHP] = GameManager.Inst().Player.GetMaxHP();
        PlayerDatas[(int)PlaData.BULLETTYPE] = GameManager.Inst().Player.GetBulletType();
        PlayerDatas[(int)PlaData.COLOR] = GameManager.Inst().ShtManager.GetColorSelection(2);

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

            //for(int j = 0; j < Constants.TIMEDATASIZE; j++)
            {
                CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.YEAR] = GameManager.Inst().ResManager.StartTimes[i].Year;
                CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.MONTH] = GameManager.Inst().ResManager.StartTimes[i].Month;
                CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.DATE] = GameManager.Inst().ResManager.StartTimes[i].Day;
                CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.HOUR] = GameManager.Inst().ResManager.StartTimes[i].Hour;
                CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.MINUTE] = GameManager.Inst().ResManager.StartTimes[i].Minute;
                CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.SECOND] = GameManager.Inst().ResManager.StartTimes[i].Second;
            }
        }

        Inventories = new int[Constants.INVDATASIZE * GameManager.Inst().Player.MaxInventory];
        for (int i = 0; i < GameManager.Inst().Player.MaxInventory; i++)
        {
            Player.EqData eq = GameManager.Inst().Player.GetItem(i);
            if (eq != null)
            {
                Inventories[Constants.INVDATASIZE * i + (int)InvData.TYPE] = eq.Type;
                Inventories[Constants.INVDATASIZE * i + (int)InvData.GRADE] = eq.Rarity;
                Inventories[Constants.INVDATASIZE * i + (int)InvData.VALUE] = (int)eq.Value;
                Inventories[Constants.INVDATASIZE * i + (int)InvData.AMOUNT] = eq.Quantity;
                Inventories[Constants.INVDATASIZE * i + (int)InvData.ID] = eq.UID;
                Inventories[Constants.INVDATASIZE * i + (int)InvData.COOLTIME] = (int)eq.CoolTime;
            }
        }

        for (int i = 0; i < Constants.MAXBULLETS; i++)
        {
            Weapons[Constants.WPDATASIZE * i + (int)WPData.POWERLEVEL] = GameManager.Inst().UpgManager.BData[i].GetPowerLevel();
            Weapons[Constants.WPDATASIZE * i + (int)WPData.BASEDAMAGE] = GameManager.Inst().UpgManager.BData[i].GetBaseDamage();
            Weapons[Constants.WPDATASIZE * i + (int)WPData.RARITY] = GameManager.Inst().UpgManager.BData[i].GetRarity();
            Weapons[Constants.WPDATASIZE * i + (int)WPData.PRICE] = GameManager.Inst().UpgManager.BData[i].GetPrice();
            Weapons[Constants.WPDATASIZE * i + (int)WPData.SPEED] = GameManager.Inst().UpgManager.BData[i].GetSpeed();
            Weapons[Constants.WPDATASIZE * i + (int)WPData.RELOAD] = GameManager.Inst().UpgManager.BData[i].GetReloadTime();
            Weapons[Constants.WPDATASIZE * i + (int)WPData.DURATION] = GameManager.Inst().UpgManager.BData[i].GetDuration();
            Weapons[Constants.WPDATASIZE * i + (int)WPData.ATK] = GameManager.Inst().UpgManager.BData[i].GetAtk();
            Weapons[Constants.WPDATASIZE * i + (int)WPData.HP] = GameManager.Inst().UpgManager.BData[i].GetHp();
            Weapons[Constants.WPDATASIZE * i + (int)WPData.SPD] = GameManager.Inst().UpgManager.BData[i].GetSpd();
            Weapons[Constants.WPDATASIZE * i + (int)WPData.EQUIP] = GameManager.Inst().UpgManager.BData[i].GetEquipIndex();
        }

        for(int i = 0; i < Constants.MAXSTAGES * Constants.MAXQUESTS; i++)
        {
            Quests[Constants.QSTDATASIZE * i + (int)QSTData.ID] = GameManager.Inst().QstManager.Quests[i].QuestId;
            Quests[Constants.QSTDATASIZE * i + (int)QSTData.COUNT] = GameManager.Inst().QstManager.Quests[i].CurrentCount;
        }
    }

    public void LoadData()
    {
        if (IsEraseData)
        {
            IsEraseData = false;
            return;
        }

        if (Coin > 0)
            GameManager.Inst().Player.AddCoin(Coin);

        if (MaxInventory > 0)
        {
            if(MaxInventory > Constants.MININVENTORY)
            {
                int count = (MaxInventory - Constants.MININVENTORY) / 10;
                for (int i = 0; i < count; i++)
                {
                    GameManager.Inst().Player.MaxInventory += 10;
                    GameManager.Inst().AddInventory();
                }
            }
        }

        if (GameManager.Inst().StgManager.Stage < CurrentStage)
            GameManager.Inst().StgManager.Stage = CurrentStage;

        if (GameManager.Inst().StgManager.Stage < ReachedStage)
            GameManager.Inst().StgManager.UnlockStages(ReachedStage);

        if(ReachedStage > 1)
        {
            for (int i = 0; i < ReachedStage; i++)
                GameManager.Inst().ResManager.StartCount(i);
        }

        if (Resources.Length == Constants.MAXSTAGES)
            for (int i = 0; i < Constants.MAXSTAGES; i++)
                GameManager.Inst().Resources[i] = Resources[i];
        else
            Resources = new int[Constants.MAXSTAGES];

        if (BossGauges.Length == Constants.MAXSTAGES)
            for (int i = 0; i < Constants.MAXSTAGES; i++)
            {
                GameManager.Inst().StgManager.BossCount[i] = BossGauges[i];

                if (i == GameManager.Inst().StgManager.Stage)
                    GameManager.Inst().StgManager.FillGauge();
            }
                
        else
            BossGauges = new int[Constants.MAXSTAGES];

        if (PlayerDatas != null && PlayerDatas.Length == Constants.PLADATASIZE)
        {
            GameManager.Inst().UpgManager.SetPlayerLevel(PlayerDatas[(int)PlaData.LEVEL]);
            GameManager.Inst().Player.SetCurHP(PlayerDatas[(int)PlaData.CURHP]);
            GameManager.Inst().Player.SetMaxHP(PlayerDatas[(int)PlaData.MAXHP]);
            GameManager.Inst().Player.SetBulletType(PlayerDatas[(int)PlaData.BULLETTYPE]);
            GameManager.Inst().Player.SetSkinColor(PlayerDatas[(int)PlaData.COLOR]);
        }
        else
            PlayerDatas = new int[Constants.PLADATASIZE];

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

        if (Inventories != null && Inventories.Length == GameManager.Inst().Player.MaxInventory * Constants.INVDATASIZE)
        {
            for (int i = 0; i < GameManager.Inst().Player.MaxInventory; i++)
            {
                if (Inventories[Constants.INVDATASIZE * i + (int)InvData.ID] > 0)
                {
                    Player.EqData eq = new Player.EqData();
                    eq.Type = Inventories[Constants.INVDATASIZE * i + (int)InvData.TYPE];
                    eq.Rarity = Inventories[Constants.INVDATASIZE * i + (int)InvData.GRADE];
                    eq.Value = Inventories[Constants.INVDATASIZE * i + (int)InvData.VALUE];
                    eq.Quantity = Inventories[Constants.INVDATASIZE * i + (int)InvData.AMOUNT];
                    eq.UID = Inventories[Constants.INVDATASIZE * i + (int)InvData.ID];
                    eq.CoolTime = Inventories[Constants.INVDATASIZE * i + (int)InvData.COOLTIME];
                                        
                    if(eq.UID / 100 == 3)
                        eq.Icon = GameManager.Inst().UiManager.FoodImages[eq.Type + eq.Rarity * Constants.MAXREINFORCETYPE];
                    else if (eq.UID / 100 == 6)
                        eq.Icon = GameManager.Inst().UiManager.EquipImages[eq.Type];

                    GameManager.Inst().Player.AddItem(eq);
                }
            }
        }
        else
            Inventories = new int[GameManager.Inst().Player.MaxInventory * Constants.INVDATASIZE];
        

        if (Weapons != null)
        {
            for (int i = 0; i < Constants.MAXBULLETS; i++)
            {
                GameManager.Inst().UpgManager.BData[i].SetRarity((int)Weapons[Constants.WPDATASIZE * i + (int)WPData.RARITY]);
                GameManager.Inst().UpgManager.BData[i].SetPowerLevel((int)Weapons[Constants.WPDATASIZE * i + (int)WPData.POWERLEVEL]);
                GameManager.Inst().UpgManager.BData[i].SetBaseDamage((int)Weapons[Constants.WPDATASIZE * i + (int)WPData.BASEDAMAGE]);
                GameManager.Inst().UpgManager.BData[i].SetPrice((int)Weapons[Constants.WPDATASIZE * i + (int)WPData.PRICE]);
                GameManager.Inst().UpgManager.BData[i].SetSpeed(Weapons[Constants.WPDATASIZE * i + (int)WPData.SPEED]);
                GameManager.Inst().UpgManager.BData[i].SetReloadTime(Weapons[Constants.WPDATASIZE * i + (int)WPData.RELOAD]);
                GameManager.Inst().UpgManager.BData[i].SetDuration(Weapons[Constants.WPDATASIZE * i + (int)WPData.DURATION]);
                GameManager.Inst().UpgManager.BData[i].SetAtk((int)Weapons[Constants.WPDATASIZE * i + (int)WPData.ATK]);
                GameManager.Inst().UpgManager.BData[i].SetHp((int)Weapons[Constants.WPDATASIZE * i + (int)WPData.HP]);
                GameManager.Inst().UpgManager.BData[i].SetSpd((int)Weapons[Constants.WPDATASIZE * i + (int)WPData.SPD]);
                GameManager.Inst().UpgManager.BData[i].SetEquipIndex((int)Weapons[Constants.WPDATASIZE * i + (int)WPData.EQUIP]);

                GameManager.Inst().UpgManager.CheckEquip(i);
                GameManager.Inst().UpgManager.SetMaxData(i);
            }
        }
        else
            Weapons = new float[Constants.MAXBULLETS * Constants.WPDATASIZE];

        if (Quests != null)
        {
            for(int i = 0; i < Constants.MAXSTAGES * Constants.MAXQUESTS; i++)
            {
                if(GameManager.Inst().QstManager.Quests[i].QuestId == Quests[Constants.QSTDATASIZE * i + (int)QSTData.ID])
                {
                    GameManager.Inst().QstManager.Quests[i].CurrentCount = Quests[Constants.QSTDATASIZE * i + (int)QSTData.COUNT];
                    GameManager.Inst().QstManager.CheckFinish(i);
                }
            }
        }
        else
            Quests = new int[Constants.MAXSTAGES * Constants.MAXQUESTS * Constants.QSTDATASIZE];

        if (CountStartTimes != null)
            for (int i = 0; i < Constants.MAXSTAGES; i++)
            {
                string str = "";
                str += CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.YEAR].ToString() + "/";
                str += CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.MONTH].ToString() + "/";
                str += CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.DATE].ToString() + " ";
                str += CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.HOUR].ToString() + ":";
                str += CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.MINUTE].ToString() + ":";
                str += CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.SECOND].ToString();

                GameManager.Inst().ResManager.StartTimes[i] = Convert.ToDateTime(str);
                GameManager.Inst().ResManager.LoadCount(i);
            }
        else
            CountStartTimes = new int[Constants.MAXSTAGES * Constants.TIMEDATASIZE];
    }

    public void ResetData()
    {
        Coin = 0;
        Resources = new int[Constants.MAXSTAGES];
        MaxInventory = Constants.MININVENTORY;

        CurrentStage = 1;
        ReachedStage = 1;
        BossGauges = new int[Constants.MAXSTAGES];

        SubWeaponDatas = new int[Constants.MAXSTAGES * Constants.MAXSUBWEAPON * Constants.SWDATASIZE];

        Inventories = new int[GameManager.Inst().Player.MaxInventory * Constants.INVDATASIZE];

        Weapons = new float[Constants.MAXBULLETS * Constants.WPDATASIZE];

        Quests = new int[Constants.MAXSTAGES * Constants.MAXQUESTS * Constants.QSTDATASIZE];

        CountStartTimes = new int[Constants.MAXSTAGES * Constants.TIMEDATASIZE];
    }

    public void LoadReachedStage()
    {
        if (GameManager.Inst().StgManager.ReachedStage < ReachedStage)
            GameManager.Inst().StgManager.ReachedStage = ReachedStage;
    }
}