using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchWindow : MonoBehaviour
{
    public GameObject[] Buttons;
    public Image[] Gauges;
    public Image[] AddGauges;
    public Text[] GaugeTexts;

    public GameObject InfoWindow;
    public Image Line;
    public Color[] LineColor;
    public Text StatName;
    public Text StatCount;
    public Text LeftCount;
    public Button FeedBtn;

    string[] statNames;
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
        if(IsPressing)
        {
            if (IsPlus)
                GameManager.Inst().UiManager.WeaponUI.AddQuantity();
            else
                GameManager.Inst().UiManager.WeaponUI.SubTQuantity();
        }
    }

    public void SetButtons(int index, bool b, Sprite img, int grade)
    {
        
    }

    public void PaintGauge(int type, int bulletType, int count)
    {
        switch(type)
        {
            case 0:
                GaugeTexts[type].text = (GameManager.Inst().UpgManager.BData[bulletType].GetAtk() + count).ToString();
                Gauges[type].GetComponent<RectTransform>().sizeDelta = new Vector2((float)GameManager.Inst().UpgManager.BData[bulletType].GetAtk() / GameManager.Inst().UpgManager.BData[bulletType].GetMaxAtk() * 160, 30);
                break;
            case 1:
                GaugeTexts[type].text = (GameManager.Inst().UpgManager.BData[bulletType].GetHp() + count).ToString();
                Gauges[type].GetComponent<RectTransform>().sizeDelta = new Vector2((float)GameManager.Inst().UpgManager.BData[bulletType].GetHp() / GameManager.Inst().UpgManager.BData[bulletType].GetMaxHp() * 160, 30);
                break;
            case 2:
                GaugeTexts[type].text = (GameManager.Inst().UpgManager.BData[bulletType].GetSpd() + count).ToString();
                Gauges[type].GetComponent<RectTransform>().sizeDelta = new Vector2((float)GameManager.Inst().UpgManager.BData[bulletType].GetSpd() / GameManager.Inst().UpgManager.BData[bulletType].GetMaxSpd() * 160, 30);
                break;
        }
    }

    public void FlickeringGauge(int type, int bulletType, int count, Color newColor)
    {
        switch (type)
        {
            case 0:
                GaugeTexts[type].text = (GameManager.Inst().UpgManager.BData[bulletType].GetAtk() + count).ToString();
                AddGauges[type].GetComponent<RectTransform>().sizeDelta = new Vector2((float)count / GameManager.Inst().UpgManager.BData[bulletType].GetMaxAtk() * 160, 30);
                AddGauges[type].color = newColor;
                break;
            case 1:
                GaugeTexts[type].text = (GameManager.Inst().UpgManager.BData[bulletType].GetHp() + count).ToString();
                AddGauges[type].GetComponent<RectTransform>().sizeDelta = new Vector2((float)count / GameManager.Inst().UpgManager.BData[bulletType].GetMaxHp() * 160, 30);
                AddGauges[type].color = newColor;
                break;
            case 2:
                GaugeTexts[type].text = (GameManager.Inst().UpgManager.BData[bulletType].GetSpd() + count).ToString();
                AddGauges[type].GetComponent<RectTransform>().sizeDelta = new Vector2((float)count / GameManager.Inst().UpgManager.BData[bulletType].GetMaxSpd() * 160, 30);
                AddGauges[type].color = newColor;
                break;
        }
    }

    public void WindowOn(int rarity, int type, int value, int left)
    {
        InfoWindow.SetActive(true);

        Line.color = LineColor[rarity];
        StatName.text = statNames[type];
        StatCount.text = "+" + value.ToString();
        LeftCount.text = left.ToString();
        FeedBtn.interactable = true;

        switch(type)
        {
            case 0:
                if (GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.WeaponUI.GetCurBulletType()].GetAtk() >= GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.WeaponUI.GetCurBulletType()].GetMaxAtk())
                    FeedBtn.interactable = false;
                break;
            case 1:
                if (GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.WeaponUI.GetCurBulletType()].GetHp() >= GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.WeaponUI.GetCurBulletType()].GetMaxHp())
                    FeedBtn.interactable = false;
                break;
            case 2:
                if (GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.WeaponUI.GetCurBulletType()].GetSpd() >= GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.WeaponUI.GetCurBulletType()].GetMaxSpd())
                    FeedBtn.interactable = false;
                break;
        }
        
    }

    public void SetInfo(int value, int left)
    {
        StatCount.text = "+" + value.ToString();
        LeftCount.text = left.ToString();
    }

    public void OnClickButton(int index)
    {
        GameManager.Inst().UiManager.OnClickEquipSlotBtn(index);
    }

    public void OnClickInfoBackBtn()
    {
        InfoWindow.SetActive(false);

        GameManager.Inst().UiManager.WeaponUI.ResetData();

        for (int i = 0; i < 3; i++)
        {
            AddGauges[i].GetComponent<RectTransform>().sizeDelta = new Vector2(0, 30);

            switch (i)
            {
                case 0:
                    Gauges[i].GetComponent<RectTransform>().sizeDelta = new Vector2((float)GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.WeaponUI.GetCurBulletType()].GetAtk() / GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.WeaponUI.GetCurBulletType()].GetMaxAtk() * 160, 30);
                    GaugeTexts[i].text = GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.WeaponUI.GetCurBulletType()].GetAtk().ToString();
                    break;
                case 1:
                    Gauges[i].GetComponent<RectTransform>().sizeDelta = new Vector2((float)GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.WeaponUI.GetCurBulletType()].GetHp() / GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.WeaponUI.GetCurBulletType()].GetMaxHp() * 160, 30);
                    GaugeTexts[i].text = GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.WeaponUI.GetCurBulletType()].GetHp().ToString();
                    break;
                case 2:
                    Gauges[i].GetComponent<RectTransform>().sizeDelta = new Vector2((float)GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.WeaponUI.GetCurBulletType()].GetSpd() / GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.WeaponUI.GetCurBulletType()].GetMaxSpd() * 160, 30);
                    GaugeTexts[i].text = GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.WeaponUI.GetCurBulletType()].GetSpd().ToString();
                    break;
            }
        }
    }

    public void OnClickFeedBtn()
    {
        Player.EqData eq = GameManager.Inst().UiManager.WeaponUI.GetCurEquip();
        AddGauges[eq.Type].GetComponent<RectTransform>().sizeDelta = new Vector2(0, 30);
        GameManager.Inst().UiManager.WeaponUI.SetStatChange();

        StatCount.text = "+ 0";
        LeftCount.text = GameManager.Inst().UiManager.WeaponUI.GetCurEquip().Quantity.ToString();

        switch (eq.Type)
        {
            case 0:
                GaugeTexts[eq.Type].text = GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.WeaponUI.GetCurBulletType()].GetAtk().ToString();
                break;
            case 1:
                GaugeTexts[eq.Type].text = GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.WeaponUI.GetCurBulletType()].GetHp().ToString();
                break;
            case 2:
                GaugeTexts[eq.Type].text = GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.WeaponUI.GetCurBulletType()].GetSpd().ToString();
                break;
        }
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


//for (int i = 1; i <= 10; i++)
//{
//    if (value >= i * 10)
//        Gauges[type].transform.GetChild(i - 1).gameObject.GetComponent<Image>().color = color;
//    else
//        Gauges[type].transform.GetChild(i - 1).gameObject.GetComponent<Image>().color = Color.white;
//}

//public void FlickerGauge(int before, int after, int type, Color gaugeColor, float alpha)
//{
//    for (int i = 1; i <= 10; i++)
//    {
//        if (before >= i && after >= i)
//            Gauges[type].transform.GetChild(i - 1).gameObject.GetComponent<Image>().color = gaugeColor;
//        else if (before < i && after < i)
//            Gauges[type].transform.GetChild(i - 1).gameObject.GetComponent<Image>().color = Color.white;
//        else if (before < i || after < i)
//        {
//            Color color = gaugeColor;
//            color.a = alpha;
//            Gauges[type].transform.GetChild(i - 1).gameObject.GetComponent<Image>().color = color;
//        }
//    }
//}