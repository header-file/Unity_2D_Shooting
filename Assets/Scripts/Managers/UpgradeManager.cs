using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public enum UpgradeType
    {
        NORMAL = 0,
        SPREAD = 1,
        MISSILE = 2,
        LASER = 3,
        CHARGE = 4,
        BOOMERANG = 5,
        CHAIN = 6,
        SUBWEAPON = 100,
        PLAYER = 1000
    };

    public struct BulletData
    {
        public void SetPowerLevel(int Level)
        {
            if (Level <= MaxBulletLevel * (Rarity + 1))
                PowerLevel = Level;
        }
        public void SetBaseDamage(int damage) { BaseDamage = damage; }
        public void SetRarity(int rare) { Rarity = rare; }
        public void SetHealth(int hp) { Health = hp; }
        public void SetSpeed(float velocity) { Speed = velocity; }
        public void SetReloadTime(float Time) { ReloadTime = Time; }
        public void SetDuration(float Time) { Duration = Time; }
        public void SetPrice(int price) { Price = price; }
        public void AddRarity() { Rarity++; }
        public void SetActive(bool b) { IsActive = b; }
        public void SetAtk(int atk)
        {
            if (MaxAtk < atk)
                Atk = MaxAtk;
            else
                Atk = atk;
        }
        public void SetHp(int hp)
        {
            if (MaxHp < hp)
                Hp = MaxHp;
            else
                Hp = hp;
        }
        public void SetSpd(int spd)
        {
            if (MaxSpd < spd)
                Spd = MaxSpd;
            else
                Spd = spd;
        }
        public void SetMaxAtk(int max) { MaxAtk = max; }
        public void SetMaxHp(int max) { MaxHp = max; }
        public void SetMaxSpd(int max) { MaxSpd = max; }

        public int GetMaxBulletLevel() { return MaxBulletLevel * (Rarity + 1); }
        public int GetPowerLevel() { return PowerLevel; }
        public int GetHealth() { return Health; }
        public int GetPrice() { return Price; }
        public int GetAtk() { return Atk; }
        public int GetHp() { return Hp; }
        public int GetSpd() { return Spd; }
        public int GetMaxAtk() { return MaxAtk; }
        public int GetMaxHp() { return MaxHp; }
        public int GetMaxSpd() { return MaxSpd; }
        public float GetSpeed() { return Speed; }
        public float GetReloadTime() { return ReloadTime; }
        public float GetDuration() { return Duration; }
        public int GetDamage() { return PowerLevel + BaseDamage; }
        public int GetRarity() { return Rarity; }
        public int GetBaseDamage() { return BaseDamage; }
        public bool GetActive() { return IsActive; }
        
        public void ResetData()
        {
            PowerLevel = 1;
            BaseDamage = 0;

            Price = 10;
            Rarity = 0;

            Health = 10;

            ReloadTime = 0.0f;
            Duration = 0.0f;
            Speed = 0.0f;

            Atk = 0;
            Hp = 0;
            Spd = 0;
            MaxAtk = 0;
            MaxHp = 0;
            MaxSpd = 0;

            IsActive = false;
        }

        public void SetBulletDatas(List<Dictionary<string, object>> data, int index)
        {
            BaseDamage = int.Parse(data[index]["BaseDamage"].ToString());
            PowerLevel = int.Parse(data[index]["StartLevel"].ToString());
            ReloadTime = float.Parse(data[index]["ReloadTime"].ToString());
            Duration = float.Parse(data[index]["Duration"].ToString());
            Speed = float.Parse(data[index]["Speed"].ToString());
            Rarity = index / Constants.MAXBULLETS;
            Health = 10;
            MaxAtk = 0;
            MaxHp = 0;
            MaxSpd = 0;
        }

        const int MaxBulletLevel = 10;

        private int PowerLevel;
        private int BaseDamage;
        private int Price;
        private int Rarity;

        private int Health;

        private float Speed;
        private float ReloadTime;
        private float Duration;

        private int Atk;
        private int Hp;
        private int Spd;
        private int MaxAtk;
        private int MaxHp;
        private int MaxSpd;

        bool IsActive;
    };

    public GameObject[] SubPositions;

   

    public BulletData[] BData;

    int PlayerLevel;
    int[] PlayerUpgradePriceData;
    int[,] SubWeaponLevel;
    int SubWeaponBuyPrice;
    int CurrentSubWeaponIndex;
    int[,] ResourceData;
    int[,] SubWpPriceData;
    int[] WeaponPriceData;
    int[,,] WeaponReinforceMaxData;

    public int GetPlayerLevel(){ return PlayerLevel; }
    public int GetPlayerUpgradePriceData(int index) { return PlayerUpgradePriceData[index]; }
    public int GetSubWeaponLevel(int stage, int index) { return SubWeaponLevel[stage, index]; }
    public int GetSubWeaponPrice(int index) { return SubWpPriceData[GameManager.Inst().StgManager.Stage, SubWeaponLevel[GameManager.Inst().StgManager.Stage, index]]; }
    public int GetSubWeaponBuyPrice() { return SubWeaponBuyPrice; }
    public int GetResourceData(int rarity, int index) { return ResourceData[rarity, index]; }

    public void SetPlayerLevel(int level) { PlayerLevel = level; }
    public void SetCurrentSubWeaponIndex(int selectedIndex) { CurrentSubWeaponIndex = selectedIndex; }
    public void SetSubWeaponLevel(int stage, int index, int level) { SubWeaponLevel[stage, index] = level; }

    void Awake()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("Datas/BulletData");
        BData = new BulletData[Constants.MAXBULLETS * 5];
        for (int i = 0; i < BData.Length; i++)
        {
            BData[i].ResetData();
            BData[i].SetBulletDatas(data, i);
        }

        data = CSVReader.Read("Datas/WpAwakenData");
        ResourceData = new int[Constants.MAXRARITY, Constants.MAXSTAGES];
        for (int i = 0; i < Constants.MAXRARITY; i++)
            SetResourceDatas(data, i);

        data = CSVReader.Read("Datas/WpUpgradeData");
        WeaponPriceData = new int[50];
        for(int i = 0; i < 50; i++)
            WeaponPriceData[i] = int.Parse(data[i]["Price"].ToString());

        data = CSVReader.Read("Datas/WpReinforceMaxData");
        WeaponReinforceMaxData = new int[Constants.MAXBULLETS, 5, 3];
        for (int i = 0; i < Constants.MAXBULLETS; i++)
        {
            for(int j = 0; j < 3; j++)
                SetWeaponReinforceMaxDatas(data, i, j);

            SetMaxData(i);
        }

        data = CSVReader.Read("Datas/SubWpPriceData");
        SubWpPriceData = new int[Constants.MAXSTAGES, Constants.MAXSUBLEVEL + 1];
        for (int i = 0; i < Constants.MAXSTAGES; i++)
            SetSubWpPriceDatas(data, i);

        data = CSVReader.Read("Datas/PlayerUpgradePriceData");
        PlayerUpgradePriceData = new int[Constants.MAXPLAYERLEVEL];
        for(int i = 0; i < Constants.MAXPLAYERLEVEL; i++)
            PlayerUpgradePriceData[i] = int.Parse(data[i]["Price"].ToString());

        SubWeaponLevel = new int[Constants.MAXSTAGES, 4];
        for(int j = 0; j < Constants.MAXSTAGES; j++)
            for(int i = 0; i < 4; i++)
                SubWeaponLevel[j, i] = 0;

        CurrentSubWeaponIndex = -1;

        PlayerLevel = 1;
    }

    void Start()
    {
        SubWeaponBuyPrice = SubWpPriceData[GameManager.Inst().StgManager.Stage, 0];

        GameManager.Inst().TxtManager.SetBLevels((int)Bullet.BulletType.NORMAL, BData[(int)Bullet.BulletType.NORMAL].GetPowerLevel());
        BData[(int)Bullet.BulletType.NORMAL].SetPrice(WeaponPriceData[BData[(int)Bullet.BulletType.NORMAL].GetPowerLevel()]);
    }

    void SetResourceDatas(List<Dictionary<string, object>> data, int rarity)
    {
        ResourceData[rarity, 0] = int.Parse(data[rarity]["A"].ToString());
        ResourceData[rarity, 1] = int.Parse(data[rarity]["B"].ToString());
        ResourceData[rarity, 2] = int.Parse(data[rarity]["C"].ToString());
        ResourceData[rarity, 3] = int.Parse(data[rarity]["D"].ToString());
    }

    void SetSubWpPriceDatas(List<Dictionary<string, object>> data, int stage)
    {
        SubWpPriceData[stage, 0] = int.Parse(data[stage]["Lv1"].ToString());
        SubWpPriceData[stage, 1] = int.Parse(data[stage]["Lv2"].ToString());
        SubWpPriceData[stage, 2] = int.Parse(data[stage]["Lv3"].ToString());
        SubWpPriceData[stage, 3] = int.Parse(data[stage]["Lv4"].ToString());
        SubWpPriceData[stage, 4] = int.Parse(data[stage]["Lv5"].ToString());
        SubWpPriceData[stage, 5] = 0;
    }

    void SetWeaponReinforceMaxDatas(List<Dictionary<string, object>> data, int bulletType, int eqType)
    {
        WeaponReinforceMaxData[bulletType, (int)Item_Equipment.Rarity.WHITE, eqType] = int.Parse(data[bulletType * 3 + eqType]["WHITE"].ToString());
        WeaponReinforceMaxData[bulletType, (int)Item_Equipment.Rarity.GREEN, eqType] = int.Parse(data[bulletType * 3 + eqType]["GREEN"].ToString());
        WeaponReinforceMaxData[bulletType, (int)Item_Equipment.Rarity.BLUE, eqType] = int.Parse(data[bulletType * 3 + eqType]["BLUE"].ToString());
        WeaponReinforceMaxData[bulletType, (int)Item_Equipment.Rarity.PURPLE, eqType] = int.Parse(data[bulletType * 3 + eqType]["PURPLE"].ToString());
        WeaponReinforceMaxData[bulletType, (int)Item_Equipment.Rarity.YELLOW, eqType] = int.Parse(data[bulletType * 3 + eqType]["YELLOW"].ToString());
    }

    public void AddLevel(int UpgType)
    {
        if(UpgType < (int)UpgradeType.SUBWEAPON)
        {
            if (BData[UpgType].GetPowerLevel() > BData[UpgType].GetMaxBulletLevel())
                return;
            else if(BData[UpgType].GetPowerLevel() == BData[UpgType].GetMaxBulletLevel())   //레어도 상승
            {
                //가격
                for(int i = 0; i < Constants.MAXSTAGES; i++)
                {
                    if (GameManager.Inst().Resources[i] < ResourceData[BData[UpgType].GetRarity(), i])
                        return;
                }
                for (int i = 0; i < Constants.MAXSTAGES; i++)
                    GameManager.Inst().SubtractResource(i, ResourceData[BData[UpgType].GetRarity(), i]);

                //BData 처리
                BData[UpgType].SetPowerLevel(1);
                BData[UpgType].AddRarity();
                BData[UpgType].SetPrice((int)Mathf.Pow(10.0f, BData[UpgType].GetRarity()));
                BData[UpgType].SetMaxAtk(WeaponReinforceMaxData[UpgType, BData[UpgType].GetRarity(), 0]);
                BData[UpgType].SetMaxHp(WeaponReinforceMaxData[UpgType, BData[UpgType].GetRarity(), 1]);
                BData[UpgType].SetMaxSpd(WeaponReinforceMaxData[UpgType, BData[UpgType].GetRarity(), 2]);

                //UI
                GameManager.Inst().TxtManager.SetBLevels(UpgType, BData[UpgType].GetPowerLevel());
                GameManager.Inst().TxtManager.SetBPrices(UpgType, BData[UpgType].GetPrice());
                GameManager.Inst().UiManager.ShowInfoArea(UpgType);
                GameManager.Inst().UiManager.InfoAreaTrigger("LevelUp");

                //퀘스트 처리
                GameManager.Inst().QstManager.QuestProgress((int)QuestManager.QuestType.FORGE, BData[UpgType].GetRarity(), 1);

                //기타 능력치
                if (BData[UpgType].GetBaseDamage() != BData[UpgType + BData[UpgType].GetRarity() * Constants.MAXBULLETS].GetBaseDamage())
                    BData[UpgType].SetBaseDamage(BData[UpgType + BData[UpgType].GetRarity() * Constants.MAXBULLETS].GetBaseDamage());
                if (BData[UpgType].GetReloadTime() != BData[UpgType + BData[UpgType].GetRarity() * Constants.MAXBULLETS].GetReloadTime())
                    BData[UpgType].SetReloadTime(BData[UpgType + BData[UpgType].GetRarity() * Constants.MAXBULLETS].GetReloadTime());
                if (BData[UpgType].GetDuration() != BData[UpgType + BData[UpgType].GetRarity() * Constants.MAXBULLETS].GetDuration())
                    BData[UpgType].SetDuration(BData[UpgType + BData[UpgType].GetRarity() * Constants.MAXBULLETS].GetDuration());

                return;
            }

            //가격
            if (GameManager.Inst().Player.GetCoin() < BData[UpgType].GetPrice())
                return;
            else
                GameManager.Inst().Player.MinusCoin(BData[UpgType].GetPrice());

            //BData 처리
            BData[UpgType].SetPowerLevel(BData[UpgType].GetPowerLevel() + 1);
            BData[UpgType].SetPrice(WeaponPriceData[BData[UpgType].GetPowerLevel()]);
            BData[UpgType].SetHealth(BData[UpgType].GetHealth() + 5);

            //UI
            GameManager.Inst().TxtManager.SetBLevels(UpgType, BData[UpgType].GetPowerLevel());
            GameManager.Inst().TxtManager.SetBPrices(UpgType, BData[UpgType].GetPrice());
            GameManager.Inst().UiManager.ShowInfoArea(UpgType);
            GameManager.Inst().UiManager.WeaponUI.InfoWindow.Show(UpgType);
            GameManager.Inst().UiManager.InfoAreaTrigger("LevelUp");
        }
        else if(UpgType == (int)UpgradeType.SUBWEAPON)
        {
            if (SubWeaponLevel[GameManager.Inst().StgManager.Stage, CurrentSubWeaponIndex] >= Constants.MAXSUBLEVEL)
                return;

            //가격
            if (SubWeaponLevel[GameManager.Inst().StgManager.Stage, CurrentSubWeaponIndex] == 0)
            {
                if (GameManager.Inst().Player.GetCoin() < SubWeaponBuyPrice)
                    return;
                else
                    GameManager.Inst().Player.MinusCoin(SubWeaponBuyPrice);
                SubWeaponBuyPrice *= 2;
                if (SubWeaponBuyPrice >= 8000)
                    SubWeaponBuyPrice = 0;
                AddSW(CurrentSubWeaponIndex);
            }
            else
            {
                if (GameManager.Inst().Player.GetCoin() < SubWpPriceData[GameManager.Inst().StgManager.Stage, SubWeaponLevel[GameManager.Inst().StgManager.Stage, CurrentSubWeaponIndex]])
                    return;
                
                GameManager.Inst().Player.MinusCoin(SubWpPriceData[GameManager.Inst().StgManager.Stage, SubWeaponLevel[GameManager.Inst().StgManager.Stage, CurrentSubWeaponIndex]]);
            }

            //Data 처리
            SubWeaponLevel[GameManager.Inst().StgManager.Stage, CurrentSubWeaponIndex]++;
            GameManager.Inst().GetSubweapons(CurrentSubWeaponIndex).SetHP(10 * SubWeaponLevel[GameManager.Inst().StgManager.Stage, CurrentSubWeaponIndex]);

            //UI
            GameManager.Inst().TxtManager.SetSPrice(SubWpPriceData[GameManager.Inst().StgManager.Stage, SubWeaponLevel[GameManager.Inst().StgManager.Stage, CurrentSubWeaponIndex]]);
            if (SubWeaponLevel[GameManager.Inst().StgManager.Stage, CurrentSubWeaponIndex] >= 5)
                GameManager.Inst().UiManager.GetBuySWUI().SetBuyBtnInteratable(false);
            GameManager.Inst().UiManager.SetSubWeaponInteratable(false);
        }
        /*else
        {
            //레벨
            if (PlayerLevel >= Constants.MAXPLAYERLEVEL)
                return;

            //가격
            if (GameManager.Inst().Player.GetCoin() < PlayerUpgradePriceData[PlayerLevel])
                return;
            else
                GameManager.Inst().Player.MinusCoin(PlayerUpgradePriceData[PlayerLevel]);

            PlayerLevel++;
            GameManager.Inst().Player.SetCurHP(GameManager.Inst().Player.GetCurHP() + 5);
            GameManager.Inst().Player.SetMaxHP(PlayerLevel * 5);
        }*/
    }

    public void AddSW(int curIndex)
    {
        GameObject subWeapon = GameManager.Inst().ObjManager.MakeObj("SubWeapon");
        int index = curIndex;
        Vector3 pos = SubPositions[curIndex].transform.position;
        subWeapon.transform.position = pos;
        SubWeapon sub = subWeapon.GetComponent<SubWeapon>();

        GameManager.Inst().SetSubWeapons(sub, curIndex);
        GameManager.Inst().SetHitAreas(subWeapon, curIndex);

        //if (index > 1)
        //    index++;
        sub.SetNumID(index);

        for(int i = 0; i < Constants.MAXBULLETS; i++)
        {
            if (GameManager.Inst().Player.GetBulletType() == i)
                continue;

            if (CheckSW(0, i))
            {
                sub.SetBulletType(i);
                return;
            }
        }
    }

    public void SWUiInteract(int curIndex)
    {
        //UI
        GameManager.Inst().TxtManager.SetSPrice(SubWpPriceData[GameManager.Inst().StgManager.Stage, SubWeaponLevel[GameManager.Inst().StgManager.Stage, curIndex]]);
        if (SubWeaponLevel[GameManager.Inst().StgManager.Stage, curIndex] >= 5)
            GameManager.Inst().UiManager.GetBuySWUI().SetBuyBtnInteratable(false);
        GameManager.Inst().UiManager.SetSubWeaponInteratable(false);
    }

    bool CheckSW(int index, int i)
    {
        if (index == 4)
            return true;

        if (GameManager.Inst().GetSubweapons(index) != null)
        {
            if (GameManager.Inst().GetSubweapons(index).GetBulletType() != i)
                return CheckSW(++index, i);
            else
                return false;
        }
        return true;
    }

    public void SetMaxData(int i)
    {
        BData[i].SetMaxAtk(WeaponReinforceMaxData[i, BData[i].GetRarity(), 0]);
        BData[i].SetMaxHp(WeaponReinforceMaxData[i, BData[i].GetRarity(), 1]);
        BData[i].SetMaxSpd(WeaponReinforceMaxData[i, BData[i].GetRarity(), 2]);
    }
}

/*
 for(int i = 0; i < Bullet.MAXBULLETS; i++)
        {
            if (GameManager.Inst().Player.GetBulletType() == i)
                continue;

            int subCnt = 0;
            for (int j = 0; j < 4; j++)
            {
                if (GameManager.Inst().GetSubweapons(j) == null)
                    break;

                if (GameManager.Inst().GetSubweapons(j) == sub)
                    continue;
                else if (GameManager.Inst().GetSubweapons(j).GetBulletType() != i)
                    subCnt++;
            }

            if (GameManager.Inst().Player.GetBulletType() < i)
                subCnt++;

            if (subCnt >= CurrentSubWeaponIndex)
            {
                sub.SetBulletType(i);
                return;
            }
        }
*/


/*UpgradeType Type = (UpgradeType)UpgType;

        switch (Type)
        {
            case UpgradeType.NORMAL:
                if (BData[(int)Bullet.BulletType.NORMAL].GetPowerLevel() >= BData[(int)Bullet.BulletType.NORMAL].GetMaxBulletLevel())
                    return;

                //가격
                if (GameManager.Inst().Player.GetCoin() < BData[(int)Bullet.BulletType.NORMAL].GetPrice())
                    return;
                else
                    GameManager.Inst().Player.MinusCoin(BData[(int)Bullet.BulletType.NORMAL].GetPrice());

                //BData 처리
                BData[(int)Bullet.BulletType.NORMAL].SetPowerLevel(BData[(int)Bullet.BulletType.NORMAL].GetPowerLevel() + 1);
                BData[(int)Bullet.BulletType.NORMAL].SetPrice();

                //UI
                GameManager.Inst().TxtManager.SetBLevels((int)Bullet.BulletType.NORMAL, BData[(int)Bullet.BulletType.NORMAL].GetPowerLevel());
                GameManager.Inst().TxtManager.SetBPrices((int)Bullet.BulletType.NORMAL, BData[(int)Bullet.BulletType.NORMAL].GetPrice());
                GameManager.Inst().UiManager.ShowDetail((int)Bullet.BulletType.NORMAL);

                //특수 효과
                if (BData[(int)Bullet.BulletType.NORMAL].GetPowerLevel() == 5)
                    BData[(int)Bullet.BulletType.NORMAL].SetReloadTime(0.25f);
                break;

            case UpgradeType.SPREAD:
                if (BData[(int)Bullet.BulletType.SPREAD].GetPowerLevel() >= BData[(int)Bullet.BulletType.SPREAD].GetMaxBulletLevel())
                    return;

                //가격
                if (GameManager.Inst().Player.GetCoin() < BData[(int)Bullet.BulletType.SPREAD].GetPrice())
                    return;
                else
                    GameManager.Inst().Player.MinusCoin(BData[(int)Bullet.BulletType.SPREAD].GetPrice());

                //BData 처리
                BData[(int)Bullet.BulletType.SPREAD].SetPowerLevel(BData[(int)Bullet.BulletType.SPREAD].GetPowerLevel() + 1);
                BData[(int)Bullet.BulletType.SPREAD].SetPrice();

                //UI
                GameManager.Inst().TxtManager.SetBLevels((int)Bullet.BulletType.SPREAD, BData[(int)Bullet.BulletType.SPREAD].GetPowerLevel());
                GameManager.Inst().TxtManager.SetBPrices((int)Bullet.BulletType.SPREAD, BData[(int)Bullet.BulletType.SPREAD].GetPrice());
                GameManager.Inst().UiManager.ShowDetail((int)Bullet.BulletType.SPREAD);

                //특수 효과
                if (BData[(int)Bullet.BulletType.SPREAD].GetPowerLevel() == 3)
                    BData[(int)Bullet.BulletType.SPREAD].SetDuration(1.0f);
                else if (BData[(int)Bullet.BulletType.SPREAD].GetPowerLevel() == 5)
                {
                    BData[(int)Bullet.BulletType.SPREAD].SetDuration(1.5f);
                    BData[(int)Bullet.BulletType.SPREAD].SetReloadTime(0.75f);
                } 
                break;

            case UpgradeType.MISSILE:
                if (BData[(int)Bullet.BulletType.MISSILE].GetPowerLevel() >= BData[(int)Bullet.BulletType.MISSILE].GetMaxBulletLevel())
                    return;

                //가격
                if (GameManager.Inst().Player.GetCoin() < BData[(int)Bullet.BulletType.MISSILE].GetPrice())
                    return;
                else
                    GameManager.Inst().Player.MinusCoin(BData[(int)Bullet.BulletType.MISSILE].GetPrice());


                //BData 처리
                BData[(int)Bullet.BulletType.MISSILE].SetPowerLevel(BData[(int)Bullet.BulletType.MISSILE].GetPowerLevel() + 1);
                BData[(int)Bullet.BulletType.MISSILE].SetPrice();

                //UI
                GameManager.Inst().TxtManager.SetBLevels((int)Bullet.BulletType.MISSILE, BData[(int)Bullet.BulletType.MISSILE].GetPowerLevel());
                GameManager.Inst().TxtManager.SetBPrices((int)Bullet.BulletType.MISSILE, BData[(int)Bullet.BulletType.MISSILE].GetPrice());
                GameManager.Inst().UiManager.ShowDetail((int)Bullet.BulletType.MISSILE);

                //특수 효과
                if (BData[(int)Bullet.BulletType.MISSILE].GetPowerLevel() == 3)
                {
                    //BData[(int)Bullet.BulletType.MISSILE].SetSpeed(4.0f);
                    BData[(int)Bullet.BulletType.MISSILE].SetReloadTime(0.75f);
                    BData[(int)Bullet.BulletType.MISSILE].SetDuration(2.5f);
                }
                else if (BData[(int)Bullet.BulletType.MISSILE].GetPowerLevel() == 5)
                {
                    //BData[(int)Bullet.BulletType.MISSILE].SetSpeed(5.0f);
                    //BData[(int)Bullet.BulletType.MISSILE].SetReloadTime(0.5f);
                    BData[(int)Bullet.BulletType.MISSILE].SetDuration(3.0f);
                }
                    
                break;

            case UpgradeType.LASER:
                if (BData[(int)Bullet.BulletType.LASER].GetPowerLevel() >= BData[(int)Bullet.BulletType.LASER].GetMaxBulletLevel())
                    return;

                //가격
                if (GameManager.Inst().Player.GetCoin() < BData[(int)Bullet.BulletType.LASER].GetPrice())
                    return;
                else
                    GameManager.Inst().Player.MinusCoin(BData[(int)Bullet.BulletType.LASER].GetPrice());

                //BData 처리
                BData[(int)Bullet.BulletType.LASER].SetPowerLevel(BData[(int)Bullet.BulletType.LASER].GetPowerLevel() + 1);
                BData[(int)Bullet.BulletType.LASER].SetPrice();

                //UI
                GameManager.Inst().TxtManager.SetBLevels((int)Bullet.BulletType.LASER, BData[(int)Bullet.BulletType.LASER].GetPowerLevel());
                GameManager.Inst().TxtManager.SetBPrices((int)Bullet.BulletType.LASER, BData[(int)Bullet.BulletType.LASER].GetPrice());
                GameManager.Inst().UiManager.ShowDetail((int)Bullet.BulletType.LASER);

                //특수 효과
                if (BData[(int)Bullet.BulletType.LASER].GetPowerLevel() == 3)
                {
                    BData[(int)Bullet.BulletType.LASER].SetDuration(0.5f);
                    BData[(int)Bullet.BulletType.LASER].SetReloadTime(4.0f);
                }
                else if (BData[(int)Bullet.BulletType.LASER].GetPowerLevel() == 5)
                {
                    BData[(int)Bullet.BulletType.LASER].SetDuration(1.0f);
                    BData[(int)Bullet.BulletType.LASER].SetReloadTime(3.0f);
                }
                
                break;

            case UpgradeType.CHARGE:
                if (BData[(int)Bullet.BulletType.CHARGE].GetPowerLevel() >= BData[(int)Bullet.BulletType.CHARGE].GetMaxBulletLevel())
                    return;

                //가격
                if (GameManager.Inst().Player.GetCoin() < BData[(int)Bullet.BulletType.CHARGE].GetPrice())
                    return;
                else
                    GameManager.Inst().Player.MinusCoin(BData[(int)Bullet.BulletType.CHARGE].GetPrice());

                //BData 처리
                BData[(int)Bullet.BulletType.CHARGE].SetPowerLevel(BData[(int)Bullet.BulletType.CHARGE].GetPowerLevel() + 1);
                BData[(int)Bullet.BulletType.CHARGE].SetPrice();

                //UI
                GameManager.Inst().TxtManager.SetBLevels((int)Bullet.BulletType.CHARGE, BData[(int)Bullet.BulletType.CHARGE].GetPowerLevel());
                GameManager.Inst().TxtManager.SetBPrices((int)Bullet.BulletType.CHARGE, BData[(int)Bullet.BulletType.CHARGE].GetPrice());
                GameManager.Inst().UiManager.ShowDetail((int)Bullet.BulletType.CHARGE);

                //특수 효과
                if (BData[(int)Bullet.BulletType.CHARGE].GetPowerLevel() == 3)
                {
                    BData[(int)Bullet.BulletType.CHARGE].SetDuration(2.0f);
                    BData[(int)Bullet.BulletType.CHARGE].SetReloadTime(1.5f);

                }
                else if(BData[(int)Bullet.BulletType.CHARGE].GetPowerLevel() == 5)
                {
                    BData[(int)Bullet.BulletType.CHARGE].SetDuration(3.0f);
                    BData[(int)Bullet.BulletType.CHARGE].SetReloadTime(1.0f);
                }

                break;

            case UpgradeType.BOOMERANG:
                if (BData[(int)Bullet.BulletType.BOOMERANG].GetPowerLevel() >= BData[(int)Bullet.BulletType.BOOMERANG].GetMaxBulletLevel())

                //가격
                if (GameManager.Inst().Player.GetCoin() < BData[(int)Bullet.BulletType.BOOMERANG].GetPrice())
                    return;
                else
                    GameManager.Inst().Player.MinusCoin(BData[(int)Bullet.BulletType.BOOMERANG].GetPrice());

                //BData 처리
                BData[(int)Bullet.BulletType.BOOMERANG].SetPowerLevel(BData[(int)Bullet.BulletType.BOOMERANG].GetPowerLevel() + 1);
                BData[(int)Bullet.BulletType.BOOMERANG].SetPrice();

                //UI
                GameManager.Inst().TxtManager.SetBLevels((int)Bullet.BulletType.BOOMERANG, BData[(int)Bullet.BulletType.BOOMERANG].GetPowerLevel());
                GameManager.Inst().TxtManager.SetBPrices((int)Bullet.BulletType.BOOMERANG, BData[(int)Bullet.BulletType.BOOMERANG].GetPrice());
                GameManager.Inst().UiManager.ShowDetail((int)Bullet.BulletType.BOOMERANG);

                //특수 효과
                if (BData[(int)Bullet.BulletType.BOOMERANG].GetPowerLevel() == 3)
                {
                    BData[(int)Bullet.BulletType.BOOMERANG].SetDuration(4.5f);
                    BData[(int)Bullet.BulletType.BOOMERANG].SetReloadTime(1.5f);

                }
                else if (BData[(int)Bullet.BulletType.BOOMERANG].GetPowerLevel() == 5)
                {
                    BData[(int)Bullet.BulletType.BOOMERANG].SetDuration(7.0f);
                    BData[(int)Bullet.BulletType.BOOMERANG].SetReloadTime(1.0f);
                }

                break;

            case UpgradeType.CHAIN:
                if (BData[(int)Bullet.BulletType.CHAIN].GetPowerLevel() >= BData[(int)Bullet.BulletType.CHAIN].GetMaxBulletLevel())
                    return;

                //가격
                if (GameManager.Inst().Player.GetCoin() < BData[(int)Bullet.BulletType.CHAIN].GetPrice())
                    return;
                else
                    GameManager.Inst().Player.MinusCoin(BData[(int)Bullet.BulletType.CHAIN].GetPrice());

                //BData 처리
                BData[(int)Bullet.BulletType.CHAIN].SetPowerLevel(BData[(int)Bullet.BulletType.CHAIN].GetPowerLevel() + 1);
                BData[(int)Bullet.BulletType.CHAIN].SetPrice();

                //UI
                GameManager.Inst().TxtManager.SetBLevels((int)Bullet.BulletType.CHAIN, BData[(int)Bullet.BulletType.CHAIN].GetPowerLevel());
                GameManager.Inst().TxtManager.SetBPrices((int)Bullet.BulletType.CHAIN, BData[(int)Bullet.BulletType.CHAIN].GetPrice());
                GameManager.Inst().UiManager.ShowDetail((int)Bullet.BulletType.CHAIN);

                //특수 효과
                if (BData[(int)Bullet.BulletType.NORMAL].GetPowerLevel() == 3)
                    BData[(int)Bullet.BulletType.NORMAL].SetReloadTime(1.5f);
                else if (BData[(int)Bullet.BulletType.NORMAL].GetPowerLevel() == 5)
                    BData[(int)Bullet.BulletType.NORMAL].SetReloadTime(1.0f);
                break;

            case UpgradeType.SUBWEAPON:
                if (SubWeaponLevel[CurrentSubWeaponIndex] >= MAXSUBLEVEL)
                    return;

                //가격
                if (SubWeaponLevel[CurrentSubWeaponIndex] == 0)
                {
                    if (GameManager.Inst().Player.GetCoin() < SubWeaponBuyPrice)
                        return;
                    else
                        GameManager.Inst().Player.MinusCoin(SubWeaponBuyPrice);
                    SubWeaponBuyPrice *= 2;
                    if (SubWeaponBuyPrice >= 8000)
                        SubWeaponBuyPrice = 0;
                    AddSW();
                }
                else
                {
                    if (GameManager.Inst().Player.GetCoin() < SubWeaponPrice[CurrentSubWeaponIndex])
                        return;
                    else
                        GameManager.Inst().Player.MinusCoin(SubWeaponPrice[CurrentSubWeaponIndex]);
                }

                //Data 처리
                SubWeaponLevel[CurrentSubWeaponIndex]++;
                if (SubWeaponLevel[CurrentSubWeaponIndex] < 5)
                    SubWeaponPrice[CurrentSubWeaponIndex] = (int)Mathf.Pow(10, (float)(SubWeaponLevel[CurrentSubWeaponIndex] + 2));
                else
                    SubWeaponPrice[CurrentSubWeaponIndex] = 0;

                GameManager.Inst().GetSubweapons(CurrentSubWeaponIndex).SetHP(10 * SubWeaponLevel[CurrentSubWeaponIndex]);

                //UI
                //GameManager.Inst().TxtManager.SetSLevel(SubWeaponLevel);
                GameManager.Inst().TxtManager.SetSPrice(SubWeaponPrice[CurrentSubWeaponIndex]);
                if(SubWeaponLevel[CurrentSubWeaponIndex] >= 5)
                    GameManager.Inst().UiManager.GetBuySWUI().SetBuyBtnInteratable(false);
                GameManager.Inst().UiManager.SetSubWeaponInteratable(false);

                break;
        }*/
