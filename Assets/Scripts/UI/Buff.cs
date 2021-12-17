using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buff : MonoBehaviour
{
    public GameObject ConfirmWindow;
    public GameObject AdWindow;
    public Text LeftText;
    public GameObject JewelWindow;
    public Text AmountText;
    public BuffSlot[] BuffSlots;
    public Button YesBtn;
    public int BuffType;
    public int AdCount;


    void Awake()
    {
        AdCount = 3;
        ConfirmWindow.SetActive(false);
    }

    public void SubtractAdCount(int num)
    {
        AdCount -= num;
        LeftText.text = AdCount.ToString() + " / 3";
    }

    public void ResetAdCount()
    {
        AdCount = 3;
        LeftText.text = AdCount.ToString() + " / 3";
    }

    public void OnClickAdYesBtn()
    {
        GameManager.Inst().BufManager.CurrentBuffType = BuffType;
        GameManager.Inst().AdsManager.AdvType = AdvertiseManager.AdType.SHOP_BUFF;
        GameManager.Inst().AdsManager.PlayAd();

        ConfirmWindow.SetActive(false);
    }

    public void OnClickJewelYesBtn()
    {
        GameManager.Inst().AddJewel(-3);
        GameManager.Inst().BufManager.StartBuff(BuffType);

        ConfirmWindow.SetActive(false);
    }

    public void OnClickNoBtn()
    {
        ConfirmWindow.SetActive(false);
    }
}
