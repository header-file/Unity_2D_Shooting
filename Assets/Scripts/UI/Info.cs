﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    public Text Name;
    public Text Level;
    public Button UpgBtn;
    public Text SubPrice;
    public Text WeaponName;
    public Image WeaponImage;
    public GameObject[] ColorSelected;

    int CharacterType;

    public void SetColorSelected(int index)
    {
        for (int i = 0; i < GameManager.Inst().ShtManager.MAXCOLOR; i++)
            ColorSelected[i].SetActive(false);

        ColorSelected[index].SetActive(true);
    }

    public void ShowInfo(int Type, int CurrentBulletType)
    {
        CharacterType = Type;
        WeaponName.text = GameManager.Inst().TxtManager.GetBulNames(CurrentBulletType);
        WeaponImage.sprite = GameManager.Inst().UiManager.WeaponImages[CurrentBulletType];

        for (int i = 0; i < GameManager.Inst().ShtManager.MAXCOLOR; i++)
            ColorSelected[i].SetActive(false);
        ColorSelected[GameManager.Inst().ShtManager.GetColorSelection(Type)].SetActive(true);

        if (Type == 2)
        {
            Name.text = "Player";
            Level.text = "";
            UpgBtn.interactable = false;
            SubPrice.text = "0";
        }
        else
        {
            if(Type > 2)
            {
                int type = Type - 1;
                Name.text = GameManager.Inst().TxtManager.GetSubNames(type);
                Level.text = "Lv." + GameManager.Inst().UpgManager.GetSubWeaponLevel(type).ToString();
                if (GameManager.Inst().UpgManager.GetSubWeaponLevel(type) >= UpgradeManager.MAXSUBLEVEL)
                    UpgBtn.interactable = false;
                else
                    UpgBtn.interactable = true;
                SubPrice.text = GameManager.Inst().UpgManager.GetSubWeaponPrice(type).ToString();

                GameManager.Inst().UpgManager.SetCurrentSubWeaponIndex(type);
            }
            else
            {
                Name.text = GameManager.Inst().TxtManager.GetSubNames(Type);
                Level.text = "Lv." + GameManager.Inst().UpgManager.GetSubWeaponLevel(Type).ToString();
                if (GameManager.Inst().UpgManager.GetSubWeaponLevel(Type) >= UpgradeManager.MAXSUBLEVEL)
                    UpgBtn.interactable = false;
                else
                    UpgBtn.interactable = true;
                SubPrice.text = GameManager.Inst().UpgManager.GetSubWeaponPrice(Type).ToString();

                GameManager.Inst().UpgManager.SetCurrentSubWeaponIndex(Type);
            }
        } 
    }

    public void Buy()
    {
        GameManager.Inst().UpgManager.AddLevel((int)UpgradeManager.UpgradeType.SUBWEAPON);
        
        if (CharacterType > 2)
        {
            int type = CharacterType - 1;
            Level.text = "Lv." + GameManager.Inst().UpgManager.GetSubWeaponLevel(type).ToString();
            if (GameManager.Inst().UpgManager.GetSubWeaponLevel(type) >= UpgradeManager.MAXSUBLEVEL)
                UpgBtn.interactable = false;
            else
                UpgBtn.interactable = true;
            SubPrice.text = GameManager.Inst().UpgManager.GetSubWeaponPrice(type).ToString();
        }
        else
        {
            Level.text = "Lv." + GameManager.Inst().UpgManager.GetSubWeaponLevel(CharacterType).ToString();
            if (GameManager.Inst().UpgManager.GetSubWeaponLevel(CharacterType) >= UpgradeManager.MAXSUBLEVEL)
                UpgBtn.interactable = false;
            else
                UpgBtn.interactable = true;
            SubPrice.text = GameManager.Inst().UpgManager.GetSubWeaponPrice(CharacterType).ToString();
        }
    }
}
