﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour
{
    //t
    public Alarm Alarm;
    public Text CoinText;
    public Text JewelText;
    public Text[] Resources;
    public Buff Buff;
    public Animation ResourceAnim;

    //플레이어용 UI
    public GameObject PlayerUI;
    public Image PlayerTimerImg;
    public Text PlayerTimerText;
    public GameObject PlayerHPUI;
    public Image PlayerHPBar;
    public GameObject PlayerEquipUI;
    public Image PlayerEquipBar;
    public Image PlayerEquipIcon;

    //보스용 UI
    public GameObject BossHPBarCanvas;
    public Image BossHPBar;
    public Text BossHPBarText;
    public Image BossGaugeBar;
    public Text BossTimer;
    public Image BossTimerGaugeBar;
    public Text BossLevel;

    public BossGauge BossGauge;
    public SideMenu SideMenu;
    public GameObject Resource;
    public FloatingBubble Floating;
    public GameObject SpecialBtn;

    public Bottom Bottom;
    public Center Center;
    public ZzinBottom ZzinBottom;

    //Tutorial용
    public Tutorial Tutorial;

    //Popup
    public GameObject PopUpPref;
    public PopupReward PopupReward;
    public PopupStageopen PopupStageopen;
    public PopupBossclear PopupBossclear;

    void Awake()
    {
        for (int i = 0; i < 5; i++)
            Bottom.Arrows.transform.GetChild(i).gameObject.SetActive(false);

        //Player UI Setting
        GameManager.Inst().Player.UI = PlayerUI;
        GameManager.Inst().Player.TimerImage = PlayerTimerImg;
        GameManager.Inst().Player.TimerText = PlayerTimerText;
        GameManager.Inst().Player.HPUI = PlayerHPUI;
        GameManager.Inst().Player.HPBar = PlayerHPBar;
        GameManager.Inst().Player.EquipUI = PlayerEquipUI;
        GameManager.Inst().Player.EquipBar = PlayerEquipBar;
        GameManager.Inst().Player.EquipIcon = PlayerEquipIcon;

        PopupReward = Instantiate(PopUpPref).GetComponent<PopupReward>();
        PopupReward.transform.SetParent(transform);
        PopupReward.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        CoinText.text = GameManager.Inst().Player.GetCoin().ToString();
        JewelText.text = GameManager.Inst().Jewel.ToString();

        if (SceneManager.GetActiveScene().name == "Stage0")
            Floating.gameObject.SetActive(false);
    }

    void Start()
    {
        if (GameManager.Inst().IsFullPrice)
            SpecialBtn.SetActive(false);
        else
            SpecialBtn.SetActive(true);
    }

    public SideMenuSlot GetSideMenuSlot(int i)
    {
        if (SideMenu.Slots.Length == Constants.MAXSTAGES)
            return SideMenu.Slots[i];

        return null;
    }

    public void OnClickMenu()
    {
        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);

        Center.Menu.gameObject.SetActive(true);
    }

    public void OnClickResourceToggleBtn()
    {
        if (Resource.activeSelf)
            Resource.SetActive(false);
        else
        {
            Resource.SetActive(true);

            if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 3)
                GameManager.Inst().Tutorials.Step++;
        }
    }

    public void OnClickCheatBtn()
    {
        ZzinBottom.OnClickHomeBtn();

        Center.Cheat.gameObject.SetActive(true);
        Center.Cheat.ShowPage();

        GameManager.Inst().UiManager.MainUI.ZzinBottom.HomeBtn.SetActive(true);

        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);
    }

    public void OnClickCheatBackBtn()
    {
        Center.Cheat.gameObject.SetActive(false);

        ZzinBottom.HomeBtn.SetActive(false);

        GameManager.Inst().IptManager.SetIsAbleControl(true);
        GameManager.Inst().IptManager.SetIsAbleSWControl(true);
    }

    public void OnClickSpecialBtn()
    {
        GameManager.Inst().IptManager.SetIsAbleControl(true);
        GameManager.Inst().IptManager.SetIsAbleSWControl(true);

        ZzinBottom.OnClickShopBtn();
        //Center.Shop.Toggles[3].isOn = true;
    }

    public void OnClickBuffBtn(int index)
    {
        Buff.BuffType = index;
        Buff.ConfirmWindow.SetActive(true);

        if (Buff.AdCount > 0)
        {
            Buff.AdWindow.SetActive(true);
            Buff.JewelWindow.SetActive(false);
        }
        else
        {
            Buff.AdWindow.SetActive(false);
            Buff.JewelWindow.SetActive(true);

            if (GameManager.Inst().Jewel >= 3)
            {
                Buff.AmountText.color = Color.white;
                Buff.YesBtn.interactable = true;
            }
            else
            {
                Buff.AmountText.color = Color.red;
                Buff.YesBtn.interactable = false;
            }
        }
    }
}
