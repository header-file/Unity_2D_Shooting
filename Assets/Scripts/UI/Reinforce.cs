using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Reinforce : MonoBehaviour
{
    public GameObject InfoWindow;
    public Text[] StatName;
    public Text[] StatCount;
    public Text[] MaxCount;
    public Text[] LeftCount;
    public Button FeedBtn;
    public Button[] PlusBtn;
    public Button[] MinBtn;

    string[] statNames;
    int CurType;
    bool IsPressing;
    bool IsPlus;

    void Start()
    {
        statNames = new string[3];
        statNames[0] = "ATK";
        statNames[1] = "H  P";
        statNames[2] = "SPD";

        InfoWindow.SetActive(false);
        IsPressing = false;
    }

    void Update()
    {
        if (IsPressing)
        {
            if (IsPlus)
                GameManager.Inst().UiManager.MainUI.Center.Weapon.AddQuantity(CurType);
            else
                GameManager.Inst().UiManager.MainUI.Center.Weapon.SubTQuantity();
        }
    }

    public void WindowOn()
    {
        InfoWindow.SetActive(true);
        FeedBtn.interactable = false;
        GameManager.Inst().UiManager.MainUI.Center.Weapon.SwitchWindow.SetActive(false);

        for (int i = 0; i < Constants.MAXREINFORCETYPE; i++)
        {
            Player.EqData eq = GameManager.Inst().Player.GetReinforce(i);

            StatName[i].text = statNames[i];
            StatCount[i].text = "+0";
            LeftCount[i].text = "0";
            MaxCount[i].text = eq.Quantity.ToString();


            if (eq != null)
            {
                GameManager.Inst().UiManager.MainUI.Center.Weapon.SetCurEquip(eq);

                PlusBtn[i].interactable = true;
                MinBtn[i].interactable = false;
                FeedBtn.interactable = true;

                switch (eq.Type)
                {
                    case 0:
                        if (GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetAtk() >= GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetMaxAtk())
                            FeedBtn.interactable = false;
                        break;
                    case 1:
                        if (GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetHp() >= GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetMaxHp())
                            FeedBtn.interactable = false;
                        break;
                    case 2:
                        if (GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetSpd() >= GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetMaxSpd())
                            FeedBtn.interactable = false;
                        break;
                }
            }
            else
            {
                PlusBtn[i].interactable = false;
                MinBtn[i].interactable = false;
            }
        }

        if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 34)
            GameManager.Inst().Tutorials.Step++;
    }

    public void SetInfo(int value, int now)
    {
        StatCount[CurType].text = "+" + value.ToString();
        LeftCount[CurType].text = now.ToString();

        if (now >= 1)
            FeedBtn.interactable = true;
        else
            FeedBtn.interactable = false;

        if (now >= GameManager.Inst().Player.GetReinforce(CurType).Quantity)
            PlusBtn[CurType].interactable = false;
        else
            PlusBtn[CurType].interactable = true;

        if (now <= 0)
            MinBtn[CurType].interactable = false;
        else
            MinBtn[CurType].interactable = true;
    }

    public void OnClickInfoBackBtn()
    {
        InfoWindow.SetActive(false);

        GameManager.Inst().UiManager.MainUI.Center.Weapon.ResetData();
    }

    public void OnClickFeedBtn()
    {
        Player.EqData eq = GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurEquip();
        GameManager.Inst().UiManager.MainUI.Center.Weapon.SetStatChange();

        for (int i = 0; i < Constants.MAXREINFORCETYPE; i++)
        {
            StatCount[i].text = "+ 0";
            LeftCount[i].text = "0";
            MaxCount[i].text = GameManager.Inst().Player.GetReinforce(i).Quantity.ToString();
        }

        if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 36)
        {
            OnClickInfoBack();
            GameManager.Inst().Tutorials.Step++;
        }
    }

    public void OnClickInfoBack()
    {
        GameManager.Inst().UiManager.MainUI.Center.Weapon.SwitchWindow.SetActive(true);
        InfoWindow.SetActive(false);
    }

    public void StartPress(int index)
    {
        IsPressing = true;
        IsPlus = index % 2 == 0 ? false : true;
        CurType = index / 2;
    }

    public void EndPress()
    {
        IsPressing = false;
    }
}
