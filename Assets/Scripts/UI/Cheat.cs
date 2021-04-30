using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cheat : MonoBehaviour
{
    public Toggle[] Toggles;
    public GameObject[] Pages;

    public Button[] CharReviveBtns;
    public Button[] CharGodModeBtns;
    public Button[] CharCreateBtns;
    public Button[] CharDeleteBtns;
    public Text[] GodModeBtnTexts;

    public Button PlusLevelBtn;
    public Button MinusLevelBtn;
    public Text BulletName;
    public Text BulletLevelText;
    public Text RarityText;
    public Text[] StatusText;
    public Button[] PlusReinforceBtn;
    public Button[] MinusReinforceBtn;
    public Text[] ReinforceText;
    public Image[] ReinforceImage;

    int CurrentBulletType;


    void Start()
    {
        //Toggles[0].isOn = true;

        CurrentBulletType = 0;
        gameObject.SetActive(false);
    }

    public void SwitchPage(int index)
    {
        if (Toggles[index].isOn == false)
            return;

        for (int i = 0; i < Pages.Length; i++)
            Pages[i].SetActive(false);

        Toggles[index].Select();
        Pages[index].SetActive(true);
        ShowPage(index);
    }

    void ShowPage(int index)
    {
        switch(index)
        {
            case 0:
                ShowCharacterPage();
                break;
            case 1:
                ShowWeaponPage();
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
        }
    }

    void ShowCharacterPage()
    {
        for(int i = 0; i < Constants.MAXSUBWEAPON; i++)
        {
            if (GameManager.Inst().GetSubweapons(i) == null)
                CharacterOffButtons(i);
            else
                CharacterOnButtons(i);
        }
    }

    void CharacterOnButtons(int i)
    {
        CharCreateBtns[i].interactable = false;
        CharDeleteBtns[i].interactable = true;
        CharReviveBtns[i + 1].interactable = true;
        CharGodModeBtns[i + 1].interactable = true;
    }

    void CharacterOffButtons(int i)
    {
        CharCreateBtns[i].interactable = true;
        CharDeleteBtns[i].interactable = false;
        CharReviveBtns[i + 1].interactable = false;
        CharGodModeBtns[i + 1].interactable = false;
    }

    void ShowWeaponPage()
    {
        CurrentBulletType = 0;

        ShowWeaponData();
    }

    void ShowWeaponData()
    {
        BulletName.text = GameManager.Inst().TxtManager.BulletTypeNames[CurrentBulletType];
        BulletLevelText.text = GameManager.Inst().UpgManager.BData[CurrentBulletType].GetPowerLevel().ToString();
        RarityText.text = GameManager.Inst().TxtManager.RarityNames[GameManager.Inst().UpgManager.BData[CurrentBulletType].GetRarity()];

        StatusText[0].text = GameManager.Inst().UpgManager.BData[CurrentBulletType].GetDamage().ToString();
        StatusText[1].text = GameManager.Inst().UpgManager.BData[CurrentBulletType].GetSpeed().ToString();
        StatusText[2].text = GameManager.Inst().UpgManager.BData[CurrentBulletType].GetHealth().ToString();

        ReinforceText[0].text = GameManager.Inst().UpgManager.BData[CurrentBulletType].GetAtk().ToString();
        ReinforceText[1].text = GameManager.Inst().UpgManager.BData[CurrentBulletType].GetSpd().ToString();
        ReinforceText[2].text = GameManager.Inst().UpgManager.BData[CurrentBulletType].GetHp().ToString();

        ReinforceImage[0].fillAmount = (float)GameManager.Inst().UpgManager.BData[CurrentBulletType].GetAtk() / GameManager.Inst().UpgManager.BData[CurrentBulletType].GetMaxAtk();
        ReinforceImage[1].fillAmount = (float)GameManager.Inst().UpgManager.BData[CurrentBulletType].GetSpd() / GameManager.Inst().UpgManager.BData[CurrentBulletType].GetMaxSpd();
        ReinforceImage[2].fillAmount = (float)GameManager.Inst().UpgManager.BData[CurrentBulletType].GetHp() / GameManager.Inst().UpgManager.BData[CurrentBulletType].GetMaxHp();

        if (GameManager.Inst().UpgManager.BData[CurrentBulletType].GetRarity() == Constants.MAXRARITY - 1 &&
            GameManager.Inst().UpgManager.BData[CurrentBulletType].GetPowerLevel() >= GameManager.Inst().UpgManager.BData[CurrentBulletType].GetMaxBulletLevel())
            PlusLevelBtn.interactable = false;
        else
            PlusLevelBtn.interactable = true;

        if (GameManager.Inst().UpgManager.BData[CurrentBulletType].GetRarity() == 0 &&
            GameManager.Inst().UpgManager.BData[CurrentBulletType].GetPowerLevel() <= 1)
            MinusLevelBtn.interactable = false;
        else
            MinusLevelBtn.interactable = true;

        if (GameManager.Inst().UpgManager.BData[CurrentBulletType].GetAtk() >= GameManager.Inst().UpgManager.BData[CurrentBulletType].GetMaxAtk())
            PlusReinforceBtn[0].interactable = false;
        else
            PlusReinforceBtn[0].interactable = true;

        if (GameManager.Inst().UpgManager.BData[CurrentBulletType].GetAtk() <= 0)
            MinusReinforceBtn[0].interactable = false;
        else
            MinusReinforceBtn[0].interactable = true;

        if (GameManager.Inst().UpgManager.BData[CurrentBulletType].GetSpd() >= GameManager.Inst().UpgManager.BData[CurrentBulletType].GetMaxSpd())
            PlusReinforceBtn[1].interactable = false;
        else
            PlusReinforceBtn[1].interactable = true;

        if (GameManager.Inst().UpgManager.BData[CurrentBulletType].GetSpd() <= 0)
            MinusReinforceBtn[1].interactable = false;
        else
            MinusReinforceBtn[1].interactable = true;

        if (GameManager.Inst().UpgManager.BData[CurrentBulletType].GetHp() >= GameManager.Inst().UpgManager.BData[CurrentBulletType].GetMaxHp())
            PlusReinforceBtn[2].interactable = false;
        else
            PlusReinforceBtn[2].interactable = true;

        if (GameManager.Inst().UpgManager.BData[CurrentBulletType].GetHp() <= 0)
            MinusReinforceBtn[2].interactable = false;
        else
            MinusReinforceBtn[2].interactable = true;
    }

    //Button
    public void OnClickRevive(int index)
    {
        if(index == 0)
            GameManager.Inst().Player.Revive();
        else
        {
            int i = index - 1;
            GameManager.Inst().GetSubweapons(i).Revive();
        }
    }

    public void OnClickInvincible(int index)
    {
        if (index == 0)
        {
            if (GameManager.Inst().Player.IsGodMode == true)
            {
                GameManager.Inst().Player.IsGodMode = false;
                GodModeBtnTexts[index].color = Color.white;
            }
            else
            {
                GameManager.Inst().Player.IsGodMode = true;
                GodModeBtnTexts[index].color = Color.yellow;
            }
        }
            
        else
        {
            int i = index - 1;

            if (GameManager.Inst().GetSubweapons(i).IsGodMode == true)
            {
                GameManager.Inst().GetSubweapons(i).IsGodMode = false;
                GodModeBtnTexts[index].color = Color.white;
            }
            else
            {
                GameManager.Inst().GetSubweapons(i).IsGodMode = true;
                GodModeBtnTexts[index].color = Color.yellow;
            }
        }
    }

    public void OnClickCreate(int index)
    {
        GameManager.Inst().UpgManager.AddSW(index);
        GameManager.Inst().UpgManager.AfterWork(index);

        CharacterOnButtons(index);
    }

    public void OnClickDelete(int index)
    {
        GameManager.Inst().GetSubweapons(index).Disable();

        CharacterOffButtons(index);
    }

    public void OnClickNextWeaponBtn()
    {
        CurrentBulletType++;
        if (CurrentBulletType >= Constants.MAXBULLETS)
            CurrentBulletType = 0;

        ShowWeaponData();
    }

    public void OnClickPrevWeaponBtn()
    {
        CurrentBulletType--;
        if (CurrentBulletType <= 0)
            CurrentBulletType = Constants.MAXBULLETS - 1;

        ShowWeaponData();
    }

    public void OnClickPlusWeaponLevelBtn()
    {
        if (GameManager.Inst().UpgManager.BData[CurrentBulletType].GetPowerLevel() < GameManager.Inst().UpgManager.BData[CurrentBulletType].GetMaxBulletLevel())
            GameManager.Inst().UpgManager.LevelUp(CurrentBulletType);
        else
            GameManager.Inst().UpgManager.RarityUp(CurrentBulletType);

        ShowWeaponData();
    }

    public void OnClickMinusWeaponLevelBtn()
    {
        if (GameManager.Inst().UpgManager.BData[CurrentBulletType].GetPowerLevel() > 1)
            GameManager.Inst().UpgManager.LevelDown(CurrentBulletType);
        else
            GameManager.Inst().UpgManager.RarityDown(CurrentBulletType);

        ShowWeaponData();
    }

    public void OnClickPlusWeaponReinforceBtn(int index)
    {
        switch(index)
        {
            case 0:
                GameManager.Inst().UpgManager.BData[CurrentBulletType].SetAtk(GameManager.Inst().UpgManager.BData[CurrentBulletType].GetAtk() + 1);
                break;
            case 1:
                GameManager.Inst().UpgManager.BData[CurrentBulletType].SetSpd(GameManager.Inst().UpgManager.BData[CurrentBulletType].GetSpd() + 1);
                break;
            case 2:
                GameManager.Inst().UpgManager.BData[CurrentBulletType].SetHp(GameManager.Inst().UpgManager.BData[CurrentBulletType].GetHp() + 1);
                break;
        }

        ShowWeaponData();
    }

    public void OnClickMinusWeaponReinforceBtn(int index)
    {
        switch (index)
        {
            case 0:
                GameManager.Inst().UpgManager.BData[CurrentBulletType].SetAtk(GameManager.Inst().UpgManager.BData[CurrentBulletType].GetAtk() - 1);
                break;
            case 1:
                GameManager.Inst().UpgManager.BData[CurrentBulletType].SetSpd(GameManager.Inst().UpgManager.BData[CurrentBulletType].GetSpd() - 1);
                break;
            case 2:
                GameManager.Inst().UpgManager.BData[CurrentBulletType].SetHp(GameManager.Inst().UpgManager.BData[CurrentBulletType].GetHp() - 1);
                break;
        }

        ShowWeaponData();
    }
}
