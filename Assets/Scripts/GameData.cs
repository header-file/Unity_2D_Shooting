using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GameData
{
    public int Coin;

    public void SaveData()
    {
        Coin = GameManager.Inst().Player.GetCoin();
    }

    public void LoadData()
    {
        if (Coin > 0)
        {
            GameManager.Inst().Player.AddCoin(Coin);
        }
    }
}
