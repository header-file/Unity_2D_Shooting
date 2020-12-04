using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Detail : MonoBehaviour
{
    public GameObject Name;
    public GameObject CurrentLevel;
    public GameObject NextLevel;
    public GameObject Price;

    int BulletType;

    public void SetBulletType(int Type) { BulletType = Type; }

    void Awake()
    {
        BulletType = -1;
    }

    void Update()
    {
        
    }

    public void SetDetails()
    {
        Name.GetComponent<Text>().text = GameManager.Inst().TxtManager.GetBNames(BulletType);
        string lv = GameManager.Inst().TxtManager.GetBLevels(BulletType);
        CurrentLevel.GetComponent<Text>().text = lv;
        Price.GetComponent<Text>().text = GameManager.Inst().TxtManager.GetBPrices(BulletType);

        int level = GameManager.Inst().UpgManager.GetBData(BulletType).GetPowerLevel();
        if (level < 4)
            NextLevel.GetComponent<Text>().text = "Lv" + (level + 1).ToString();
        else if (level == 4)
            NextLevel.GetComponent<Text>().text = "Lv" + "MAX";
        else
            NextLevel.GetComponent<Text>().text = " ";
    }

    public void OnClickUpgradeBtn()
    {
        GameManager.Inst().UpgManager.AddLevel(BulletType);
    }
}
