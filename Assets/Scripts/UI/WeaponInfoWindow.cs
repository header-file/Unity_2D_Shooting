﻿using System.Collections;
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

        ATKAfter.text = (GameManager.Inst().UpgManager.BData[type].GetDamage() + 1).ToString();
        HPAfter.text = (GameManager.Inst().UpgManager.BData[type].GetHealth() + 5).ToString();
        SPDAfter.text = (GameManager.Inst().UpgManager.BData[type].GetSpeed()).ToString();
    }
}
