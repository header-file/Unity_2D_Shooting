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
    public Text[] Resources;
    public Text BossTimer;
    public string[] EquipName;
    public string[] EquipDetailFront;
    public string[] EquipDetailBack;
    public string[] EquipDetailSimple;

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
        if (level < GameManager.Inst().UpgManager.BData[index].GetMaxBulletLevel())
            BLevels[index].text = "Lv." + level.ToString();
        else
            BLevels[index].text = "Lv." + "MAX";

    }
    public void SetBPrices(int index, int price) { BPrices[index] = price.ToString(); }
    public void SetSPrice(int price) { SubPrice.GetComponent<Text>().text = price.ToString(); }
    public void SetSName(int index) { SubName.GetComponent<Text>().text = SubNames[index]; }
    

    void Awake()
    {
        GameManager.Inst().TxtManager = gameObject.GetComponent<TextManager>();

        BNames = new Text[Constants.MAXBULLETS];
        BLevels = new Text[Constants.MAXBULLETS];
        BPrices = new string[Constants.MAXBULLETS];

        BulNames = new string[Constants.MAXBULLETS];
        BulNames[0] = "Normal";
        BulNames[1] = "Spread";
        BulNames[2] = "Missile";
        BulNames[3] = "Laser";
        BulNames[4] = "Charge";
        BulNames[5] = "Boomerang";
        BulNames[6] = "Chain";

        SubNames = new string[4];
        for(int i = 0; i < Constants.MAXSUBWEAPON; i++)
           SubNames[i] = "Turret - 0" + (i + 1).ToString();

        for (int i = 0; i < Constants.MAXBULLETS; i++)
        {
            BNames[i] = BulletNames[i].GetComponent<Text>();
            BNames[i].text = BulNames[i];
            BLevels[i] = BulletLevels[i].GetComponent<Text>();
            BPrices[i] = "0";
        }
    }

    void Start()
    {
        for (int i = 0; i < Constants.MAXBULLETS; i++)
            SetBLevels(i, GameManager.Inst().UpgManager.BData[i].GetPowerLevel());
    }

    void FixedUpdate()
    {
        if (!GameManager.Inst().StgManager.IsBoss)
            return;

        float time = GameManager.Inst().StgManager.BossTimer;
        time = (float)System.Math.Truncate((double)time * 100) / 100;
        BossTimer.text = time.ToString();

        GameManager.Inst().StgManager.BossTimer -= Time.deltaTime;

        if(GameManager.Inst().StgManager.BossTimer <= 0)
        {
            GameManager.Inst().StgManager.Boss.Die();

            GameManager.Inst().StgManager.IsBoss = false;
            GameManager.Inst().StgManager.RestartStage();
        }
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
