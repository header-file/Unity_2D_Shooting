﻿using System.Collections;
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
    public Text WeaponName;
    public Image[] SlotImgs;
    public GameObject[] Grades;
    public GameObject[] Gauges;
    public CanvasGroup[] CanvasGps;
    public Animator[] Anim;
    

    public void SetWeaponName(int index) { WeaponName.text = GameManager.Inst().Player.Types[index]; }

    public void SetAlpha(float alpha)
    {
        for (int i = 0; i < CanvasGps.Length; i++)
            CanvasGps[i].alpha = alpha;
    }

    public void SetAnimTrigger(string trigger)
    {
        for (int i = 0; i < 3; i++)
            Anim[i].SetTrigger(trigger);
    }

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

    public void SetSlots(int index, bool b, Sprite img, int grade)
    {
        if (b)
        {
            SlotImgs[index].sprite = img;
            SlotImgs[index + 3].gameObject.SetActive(false);

            for(int i = 0; i < 5; i++)
                Grades[index].transform.GetChild(i).gameObject.SetActive(false);

            Grades[index].transform.GetChild(grade).gameObject.SetActive(true);
        }
        else
        {
            SlotImgs[index].sprite = img;
            SlotImgs[index + 3].gameObject.SetActive(true);

            for (int i = 0; i < 5; i++)
                Grades[index].transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void PaintGauge(int type, float value, Color color)
    {
        for (int i = 1; i <= 10; i++)
        {
            if (value >= i * 10)
                Gauges[type].transform.GetChild(i - 1).gameObject.GetComponent<Image>().color = color;
            else
                Gauges[type].transform.GetChild(i - 1).gameObject.GetComponent<Image>().color = Color.white;
        }
    }
}
