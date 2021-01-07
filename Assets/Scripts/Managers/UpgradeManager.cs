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
        SPLIT = 6,
        SUBWEAPON = 100
    };

    public struct BulletData
    {
        public void SetPowerLevel(int Level)
        {
            if (Level <= MaxBulletLevel)
                PowerLevel = Level;
        }
        public void SetPrice()
        {
            if (PowerLevel < MaxBulletLevel)
                Price = (int)Mathf.Pow(10.0f, (float)(PowerLevel + 1));
            else
                Price = 0;
        }
        public void SetBaseDamage(int damage) { BaseDamage = damage; }
        public void SetSpeed(float velocity) { Speed = velocity; }
        public void SetReloadTime(float Time) { ReloadTime = Time; }
        public void SetDuration(float Time) { Duration = Time; }
        public void SetAtk(int atk) { Atk = atk; }
        public void SetRng(int rng) { Rng = rng; }
        public void SetSpd(int spd) { Spd = spd; }
        
        public int GetMaxBulletLevel() { return MaxBulletLevel; }
        public int GetPowerLevel() { return PowerLevel; }
        public int GetPrice() { return Price; }
        public int GetAtk() { return Atk; }
        public int GetRng() { return Rng; }
        public int GetSpd() { return Spd; }
        public float GetSpeed() { return Speed; }
        public float GetReloadTime() { return ReloadTime; }
        public float GetDuration() { return Duration; }
        public int GetDamage() { return PowerLevel * BaseDamage; }
        
        public void ResetData()
        {
            PowerLevel = 0;
            BaseDamage = 0;

            Price = 10;

            ReloadTime = 0.0f;
            Duration = 0.0f;
            Speed = 0.0f;

            Atk = 0;
            Rng = 0;
            Spd = 0;
        }

        public void SetDatas(List<Dictionary<string, object>> data, int index)
        {
            BaseDamage = int.Parse(data[index]["BaseDamage"].ToString());
            PowerLevel = int.Parse(data[index]["StartLevel"].ToString());
            ReloadTime = float.Parse(data[index]["ReloadTime"].ToString());
            Duration = float.Parse(data[index]["Duration"].ToString());
            Speed = float.Parse(data[index]["Speed"].ToString());

            SetPrice();
        }

        const int MaxBulletLevel = 5;

        int PowerLevel;
        int BaseDamage;
        int Price;        

        float Speed;
        float ReloadTime;
        float Duration;

        int Atk;
        int Rng;
        int Spd;
    };

    public GameObject[] SubPositions;

    public static int MAXSUBLEVEL = 5;

    BulletData[] BData;
    int[] SubWeaponLevel;
    int[] SubWeaponPrice;
    int SubWeaponBuyPrice;
    int CurrentSubWeaponIndex;

    public BulletData GetBData(int index) { return BData[index]; }
    public int GetSubWeaponLevel(int index) { return SubWeaponLevel[index]; }
    public int GetSubWeaponPrice(int index) { return SubWeaponPrice[index]; }
    public int GetSubWeaponBuyPrice() { return SubWeaponBuyPrice; }

    public void SetCurrentSubWeaponIndex(int selectedIndex) { CurrentSubWeaponIndex = selectedIndex; }

    void Awake()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("Datas/BulletData");

        BData = new BulletData[Bullet.MAXBULLETS];
        for (int i = 0; i < BData.Length; i++)
        {
            BData[i].ResetData();
            BData[i].SetDatas(data, i);
        }

        SubWeaponLevel = new int[4];
        SubWeaponPrice = new int[4];
        for(int i = 0; i < 4; i++)
        {
            SubWeaponLevel[i] = 0;
            SubWeaponPrice[i] = 1000;
        }
        CurrentSubWeaponIndex = -1;
        SubWeaponBuyPrice = 1000;
    }

    void Start()
    {
        GameManager.Inst().TxtManager.SetBLevels((int)Bullet.BulletType.NORMAL, BData[(int)Bullet.BulletType.NORMAL].GetPowerLevel());
        //GameManager.Inst().TxtManager.SetBPrices((int)Bullet.BulletType.NORMAL, BData[(int)Bullet.BulletType.NORMAL].GetPrice());
    }

    public void AddLevel(int UpgType)
    {
        UpgradeType Type = (UpgradeType)UpgType;

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

            case UpgradeType.SPLIT:
                if (BData[(int)Bullet.BulletType.SPLIT].GetPowerLevel() >= BData[(int)Bullet.BulletType.SPLIT].GetMaxBulletLevel())
                    return;

                //가격
                if (GameManager.Inst().Player.GetCoin() < BData[(int)Bullet.BulletType.SPLIT].GetPrice())
                    return;
                else
                    GameManager.Inst().Player.MinusCoin(BData[(int)Bullet.BulletType.SPLIT].GetPrice());

                //BData 처리
                BData[(int)Bullet.BulletType.SPLIT].SetPowerLevel(BData[(int)Bullet.BulletType.SPLIT].GetPowerLevel() + 1);
                BData[(int)Bullet.BulletType.SPLIT].SetPrice();

                //UI
                GameManager.Inst().TxtManager.SetBLevels((int)Bullet.BulletType.SPLIT, BData[(int)Bullet.BulletType.SPLIT].GetPowerLevel());
                GameManager.Inst().TxtManager.SetBPrices((int)Bullet.BulletType.SPLIT, BData[(int)Bullet.BulletType.SPLIT].GetPrice());
                GameManager.Inst().UiManager.ShowDetail((int)Bullet.BulletType.SPLIT);

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
        }
    }

    public void AddSW()
    {
        GameObject subWeapon = GameManager.Inst().ObjManager.MakeObj("SubWeapon");
        //int index = GameManager.Inst().UiManager.NewWindows[(int)UIManager.NewWindowType.BUYSUBWEAPON].GetComponent<BuySubWeapon>().GetSelectedIndex();
        int index = CurrentSubWeaponIndex;
        Vector3 pos = SubPositions[CurrentSubWeaponIndex].transform.position;
        subWeapon.transform.position = pos;
        SubWeapon sub = subWeapon.GetComponent<SubWeapon>();

        GameManager.Inst().SetSubWeapons(sub, CurrentSubWeaponIndex);
        if (index > 1)
            index++;
        subWeapon.GetComponent<SubWeapon>().SetNumID(index);
    }

    public void Update()
    {
        /*Player player = GameObject.Find("Player").gameObject.GetComponent<Player>();
        if (Input.GetMouseButtonUp(2))
            AddLevel((UpgradeType)player.GetBulletType());
        else if (Input.GetKeyUp(KeyCode.RightArrow))
            player
        else if (Input.GetKeyUp(KeyCode.UpArrow))
            AddLevel("SWLaser");
        else if (Input.GetKeyUp(KeyCode.DownArrow))
            AddLevel("Laser");
        if (Input.GetKeyUp(KeyCode.Space))
            AddLevel(UpgradeType.SUBWEAPON);*/
    }
}
