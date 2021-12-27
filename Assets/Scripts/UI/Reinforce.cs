using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reinforce : MonoBehaviour
{
    public GameObject InfoWindow;
    //public Image Line;
    public Color[] LineColor;
    public Image Icon;
    public Text StatName;
    public Text StatCount;
    public Text LeftCount;
    public Button FeedBtn;
    public Button PlusBtn;
    public Button MinBtn;

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
                GameManager.Inst().UiManager.MainUI.Center.Weapon.AddQuantity();
            else
                GameManager.Inst().UiManager.MainUI.Center.Weapon.SubTQuantity();
        }
    }

    public void WindowOn(int type)
    {
        InfoWindow.SetActive(true);

        CurType = type;
        Player.EqData eq = GameManager.Inst().Player.GetReinforce(type);
        Icon.sprite = GameManager.Inst().UiManager.FoodImages[type];
        StatName.text = statNames[type];
        StatName.color = LineColor[type];
        StatCount.text = "+0";
        LeftCount.text = "0";

        if (eq != null)
        {
            GameManager.Inst().UiManager.MainUI.Center.Weapon.SetCurEquip(eq);

            PlusBtn.interactable = true;
            MinBtn.interactable = false;
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
            PlusBtn.interactable = false;
            MinBtn.interactable = false;
            FeedBtn.interactable = false;
        }
    }

    public void SetInfo(int value, int now)
    {
        StatCount.text = "+" + value.ToString();
        LeftCount.text = now.ToString();

        if (now >= 1)
            FeedBtn.interactable = true;
        else
            FeedBtn.interactable = false;

        if (now >= GameManager.Inst().Player.GetReinforce(CurType).Quantity)
            PlusBtn.interactable = false;
        else
            PlusBtn.interactable = true;

        if (now <= 0)
            MinBtn.interactable = false;
        else
            MinBtn.interactable = true;
    }

    public void OnClickInfoBackBtn()
    {
        InfoWindow.SetActive(false);

        GameManager.Inst().UiManager.MainUI.Center.Weapon.ResetData();

        //for (int i = 0; i < 3; i++)
        //{
        //    AddGauges[i].GetComponent<RectTransform>().sizeDelta = new Vector2(0, 30);

        //    switch (i)
        //    {
        //        case 0:
        //            Gauges[i].GetComponent<RectTransform>().sizeDelta = new Vector2((float)GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetAtk() / GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetMaxAtk() * 160, 30);
        //            GaugeTexts[i].text = GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetAtk().ToString();
        //            break;
        //        case 1:
        //            Gauges[i].GetComponent<RectTransform>().sizeDelta = new Vector2((float)GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetHp() / GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetMaxHp() * 160, 30);
        //            GaugeTexts[i].text = GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetHp().ToString();
        //            break;
        //        case 2:
        //            Gauges[i].GetComponent<RectTransform>().sizeDelta = new Vector2((float)GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetSpd() / GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetMaxSpd() * 160, 30);
        //            GaugeTexts[i].text = GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetSpd().ToString();
        //            break;
        //    }
        //}
    }

    public void OnClickFeedBtn()
    {
        Player.EqData eq = GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurEquip();
        //AddGauges[eq.Type].GetComponent<RectTransform>().sizeDelta = new Vector2(0, 30);
        GameManager.Inst().UiManager.MainUI.Center.Weapon.SetStatChange();

        StatCount.text = "+ 0";
        LeftCount.text = "0";

        //switch (eq.Type)
        //{
        //    case 0:
        //        GaugeTexts[eq.Type].text = GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetAtk().ToString();
        //        break;
        //    case 1:
        //        GaugeTexts[eq.Type].text = GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetHp().ToString();
        //        break;
        //    case 2:
        //        GaugeTexts[eq.Type].text = GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetSpd().ToString();
        //        break;
        //}
    }

    public void OnClickInfoBack()
    {
        InfoWindow.SetActive(false);
    }

    public void StartPress(bool isPlus)
    {
        IsPressing = true;
        IsPlus = isPlus;
    }

    public void EndPress()
    {
        IsPressing = false;
    }
}
