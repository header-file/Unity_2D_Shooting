using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInfoWindow : MonoBehaviour
{
    public Text ATKBefore;
    public Text HPBefore;
    public Text SPDBefore;
    public Text ATKAfter;
    public Text HPAfter;
    public Text SPDAfter;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Show(int type)
    {
        ATKBefore.text = GameManager.Inst().UpgManager.BData[type].GetDamage().ToString();
        HPBefore.text = GameManager.Inst().UpgManager.BData[type].GetHealth().ToString();
        SPDBefore.text = GameManager.Inst().UpgManager.BData[type].GetSpeed().ToString();

        if (GameManager.Inst().UpgManager.BData[type].GetPowerLevel() == GameManager.Inst().UpgManager.BData[type].GetMaxBulletLevel() &&
            GameManager.Inst().UpgManager.BData[type].GetRarity() < (Constants.MAXRARITY - 1))
        {
            ATKAfter.text = (GameManager.Inst().UpgManager.BulletDatas[type + (GameManager.Inst().UpgManager.BData[type].GetRarity() + 1) * (Constants.MAXBULLETS + 2)].GetDamage()).ToString();
            HPAfter.text = ((GameManager.Inst().UpgManager.BData[type].GetRarity() + 2) * 150 + GameManager.Inst().UpgManager.BData[type].GetPowerLevel() * 3).ToString();
            SPDAfter.text = (GameManager.Inst().UpgManager.BulletDatas[type + (GameManager.Inst().UpgManager.BData[type].GetRarity() + 1) * (Constants.MAXBULLETS + 2)].GetSpeed()).ToString();
        }
        else if (GameManager.Inst().UpgManager.BData[type].GetRarity() >= (Constants.MAXRARITY - 1))
        {
            ATKAfter.text = "MAX";
            HPAfter.text = "MAX";
            SPDAfter.text = "MAX";
        }
        else
        {
            ATKAfter.text = (GameManager.Inst().UpgManager.BData[type].GetDamage() + 1).ToString();
            HPAfter.text = ((GameManager.Inst().UpgManager.BData[type].GetRarity() + 1) * 150 + (GameManager.Inst().UpgManager.BData[type].GetPowerLevel() + 1) * 3).ToString();
            SPDAfter.text = GameManager.Inst().UpgManager.BData[type].GetSpeed().ToString();
        }
    }
}
