using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public GameObject[] BulletNames;
    public GameObject[] BulletLevels;
    public GameObject SubPrice;
    public GameObject SubName;
    public GameObject[] CoolTimes;

    Text[] BNames;
    Text[] BLevels;
    string[] BPrices;

    public string GetBNames(int index) { return BNames[index].text; }
    public string GetBLevels(int index) { return BLevels[index].text; }
    public string GetBPrices(int index) { return BPrices[index]; }

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
    public void SetBPrices(int index, int price) { BPrices[index] = price.ToString(); }
    /*public void SetSLevel(int level)
    {
        if (level < 4)
            SubLevel.GetComponent<Text>().text = level.ToString();
        else
            SubLevel.GetComponent<Text>().text = "MAX";
    }*/
    public void SetSPrice(int price) { SubPrice.GetComponent<Text>().text = price.ToString(); }
    public void SetSName(int index) { SubName.GetComponent<Text>().text = "Turret - 0" + (index + 1).ToString(); }
    public void SetCoolTimes(int index, int time)
    {
        int min = time / 60;
        int sec = time % 60;

        string text = "";
        if (min >= 10)
            text += min.ToString();
        else
            text += ("0" + min.ToString());

        text += ":";

        if (sec >= 10)
            text += sec.ToString();
        else
            text += ("0" + sec.ToString());

        CoolTimes[index].GetComponent<Text>().text = text;
    }

    void Awake()
    {
        BNames = new Text[Bullet.MAXBULLETS];
        BLevels = new Text[Bullet.MAXBULLETS];
        BPrices = new string[Bullet.MAXBULLETS];

        for (int i = 0; i < Bullet.MAXBULLETS; i++)
        {
            BNames[i] = BulletNames[i].GetComponent<Text>();
            BLevels[i] = BulletLevels[i].GetComponent<Text>();
            BPrices[i] = "0";
        }

        BNames[0].text = "Normal";
        BNames[1].text = "Spread";
        BNames[2].text = "Missile";
        BNames[3].text = "Laser";
        BNames[4].text = "Charge";
        BNames[5].text = "Boomerang";
        BNames[6].text = "Split";
    }

    void Start()
    {
        for (int i = 0; i < Bullet.MAXBULLETS; i++)
            SetBLevels(i, GameManager.Inst().UpgManager.GetBData(i).GetPowerLevel());
    }
}
