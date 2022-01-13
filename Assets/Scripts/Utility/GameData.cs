using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static System.DateTime;
using UnityEngine.SceneManagement;

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
        RARITY = 1,
        PRICE = 2,
        ATK = 3,
        HP = 4,
        SPD = 5,
        EQUIP = 6,
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

    public string UID;
    public bool IsFullPrice;

    public int Coin;
    public int Jewel;
    public int[] Resources;

    public bool IsDaily;
    public int DailyLeft;
    public bool IsDailyPlus;
    public int DailyPlusLeft;
    public int[] LastDailyTime;

    public int AdLeft;
    public int[] LastAdTime;

    public int BuffAdLeft;
    public int[] LastBuffAdTime;
    public int[] BuffLefts;

    public int MaxInventory;

    public int CurrentStage;
    public int ReachedStage;

    public int[] PlayerDatas;
    public int[] SubWeaponDatas;
    public float[] Weapons;

    public int[] Inventories;
    public int[] ReinforceInventories;

    public int[] Quests;

    public int[] FeverGauges;
    public int[] FullFevers;
    public int[] BossGauges;
    public int[] BossDeathCounts;

    public int[] CountStartTimes;

    public float BGMVolume = 0.5f;
    public float EffectVolume = 0.5f;
    public bool IsMuteBGM;
    public bool IsMuteEffect;

    public bool IsEraseData;

    public bool IsTutorial;


    public void SaveData()
    {
        if (GameManager.Inst().Login != null)
            if (GameManager.Inst().Login.PlayerID != "")
                UID = GameManager.Inst().Login.PlayerID;

        IsFullPrice = GameManager.Inst().IsFullPrice;

        if (IsEraseData || IsTutorial)
            return;

        Coin = GameManager.Inst().Player.GetCoin();
        Jewel = GameManager.Inst().Jewel;
        CurrentStage = GameManager.Inst().StgManager.Stage;
        ReachedStage = GameManager.Inst().StgManager.ReachedStage;

        MaxInventory = GameManager.Inst().Player.MaxInventory;
        
        PlayerDatas[(int)PlaData.LEVEL] = GameManager.Inst().UpgManager.GetPlayerLevel();
        PlayerDatas[(int)PlaData.CURHP] = GameManager.Inst().Player.GetCurHP();
        PlayerDatas[(int)PlaData.MAXHP] = GameManager.Inst().Player.GetMaxHP();
        PlayerDatas[(int)PlaData.BULLETTYPE] = GameManager.Inst().Player.GetBulletType();
        PlayerDatas[(int)PlaData.COLOR] = GameManager.Inst().ShtManager.GetColorSelection(2);

        for(int i = 0; i < Constants.MAXRESOURCETYPES; i++)
            Resources[i] = GameManager.Inst().Resources[i];

        for (int i = 0; i < Constants.MAXSTAGES; i++)
        {
            BossGauges[i] = GameManager.Inst().StgManager.BossCount[i];
            BossDeathCounts[i] = GameManager.Inst().StgManager.BossDeathCounts[i];

            for(int j = 0; j < 3; j++)
            {
                FeverGauges[i * 6 + j * 2] = Mathf.FloorToInt(GameManager.Inst().StgManager.MinFever[i * 3 + j] * 100.0f);
                FeverGauges[i * 6 + j * 2 + 1] = Mathf.FloorToInt(GameManager.Inst().StgManager.MaxFever[i * 3 + j] * 100.0f);
            }
            FullFevers[i] = GameManager.Inst().StgManager.FullFever[i];

            for (int j = 0; j < Constants.MAXSUBWEAPON; j++)
            {
                SubWeaponDatas[Constants.MAXSUBWEAPON * Constants.SWDATASIZE * i + Constants.SWDATASIZE * j + (int)SWData.LEVEL] = GameManager.Inst().UpgManager.GetSubWeaponLevel(i, j);

                if (GameManager.Inst().UpgManager.GetSubWeaponLevel(i, j) > 0)
                {
                    //if (i == GameManager.Inst().StgManager.Stage - 1)
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

        ReinforceInventories = new int[Constants.INVDATASIZE * Constants.MAXREINFORCETYPE];
        for(int i = 0; i < Constants.MAXREINFORCETYPE; i++)
        {
            Player.EqData reinf = GameManager.Inst().Player.GetReinforce(i);
            if (reinf == null)
                continue;

            ReinforceInventories[Constants.INVDATASIZE * i + (int)InvData.TYPE] = reinf.Type;
            ReinforceInventories[Constants.INVDATASIZE * i + (int)InvData.GRADE] = reinf.Rarity;
            ReinforceInventories[Constants.INVDATASIZE * i + (int)InvData.VALUE] = (int)reinf.Value;
            ReinforceInventories[Constants.INVDATASIZE * i + (int)InvData.AMOUNT] = reinf.Quantity;
            ReinforceInventories[Constants.INVDATASIZE * i + (int)InvData.ID] = reinf.UID;
            ReinforceInventories[Constants.INVDATASIZE * i + (int)InvData.COOLTIME] = (int)reinf.CoolTime;
        }

        for (int i = 0; i < Constants.MAXBULLETS; i++)
        {
            Weapons[Constants.WPDATASIZE * i + (int)WPData.POWERLEVEL] = GameManager.Inst().UpgManager.BData[i].GetPowerLevel();
            Weapons[Constants.WPDATASIZE * i + (int)WPData.RARITY] = GameManager.Inst().UpgManager.BData[i].GetRarity();
            Weapons[Constants.WPDATASIZE * i + (int)WPData.PRICE] = GameManager.Inst().UpgManager.BData[i].GetPrice();
            Weapons[Constants.WPDATASIZE * i + (int)WPData.ATK] = GameManager.Inst().UpgManager.BData[i].GetAtk();
            Weapons[Constants.WPDATASIZE * i + (int)WPData.HP] = GameManager.Inst().UpgManager.BData[i].GetHp();
            Weapons[Constants.WPDATASIZE * i + (int)WPData.SPD] = GameManager.Inst().UpgManager.BData[i].GetSpd();
            Weapons[Constants.WPDATASIZE * i + (int)WPData.EQUIP] = GameManager.Inst().UpgManager.BData[i].GetEquipIndex();
        }
        
        for (int i = 0; i < Constants.MAXSTAGES * Constants.MAXQUESTS; i++)
        {
            Quests[Constants.QSTDATASIZE * i + (int)QSTData.ID] = GameManager.Inst().QstManager.Quests[i].QuestId;
            Quests[Constants.QSTDATASIZE * i + (int)QSTData.COUNT] = GameManager.Inst().QstManager.Quests[i].CurrentCount;
        }

        AdLeft = GameManager.Inst().AdsManager.AdLeft;
        SaveLastAdTime();

        BuffAdLeft = GameManager.Inst().UiManager.MainUI.Buff.AdCount;
        for (int i = 0; i < Constants.MAXBUFFS; i++)
            BuffLefts[i] = (int)GameManager.Inst().BufManager.BuffTimers[i];

        IsMuteBGM = GameManager.Inst().SodManager.IsBgmMute;
        IsMuteEffect = GameManager.Inst().SodManager.IsEffectMute;
        BGMVolume = GameManager.Inst().SodManager.BgmVolume;
        EffectVolume = GameManager.Inst().SodManager.EffectVolume;
    }

    public void LoadData()
    {
        if (IsEraseData)
        {
            IsEraseData = false;
            return;
        }

        GameManager.Inst().IsFullPrice = IsFullPrice;
        if (GameManager.Inst().IsFullPrice)
            GameManager.Inst().BufManager.StartBuff(0);
        
        if (Coin >= 0)
            GameManager.Inst().Player.SetCoin(Coin);

        if (Jewel >= 0)
            GameManager.Inst().SetJewel(Jewel);

        if (MaxInventory > 0)
        {
            if(MaxInventory > Constants.MININVENTORY)
            {
                int count = (MaxInventory - Constants.MININVENTORY) / 10;
                for (int i = 0; i < count; i++)
                {
                    GameManager.Inst().Player.MaxInventory += 10;
                    GameManager.Inst().AddInventory(10);
                }
            }
        }
        GameManager.Inst().UiManager.InventoryScroll.GetComponent<InventoryScroll>().Contents.SetActive(false);

        if (GameManager.Inst().StgManager.Stage <= CurrentStage)
            GameManager.Inst().StgManager.Stage = CurrentStage;

        //if (GameManager.Inst().StgManager.Stage < ReachedStage)
        //    GameManager.Inst().StgManager.UnlockStages(ReachedStage);

        if(ReachedStage > 1)
            for (int i = 0; i < ReachedStage; i++)
                GameManager.Inst().ResManager.StartCount(i);

        if (Resources != null && Resources.Length == Constants.MAXRESOURCETYPES)
            for (int i = 0; i < Constants.MAXRESOURCETYPES; i++)
                GameManager.Inst().SetResource(i + 1, Resources[i]);
        else
        {
            if(Resources != null)
                for(int i = 0; i < Constants.MAXRESOURCETYPES; i++)
                    GameManager.Inst().SetResource(i + 1, Resources[i]);

            Resources = new int[Constants.MAXRESOURCETYPES];
        }

        if (FeverGauges != null && FeverGauges.Length == 6 * Constants.MAXSTAGES)
            for (int j = 0; j < Constants.MAXSTAGES; j++)
                for (int i = 0; i < 3; i++)
                    GameManager.Inst().StgManager.SetFever(j, i, FeverGauges[i * 2], FeverGauges[i * 2 + 1]);
        else
        {
            if (FeverGauges != null)
                for (int j = 0; j < (FeverGauges.Length / 6); j++)
                    for (int i = 0; i < 3; i++)
                        GameManager.Inst().StgManager.SetFever(j, i, FeverGauges[i * 2], FeverGauges[i * 2 + 1]);

            FeverGauges = new int[2 * 3 * Constants.MAXSTAGES];
        }

        if (FullFevers != null && FullFevers.Length == Constants.MAXSTAGES)
            for (int i = 0; i < Constants.MAXSTAGES; i++)
                GameManager.Inst().StgManager.FullFever[i] = FullFevers[i];
        else
        {
            if (FullFevers != null)
                for (int i = 0; i < FullFevers.Length; i++)
                    GameManager.Inst().StgManager.FullFever[i] = FullFevers[i];

            FullFevers = new int[Constants.MAXSTAGES];
        }            

        if (BossGauges != null && BossGauges.Length == Constants.MAXSTAGES)
            for (int i = 0; i < Constants.MAXSTAGES; i++)
                GameManager.Inst().StgManager.BossCount[i] = BossGauges[i];
        else
        {
            if (BossGauges != null)
                for (int i = 0; i < BossGauges.Length; i++)
                    GameManager.Inst().StgManager.BossCount[i] = BossGauges[i];

            BossGauges = new int[Constants.MAXSTAGES];
        }

        if (BossDeathCounts != null && BossDeathCounts.Length == Constants.MAXSTAGES)
            for (int i = 0; i < Constants.MAXSTAGES; i++)
                GameManager.Inst().StgManager.BossDeathCounts[i] = BossDeathCounts[i];
        else
        {
            if (BossDeathCounts != null)
                for (int i = 0; i < BossDeathCounts.Length; i++)
                    GameManager.Inst().StgManager.BossDeathCounts[i] = BossDeathCounts[i];

            BossDeathCounts = new int[Constants.MAXSTAGES];
        }

        if (PlayerDatas != null && PlayerDatas.Length == Constants.PLADATASIZE)
        {
            GameManager.Inst().UpgManager.SetPlayerLevel(PlayerDatas[(int)PlaData.LEVEL]);
            GameManager.Inst().Player.SetCurHP(PlayerDatas[(int)PlaData.CURHP]);
            GameManager.Inst().Player.SetMaxHP(PlayerDatas[(int)PlaData.MAXHP]);
            GameManager.Inst().Player.SetBulletType(PlayerDatas[(int)PlaData.BULLETTYPE]);
            GameManager.Inst().Player.SetSkinColor(PlayerDatas[(int)PlaData.COLOR]);
        }
        else
        {
            if (PlayerDatas != null)
            {
                GameManager.Inst().UpgManager.SetPlayerLevel(PlayerDatas[(int)PlaData.LEVEL]);
                GameManager.Inst().Player.SetCurHP(PlayerDatas[(int)PlaData.CURHP]);
                GameManager.Inst().Player.SetMaxHP(PlayerDatas[(int)PlaData.MAXHP]);
                GameManager.Inst().Player.SetBulletType(PlayerDatas[(int)PlaData.BULLETTYPE]);
                GameManager.Inst().Player.SetSkinColor(PlayerDatas[(int)PlaData.COLOR]);
            }

            PlayerDatas = new int[Constants.PLADATASIZE];
        }

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
        {
            if (Inventories != null)
                for (int i = 0; i < (Inventories.Length / Constants.INVDATASIZE); i++)
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

                        if (eq.UID / 100 == 3)
                            eq.Icon = GameManager.Inst().UiManager.FoodImages[eq.Type + eq.Rarity * Constants.MAXREINFORCETYPE];
                        else if (eq.UID / 100 == 6)
                            eq.Icon = GameManager.Inst().UiManager.EquipImages[eq.Type];

                        GameManager.Inst().Player.AddItem(eq);
                    }
                }

            Inventories = new int[GameManager.Inst().Player.MaxInventory * Constants.INVDATASIZE];
        }

        if (ReinforceInventories != null && ReinforceInventories.Length == Constants.MAXREINFORCETYPE * Constants.INVDATASIZE)
        {
            for (int i = 0; i < Constants.MAXREINFORCETYPE; i++)
            {
                Player.EqData eq = new Player.EqData();
                eq.Quantity = ReinforceInventories[Constants.INVDATASIZE * i + (int)InvData.AMOUNT];
                if (eq.Quantity <= 0)
                    continue;

                eq.Type = ReinforceInventories[Constants.INVDATASIZE * i + (int)InvData.TYPE];
                eq.Rarity = ReinforceInventories[Constants.INVDATASIZE * i + (int)InvData.GRADE];
                eq.Value = ReinforceInventories[Constants.INVDATASIZE * i + (int)InvData.VALUE];
                eq.UID = ReinforceInventories[Constants.INVDATASIZE * i + (int)InvData.ID];
                eq.CoolTime = ReinforceInventories[Constants.INVDATASIZE * i + (int)InvData.COOLTIME];
                eq.Icon = GameManager.Inst().UiManager.FoodImages[eq.Type + eq.Rarity * Constants.MAXREINFORCETYPE];

                GameManager.Inst().Player.AddItem(eq);
            }
        }
        else
            ReinforceInventories = new int[Constants.MAXREINFORCETYPE * Constants.INVDATASIZE];


        if (SubWeaponDatas != null && SubWeaponDatas.Length == Constants.MAXSTAGES * Constants.MAXSUBWEAPON * Constants.SWDATASIZE)
        {
            for (int i = 0; i < Constants.MAXSTAGES; i++)
            {
                for (int j = 0; j < Constants.MAXSUBWEAPON; j++)
                {
                    GameManager.Inst().UpgManager.SetSubWeaponLevel(i, j, SubWeaponDatas[Constants.MAXSUBWEAPON * Constants.SWDATASIZE * i + Constants.SWDATASIZE * j + (int)SWData.LEVEL]);

                    if (i == GameManager.Inst().StgManager.Stage - 1 &&
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
        {
            if (SubWeaponDatas != null)
                for (int i = 0; i < (SubWeaponDatas.Length / (Constants.MAXSUBWEAPON * Constants.SWDATASIZE)); i++)
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

            SubWeaponDatas = new int[Constants.MAXSTAGES * Constants.MAXSUBWEAPON * Constants.SWDATASIZE];
        }

        if (Weapons != null && Weapons.Length == Constants.MAXBULLETS * Constants.WPDATASIZE)
        {
            for (int i = 0; i < Constants.MAXBULLETS; i++)
            {
                GameManager.Inst().UpgManager.BData[i].SetRarity((int)Weapons[Constants.WPDATASIZE * i + (int)WPData.RARITY]);
                GameManager.Inst().UpgManager.BData[i].SetPowerLevel((int)Weapons[Constants.WPDATASIZE * i + (int)WPData.POWERLEVEL]);
                GameManager.Inst().UpgManager.BData[i].SetPrice((int)Weapons[Constants.WPDATASIZE * i + (int)WPData.PRICE]);
                GameManager.Inst().UpgManager.BData[i].SetAtk((int)Weapons[Constants.WPDATASIZE * i + (int)WPData.ATK]);
                GameManager.Inst().UpgManager.BData[i].SetHp((int)Weapons[Constants.WPDATASIZE * i + (int)WPData.HP]);
                GameManager.Inst().UpgManager.BData[i].SetSpd((int)Weapons[Constants.WPDATASIZE * i + (int)WPData.SPD]);
                GameManager.Inst().UpgManager.BData[i].SetEquipIndex((int)Weapons[Constants.WPDATASIZE * i + (int)WPData.EQUIP]);

                GameManager.Inst().UpgManager.SetBasicData(i);
                GameManager.Inst().UpgManager.CheckEquip(i);
                GameManager.Inst().UpgManager.SetMaxData(i);
                GameManager.Inst().UpgManager.SetHPData(i);
            }
        }
        else
        {
            if(Weapons != null)
                for (int i = 0; i < (Weapons.Length / Constants.WPDATASIZE); i++)
                {
                    GameManager.Inst().UpgManager.BData[i].SetRarity((int)Weapons[Constants.WPDATASIZE * i + (int)WPData.RARITY]);
                    GameManager.Inst().UpgManager.BData[i].SetPowerLevel((int)Weapons[Constants.WPDATASIZE * i + (int)WPData.POWERLEVEL]);
                    GameManager.Inst().UpgManager.BData[i].SetPrice((int)Weapons[Constants.WPDATASIZE * i + (int)WPData.PRICE]);
                    GameManager.Inst().UpgManager.BData[i].SetAtk((int)Weapons[Constants.WPDATASIZE * i + (int)WPData.ATK]);
                    GameManager.Inst().UpgManager.BData[i].SetHp((int)Weapons[Constants.WPDATASIZE * i + (int)WPData.HP]);
                    GameManager.Inst().UpgManager.BData[i].SetSpd((int)Weapons[Constants.WPDATASIZE * i + (int)WPData.SPD]);
                    GameManager.Inst().UpgManager.BData[i].SetEquipIndex((int)Weapons[Constants.WPDATASIZE * i + (int)WPData.EQUIP]);

                    GameManager.Inst().UpgManager.SetBasicData(i);
                    GameManager.Inst().UpgManager.CheckEquip(i);
                    GameManager.Inst().UpgManager.SetMaxData(i);
                    GameManager.Inst().UpgManager.SetHPData(i);
                }

            Weapons = new float[Constants.MAXBULLETS * Constants.WPDATASIZE];
        }

        if (CountStartTimes != null && CountStartTimes.Length == (Constants.MAXSTAGES * Constants.TIMEDATASIZE))
            for (int i = 0; i < Constants.MAXSTAGES; i++)
            {
                GameManager.Inst().ResManager.StartTimes[i] = new DateTime(CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.YEAR], CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.MONTH], CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.DATE],
                                                                            CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.HOUR], CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.MINUTE], CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.SECOND]);
                GameManager.Inst().ResManager.LoadCount(i);
            }
        else
        {
            if (CountStartTimes != null)
                for (int i = 0; i < (CountStartTimes.Length / Constants.TIMEDATASIZE); i++)
                {
                    GameManager.Inst().ResManager.StartTimes[i] = new DateTime(CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.YEAR], CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.MONTH], CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.DATE],
                                                                                CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.HOUR], CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.MINUTE], CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.SECOND]);
                    GameManager.Inst().ResManager.LoadCount(i);
                }

            CountStartTimes = new int[Constants.MAXSTAGES * Constants.TIMEDATASIZE];
            for (int i = 0; i < Constants.MAXSTAGES; i++)
            {
                CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.YEAR] = Now.Year;
                CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.MONTH] = Now.Month;
                CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.DATE] = Now.Day;
                CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.HOUR] = Now.Hour;
                CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.MINUTE] = Now.Minute;
                CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.SECOND] = Now.Second;
            }
        }

        GameManager.Inst().AdsManager.AdLeft = AdLeft;
        GameManager.Inst().UiManager.MainUI.Center.Shop.SetAdLeftText();
        if (GameManager.Inst().AdsManager.AdLeft > 0)
            GameManager.Inst().UiManager.MainUI.Center.Shop.EnableAd();
        else
            GameManager.Inst().UiManager.MainUI.Center.Shop.DisableAd();
        if (LastAdTime != null)
            LoadLastAdTime();
        else
            LastAdTime = new int[6];

        GameManager.Inst().UiManager.MainUI.Buff.SubtractAdCount(3 - BuffAdLeft);
        if (LastBuffAdTime != null)
            LoadLastBuffAdTime();
        else
            LastBuffAdTime = new int[6];

        if (BuffLefts != null)
            for (int i = 0; i < Constants.MAXBUFFS; i++)
            {
                GameManager.Inst().BufManager.BuffTimers[i] = BuffLefts[i];

                if (GameManager.Inst().BufManager.BuffTimers[i] > 0)
                {
                    GameManager.Inst().BufManager.BuffTimers[i] -= 600.0f;
                    GameManager.Inst().BufManager.StartBuff(i);
                }
            }
        else
            BuffLefts = new int[Constants.MAXBUFFS];

        GameManager.Inst().SodManager.IsBgmMute = IsMuteBGM;
        GameManager.Inst().SodManager.IsEffectMute = IsMuteEffect;
        GameManager.Inst().SodManager.BgmVolume = BGMVolume;
        GameManager.Inst().SodManager.EffectVolume = EffectVolume;
    }

    public void ResetData()
    {
        UID = "";
        Coin = 0;
        Jewel = 0;
        Resources = new int[Constants.MAXSTAGES];

        LastDailyTime = new int[6];
        IsDaily = false;
        DailyLeft = 0;
        IsDailyPlus = false;
        DailyPlusLeft = 0;

        AdLeft = 5;
        LastAdTime = new int[6];
        BuffAdLeft = 3;
        LastBuffAdTime = new int[6];
        BuffLefts = new int[Constants.MAXBUFFS];

        MaxInventory = Constants.MININVENTORY;

        CurrentStage = 1;
        ReachedStage = 1;
        FullFevers = new int[Constants.MAXSTAGES];
        FeverGauges = new int[2 * 3 * Constants.MAXSTAGES];
        BossGauges = new int[Constants.MAXSTAGES];
        BossDeathCounts = new int[Constants.MAXSTAGES];

        SubWeaponDatas = new int[Constants.MAXSTAGES * Constants.MAXSUBWEAPON * Constants.SWDATASIZE];

        Inventories = new int[GameManager.Inst().Player.MaxInventory * Constants.INVDATASIZE];

        Weapons = new float[Constants.MAXBULLETS * Constants.WPDATASIZE];
        for (int i = 0; i < Constants.MAXBULLETS; i++)
            Weapons[Constants.WPDATASIZE * i + (int)WPData.EQUIP] = -1.0f;

        Quests = new int[Constants.MAXSTAGES * Constants.MAXQUESTS * Constants.QSTDATASIZE];

        CountStartTimes = new int[Constants.MAXSTAGES * Constants.TIMEDATASIZE];
        for(int i = 0; i < Constants.MAXSTAGES; i++)
        {
            CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.YEAR] = Now.Year;
            CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.MONTH] = Now.Month;
            CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.DATE] = Now.Day;
            CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.HOUR] = Now.Hour;
            CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.MINUTE] = Now.Minute;
            CountStartTimes[Constants.TIMEDATASIZE * i + (int)TIMEData.SECOND] = Now.Second;
        }

        BGMVolume = 0.5f;
        EffectVolume = 0.5f;
        IsMuteBGM = false;
        IsMuteEffect = false;

        IsTutorial = true;
    }

    public void LoadReachedStage()
    {
        if (GameManager.Inst().StgManager.ReachedStage < ReachedStage)
            GameManager.Inst().StgManager.ReachedStage = ReachedStage;
    }

    public void MoveScene()
    {
        if (IsTutorial)
            SceneManager.LoadScene("Stage0");
        else if (GameManager.Inst().StgManager.Stage >= 1)
        {
            if (SceneManager.GetActiveScene().name != ("Stage" + GameManager.Inst().StgManager.Stage.ToString()))
                SceneManager.LoadScene("Stage" + GameManager.Inst().StgManager.Stage.ToString());
        }
    }

    public void DailyJewel()
    {
        DateTime last = new DateTime(LastDailyTime[0], LastDailyTime[1], LastDailyTime[2], LastDailyTime[3], LastDailyTime[4], LastDailyTime[5]);

        if(Now.Day != last.Day)
        {
            IsDaily = true;
            IsDailyPlus = true;
        }

        if (DailyLeft > 0)
            GameManager.Inst().UiManager.MainUI.Center.DailyLeft.Show(DailyLeft);
        if (DailyPlusLeft > 0)
            GameManager.Inst().UiManager.MainUI.Center.DailyPlusLeft.Show(DailyPlusLeft);

        if (Now.Date <= last.Date || Now.Hour < 9)
            return;

        GiveDaily();
    }

    public void GiveDaily()
    {
        if (DailyLeft > 0 && IsDaily)
        {
            if (DailyPlusLeft > 0 && IsDailyPlus)
                GameManager.Inst().UiManager.MainUI.Center.DailyJewelUI.Show(2);
            else
                GameManager.Inst().UiManager.MainUI.Center.DailyJewelUI.Show(0);
        }
        else if (DailyPlusLeft > 0 && IsDailyPlus)
            GameManager.Inst().UiManager.MainUI.Center.DailyJewelUI.Show(1);
    }

    public void ProcessDailyJewel()
    {
        if (DailyLeft > 0 && IsDailyPlus)
        {
            GameManager.Inst().AddJewel(4);
            DailyLeft--;

            GameManager.Inst().UiManager.MainUI.Center.DailyLeft.Show(DailyLeft);
            IsDaily = false;
        }
        else
            GameManager.Inst().UiManager.MainUI.Center.DailyLeft.gameObject.SetActive(false);

        if (DailyPlusLeft > 0 && IsDailyPlus)
        {
            GameManager.Inst().AddJewel(12);
            DailyPlusLeft--;

            GameManager.Inst().UiManager.MainUI.Center.DailyPlusLeft.Show(DailyPlusLeft);
            IsDailyPlus = false;
        }
        else
            GameManager.Inst().UiManager.MainUI.Center.DailyPlusLeft.gameObject.SetActive(false);

        SetLastDailyTime();
    }

    public void StartDailyJewel()
    {
        LastDailyTime = new int[6];
        SetLastDailyTime();
    }

    void SetLastDailyTime()
    {
        LastDailyTime[0] = Now.Year;
        LastDailyTime[1] = Now.Month;
        LastDailyTime[2] = Now.Day;
        LastDailyTime[3] = 9;
        LastDailyTime[4] = 0;
        LastDailyTime[5] = 0;
    }

    void SaveLastAdTime()
    {
        LastAdTime[0] = GameManager.Inst().AdsManager.LastTime.Year;
        LastAdTime[1] = GameManager.Inst().AdsManager.LastTime.Month;
        LastAdTime[2] = GameManager.Inst().AdsManager.LastTime.Day;
        LastAdTime[3] = GameManager.Inst().AdsManager.LastTime.Hour;
        LastAdTime[4] = GameManager.Inst().AdsManager.LastTime.Minute;
        LastAdTime[5] = GameManager.Inst().AdsManager.LastTime.Second;

        LastBuffAdTime[0] = Now.Year;
        LastBuffAdTime[1] = Now.Month;
        LastBuffAdTime[2] = Now.Day;
        LastBuffAdTime[3] = 9;
        LastBuffAdTime[4] = 0;
        LastBuffAdTime[5] = 0;
    }

    void LoadLastAdTime()
    {
        if (LastAdTime[0] == 0)
        {
            GameManager.Inst().AdsManager.LastTime = DateTime.Now;
            return;
        }

        GameManager.Inst().AdsManager.LastTime = new DateTime(LastAdTime[0], LastAdTime[1], LastAdTime[2],
                                                                LastAdTime[3], LastAdTime[4], LastAdTime[5]);        
    }

    void LoadLastBuffAdTime()
    {
        if (Now.Day != LastBuffAdTime[2] && Now.Hour >= 9)
            GameManager.Inst().UiManager.MainUI.Buff.ResetAdCount();
    }

    public void LoadSubWeapon()
    {
        if (SubWeaponDatas != null && SubWeaponDatas.Length == Constants.MAXSTAGES * Constants.MAXSUBWEAPON * Constants.SWDATASIZE)
            for (int i = 0; i < Constants.MAXSTAGES; i++)
            {
                for (int j = 0; j < Constants.MAXSUBWEAPON; j++)
                {
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
        else
        {
            if (SubWeaponDatas != null)
                for (int i = 0; i < (SubWeaponDatas.Length / (Constants.MAXSUBWEAPON * Constants.SWDATASIZE)); i++)
                {
                    for (int j = 0; j < Constants.MAXSUBWEAPON; j++)
                    {
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

            SubWeaponDatas = new int[Constants.MAXSTAGES * Constants.MAXSUBWEAPON * Constants.SWDATASIZE];
        }
    }

    public void LoadQuests()
    {
        if (Quests != null && Quests.Length == (Constants.MAXSTAGES * Constants.MAXQUESTS * Constants.QSTDATASIZE))
            for (int i = 0; i < Constants.MAXSTAGES * Constants.MAXQUESTS; i++)
            {
                if (GameManager.Inst().QstManager.Quests[i].QuestId == Quests[Constants.QSTDATASIZE * i + (int)QSTData.ID])
                {
                    GameManager.Inst().QstManager.Quests[i].CurrentCount = Quests[Constants.QSTDATASIZE * i + (int)QSTData.COUNT];
                    GameManager.Inst().QstManager.CheckFinish(i);
                }
            }
        else
        {
            if (Quests != null)
                for (int i = 0; i < (Quests.Length / Constants.QSTDATASIZE); i++)
                {
                    if (GameManager.Inst().QstManager.Quests[i].QuestId == Quests[Constants.QSTDATASIZE * i + (int)QSTData.ID])
                    {
                        GameManager.Inst().QstManager.Quests[i].CurrentCount = Quests[Constants.QSTDATASIZE * i + (int)QSTData.COUNT];
                        GameManager.Inst().QstManager.CheckFinish(i);
                    }
                }

            Quests = new int[Constants.MAXSTAGES * Constants.MAXQUESTS * Constants.QSTDATASIZE];
        }
    }

    public void LoadDaily()
    {
        if (LastDailyTime != null)
        {
            if (DailyLeft > 0 || DailyPlusLeft > 0)
                DailyJewel();
        }
        else
            LastDailyTime = new int[6];
    }
}