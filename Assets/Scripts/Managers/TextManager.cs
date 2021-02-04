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
    public Text[] Resources;

    Text[] BNames;
    Text[] BLevels;
    string[] BPrices;

    string[] SubNames;
    string[] BulNames;

    public string GetBNames(int index) { return BNames[index].text; }
    public string GetBLevels(int index) { return BLevels[index].text; }
    public string GetBPrices(int index) { return BPrices[index]; }
    public string GetBulNames(int index) { return BulNames[index]; }
    public string GetSubNames(int index) { return SubNames[index]; }

    public void SetBLevels(int index, int level)
    {
        if (level < 5)
            BLevels[index].text = "Lv." + level.ToString();
        else
            BLevels[index].text = "Lv." + "MAX";

    }
    public void SetBPrices(int index, int price) { BPrices[index] = price.ToString(); }
    public void SetSPrice(int price) { SubPrice.GetComponent<Text>().text = price.ToString(); }
    public void SetSName(int index) { SubName.GetComponent<Text>().text = SubNames[index]; }
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

        BulNames = new string[Bullet.MAXBULLETS];
        BulNames[0] = "Normal";
        BulNames[1] = "Spread";
        BulNames[2] = "Missile";
        BulNames[3] = "Laser";
        BulNames[4] = "Charge";
        BulNames[5] = "Boomerang";
        BulNames[6] = "Chain";

        SubNames = new string[4];
        for(int i = 0; i < 4; i++)
           SubNames[i] = "Turret - 0" + (i + 1).ToString();

        for (int i = 0; i < Bullet.MAXBULLETS; i++)
        {
            BNames[i] = BulletNames[i].GetComponent<Text>();
            BNames[i].text = BulNames[i];
            BLevels[i] = BulletLevels[i].GetComponent<Text>();
            BPrices[i] = "0";
        }

        
    }

    void Start()
    {
        for (int i = 0; i < Bullet.MAXBULLETS; i++)
            SetBLevels(i, GameManager.Inst().UpgManager.GetBData(i).GetPowerLevel());

        for (int i = 0; i < 4; i++)
            CoolTimes[i].SetActive(false);
    }

    public void ShowDmgText(Vector3 pos, float dmg)
    {
        GameObject text = GameManager.Inst().ObjManager.MakeObj("DamageText");
        text.transform.position = pos;
        text.SetActive(true);
        DamageText dmgText = text.GetComponent<DamageText>();
        dmgText.SetText(dmg);

        ActivationTimer timer = text.GetComponent<ActivationTimer>();
        timer.IsStart = true;
    }
}
