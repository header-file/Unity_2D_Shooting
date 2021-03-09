using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GameData
{
    public struct InventoryData
    {
        int Type;
        int Grade;
        int Value;
    }

    public int Coin;
    public int[] Resources;
    public int CurrentStage;
    public int[,] SubWeaponLevels;
    public UpgradeManager.BulletData[] Weapons;
    public int[,,] Equippeds;
    public InventoryData[] Inventories;

    public int[] BossGauges;

    void Start()
    {
        Resources = new int[StageManager.MAXSTAGES];
        SubWeaponLevels = new int[StageManager.MAXSTAGES, 4];
        Weapons = new UpgradeManager.BulletData[Bullet.MAXBULLETS];
        Equippeds = new int[StageManager.MAXSTAGES, Bullet.MAXBULLETS, 3];
        Inventories = new InventoryData[GameManager.Inst().Player.MAXINVENTORY];

        BossGauges = new int[StageManager.MAXSTAGES];
    }

    public void SaveData()
    {
        Coin = GameManager.Inst().Player.GetCoin();
        //for(int i = 0; i < StageManager.MAXSTAGES; i++)
        //{
        //    Resources[i] = GameManager.Inst().Resources[i];
            
        //}
    }

    public void LoadData()
    {
        if (Coin > 0)
            GameManager.Inst().Player.AddCoin(Coin);
    }
}
