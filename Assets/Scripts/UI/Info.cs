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
    public GameObject[] ColorChips;

    int CharacterType;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetColorSelected(int index)
    {
        for (int i = 0; i < GameManager.Inst().ShtManager.MAXCOLOR; i++)
        {
            ColorSelected[i].SetActive(false);
            ColorChips[i].transform.localScale = Vector3.one;
        }

        ColorSelected[index].SetActive(true);
        ColorChips[index].transform.localScale = Vector3.one * 1.5f;
    }

    public void ShowInfo(int Type, int CurrentBulletType)
    {
        CharacterType = Type;
        WeaponName.text = GameManager.Inst().TxtManager.GetBulNames(CurrentBulletType);
        WeaponImage.sprite = GameManager.Inst().UiManager.WeaponImages[CurrentBulletType];

        for (int i = 0; i < GameManager.Inst().ShtManager.MAXCOLOR; i++)
        {
            ColorSelected[i].SetActive(false);
            ColorChips[i].transform.localScale = Vector3.one;
        }
        ColorSelected[GameManager.Inst().ShtManager.GetColorSelection(Type)].SetActive(true);
        ColorChips[GameManager.Inst().ShtManager.GetColorSelection(Type)].transform.localScale = Vector3.one * 1.5f;

        if (Type == 2)
        {
            Name.text = "Player";
            Level.text = "Lv." + GameManager.Inst().UpgManager.GetPlayerLevel().ToString();
            UpgBtn.interactable = true;
            SubPrice.text = GameManager.Inst().UpgManager.GetPlayerUpgradePriceData(GameManager.Inst().UpgManager.GetPlayerLevel()).ToString();
        }
        else
        {
            if(Type > 2)
            {
                int type = Type - 1;
                Name.text = GameManager.Inst().TxtManager.GetSubNames(type);
                Level.text = "Lv." + GameManager.Inst().UpgManager.GetSubWeaponLevel(GameManager.Inst().StgManager.Stage, type).ToString();
                if (GameManager.Inst().UpgManager.GetSubWeaponLevel(GameManager.Inst().StgManager.Stage, type) >= Constants.MAXSUBLEVEL)
                    UpgBtn.interactable = false;
                else
                    UpgBtn.interactable = true;
                SubPrice.text = GameManager.Inst().UpgManager.GetSubWeaponPrice(type).ToString();

                GameManager.Inst().UpgManager.SetCurrentSubWeaponIndex(type);
            }
            else
            {
                Name.text = GameManager.Inst().TxtManager.GetSubNames(Type);
                Level.text = "Lv." + GameManager.Inst().UpgManager.GetSubWeaponLevel(GameManager.Inst().StgManager.Stage, Type).ToString();
                if (GameManager.Inst().UpgManager.GetSubWeaponLevel(GameManager.Inst().StgManager.Stage, Type) >= Constants.MAXSUBLEVEL)
                    UpgBtn.interactable = false;
                else
                    UpgBtn.interactable = true;
                SubPrice.text = GameManager.Inst().UpgManager.GetSubWeaponPrice(Type).ToString();

                GameManager.Inst().UpgManager.SetCurrentSubWeaponIndex(Type);
            }
        } 
    }

    public void Buy(int characterType)
    {
        if (characterType == 2)
        {
            GameManager.Inst().UpgManager.AddLevel((int)UpgradeManager.UpgradeType.PLAYER);

            Level.text = "Lv." + GameManager.Inst().UpgManager.GetPlayerLevel().ToString();
            if (GameManager.Inst().UpgManager.GetPlayerLevel() >= Constants.MAXPLAYERLEVEL)
                UpgBtn.interactable = false;
            else
                UpgBtn.interactable = true;
            SubPrice.text = GameManager.Inst().UpgManager.GetPlayerUpgradePriceData(GameManager.Inst().UpgManager.GetPlayerLevel()).ToString();
        }
        else
        {
            GameManager.Inst().UpgManager.AddLevel((int)UpgradeManager.UpgradeType.SUBWEAPON);

            if (CharacterType > 2)
            {
                int type = CharacterType - 1;
                Level.text = "Lv." + GameManager.Inst().UpgManager.GetSubWeaponLevel(GameManager.Inst().StgManager.Stage, type).ToString();
                if (GameManager.Inst().UpgManager.GetSubWeaponLevel(GameManager.Inst().StgManager.Stage, type) >= Constants.MAXSUBLEVEL)
                    UpgBtn.interactable = false;
                else
                    UpgBtn.interactable = true;
                SubPrice.text = GameManager.Inst().UpgManager.GetSubWeaponPrice(type).ToString();
            }
            else
            {
                Level.text = "Lv." + GameManager.Inst().UpgManager.GetSubWeaponLevel(GameManager.Inst().StgManager.Stage, CharacterType).ToString();
                if (GameManager.Inst().UpgManager.GetSubWeaponLevel(GameManager.Inst().StgManager.Stage, CharacterType) >= Constants.MAXSUBLEVEL)
                    UpgBtn.interactable = false;
                else
                    UpgBtn.interactable = true;
                SubPrice.text = GameManager.Inst().UpgManager.GetSubWeaponPrice(CharacterType).ToString();
            }
        }
    }
}
