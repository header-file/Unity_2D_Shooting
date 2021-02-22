using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoArea : MonoBehaviour
{
    public Text Level;
    public Text CoinText;
    public GameObject Coin;
    public GameObject Resource;
    public GameObject[] Resources;
    public Text[] ResourceTexts;

    public void ShowDetail(int bulletType)
    {
        Level.text = "Level " + GameManager.Inst().UpgManager.GetBData(bulletType).GetPowerLevel().ToString();

        if(GameManager.Inst().UpgManager.GetBData(bulletType).GetPowerLevel() < (GameManager.Inst().UpgManager.GetBData(bulletType).GetRarity() + 1) * 10)
        {
            Coin.SetActive(true);
            Resource.SetActive(false);
            CoinText.text = GameManager.Inst().UpgManager.GetBData(bulletType).GetPrice().ToString();
        }
        else
        {
            Coin.SetActive(false);
            Resource.SetActive(true);

            for(int i = 0; i < StageManager.MAXSTAGES; i++)
            {
                ResourceTexts[i].text = GameManager.Inst().UpgManager.GetResourceData(GameManager.Inst().UpgManager.GetBData(bulletType).GetRarity(), i).ToString();

                if (ResourceTexts[i].text == "0")
                    Resources[i].SetActive(false);
                else
                    Resources[i].SetActive(true);
            } 
        }
    }
}
