using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public GameObject[] BulletNames;
    public GameObject[] BulletLevels;
    public GameObject[] BulletPrices;
    //public GameObject SubLevel;
    public GameObject SubPrice;
    public GameObject SubName;

    Text[] BNames;
    Text[] BLevels;
    Text[] BPrices;

    public string GetBNames(int index) { return BNames[index].text; }
    public string GetBLevels(int index) { return BLevels[index].text; }
    public string GetBPrices(int index) { return BPrices[index].text; }

    /*public void SetBNames(int index)
    {
        switch (index)
        {

        }
    }*/

    public void SetBLevels(int index, int level)
    {
        if (level < 5)
            BLevels[index].text = "Lv." + level.ToString();
        else
            BLevels[index].text = "Lv." + "MAX";

    }
    public void SetBPrices(int index, int price) { BPrices[index].text = price.ToString(); }

    /*public void SetSLevel(int level)
    {
        if (level < 4)
            SubLevel.GetComponent<Text>().text = level.ToString();
        else
            SubLevel.GetComponent<Text>().text = "MAX";
    }*/
    public void SetSPrice(int price) { SubPrice.GetComponent<Text>().text = price.ToString(); }
    public void SetSName(int index) { SubName.GetComponent<Text>().text = "Turret - 0" + (index + 1).ToString(); }

    void Awake()
    {
        BNames = new Text[5];
        BLevels = new Text[5];
        BPrices = new Text[5];

        for (int i = 0; i < Bullet.MAXBULLETS; i++)
        {
            BNames[i] = BulletNames[i].GetComponent<Text>();
            BLevels[i] = BulletLevels[i].GetComponent<Text>();
            BPrices[i] = BulletPrices[i].GetComponent<Text>();
        }

        BNames[0].text = "Normal";
        BNames[1].text = "Spread";
        BNames[2].text = "Missile";
        BNames[3].text = "Laser";
        BNames[4].text = "Charge";
    }

    void Start()
    {
        for (int i = 0; i < Bullet.MAXBULLETS; i++)
        {
            SetBLevels(i, GameManager.Inst().UpgManager.GetBData(i).GetPowerLevel());
        }
    }
}
