using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.U2D.Animation;

public class Reinforce : MonoBehaviour
{
    public CanvasGroup InfoGroup;
    public Text[] CurCount;
    public Text[] LeftCount;
    public Image[] GaugeBars;
    public Image[] AddBars;
    public Text[] CurValues;
    public Text[] AddValues;
    public Button FeedBtn;
    public Button[] PlusBtn;
    public Button[] MinBtn;
    public SpriteResolver Skin;
    public SpriteRenderer Player;
    public Animator PlayerAnim;
    public GameObject Lock;
    public Text LockText;
    
    int CurType;
    bool IsPressing = false;
    bool IsPlus;
    Color PlayerColor = Color.white;

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

    public void SetAlpha(float alpha)
    {
        InfoGroup.alpha = alpha;
        PlayerColor.a = alpha;
        Player.color = PlayerColor;
    }

    public void WindowOn()
    {
        GameManager.Inst().UiManager.MainUI.Center.Weapon.InfoArea.gameObject.SetActive(false);
        GameManager.Inst().UiManager.MainUI.Center.Weapon.EquipArea.gameObject.SetActive(false);
        gameObject.SetActive(true);

        Show();

        if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 34)
            GameManager.Inst().Tutorials.Step++;
    }

    public void Show()
    {
        FeedBtn.interactable = false;

        Skin.SetCategoryAndLabel("Skin", GameManager.Inst().Player.Types[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()]);
        PlayerAnim.SetInteger("Color", GameManager.Inst().ShtManager.BaseColor[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()] + 1);

        for (int i = 0; i < Constants.MAXREINFORCETYPE; i++)
        {
            Player.EqData eq = GameManager.Inst().Player.GetReinforce(i);

            LeftCount[i].text = "0";
            CurCount[i].text = "0";

            GaugeBars[i].fillAmount = (float)GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetAtk() / GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetMaxAtk();
            AddBars[i].fillAmount = 0.0f;
            CurValues[i].text = GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetAtk().ToString();
            AddValues[i].text = "+0";

            if (eq != null)
            {
                LeftCount[i].text = eq.Quantity.ToString();

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
    }

    public void SetInfo(int value, int max)
    {
        CurCount[CurType].text = value.ToString();
        AddValues[CurType].text = "+" + value.ToString();
        AddBars[CurType].fillAmount = (float)value / max + GaugeBars[CurType].fillAmount;

        if (value >= 1)
            FeedBtn.interactable = true;
        else
            FeedBtn.interactable = false;

        if (value >= GameManager.Inst().Player.GetReinforce(CurType).Quantity)
            PlusBtn[CurType].interactable = false;
        else
            PlusBtn[CurType].interactable = true;

        if (value <= 0)
            MinBtn[CurType].interactable = false;
        else
            MinBtn[CurType].interactable = true;
    }

    public void OnClickInfoBackBtn()
    {
        GameManager.Inst().UiManager.MainUI.Center.Weapon.ResetData();
    }

    public void OnClickFeedBtn()
    {
        Player.EqData eq = GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurEquip();
        GameManager.Inst().UiManager.MainUI.Center.Weapon.SetStatChange();

        for (int i = 0; i < Constants.MAXREINFORCETYPE; i++)
        {
            LeftCount[i].text = "0";
            CurCount[i].text = "0";

            if(GameManager.Inst().Player.GetReinforce(i) != null)
                LeftCount[i].text = GameManager.Inst().Player.GetReinforce(i).Quantity.ToString();
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
        gameObject.SetActive(false);
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
