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
    public Sprite[] Icons;
    public Image Icon;
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
                GameManager.Inst().UiManager.MainUI.Center.Weapon.AddQuantity();
            else
                GameManager.Inst().UiManager.MainUI.Center.Weapon.SubTQuantity();
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
                if (GameManager.Inst().UpgManager.BData[bulletType].GetAtk() + count > GameManager.Inst().UpgManager.BData[bulletType].GetMaxAtk())
                    GaugeTexts[type].text = GameManager.Inst().UpgManager.BData[bulletType].GetMaxAtk().ToString();
                else
                    GaugeTexts[type].text = (GameManager.Inst().UpgManager.BData[bulletType].GetAtk() + count).ToString();
                AddGauges[type].GetComponent<RectTransform>().sizeDelta = new Vector2((float)count / GameManager.Inst().UpgManager.BData[bulletType].GetMaxAtk() * 160, 30);
                AddGauges[type].color = newColor;
                break;
            case 1:
                if (GameManager.Inst().UpgManager.BData[bulletType].GetHp() + count > GameManager.Inst().UpgManager.BData[bulletType].GetMaxHp())
                    GaugeTexts[type].text = GameManager.Inst().UpgManager.BData[bulletType].GetMaxHp().ToString();
                else
                    GaugeTexts[type].text = (GameManager.Inst().UpgManager.BData[bulletType].GetHp() + count).ToString();
                AddGauges[type].GetComponent<RectTransform>().sizeDelta = new Vector2((float)count / GameManager.Inst().UpgManager.BData[bulletType].GetMaxHp() * 160, 30);
                AddGauges[type].color = newColor;
                break;
            case 2:
                if (GameManager.Inst().UpgManager.BData[bulletType].GetSpd() + count > GameManager.Inst().UpgManager.BData[bulletType].GetMaxSpd())
                    GaugeTexts[type].text = GameManager.Inst().UpgManager.BData[bulletType].GetMaxSpd().ToString();
                else
                    GaugeTexts[type].text = (GameManager.Inst().UpgManager.BData[bulletType].GetSpd() + count).ToString();
                AddGauges[type].GetComponent<RectTransform>().sizeDelta = new Vector2((float)count / GameManager.Inst().UpgManager.BData[bulletType].GetMaxSpd() * 160, 30);
                AddGauges[type].color = newColor;
                break;
        }
    }

    public void WindowOn(Player.EqData eq, int now)
    {
        InfoWindow.SetActive(true);

        Line.color = LineColor[eq.Rarity];
        Icon.sprite = Icons[eq.Rarity * 3 + eq.Type];
        StatName.text = statNames[eq.Type];
        StatCount.text = "+" + eq.Value.ToString();
        LeftCount.text = now.ToString();
        FeedBtn.interactable = true;

        switch(eq.Type)
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

    public void SetInfo(int value, int now)
    {
        StatCount.text = "+" + value.ToString();
        LeftCount.text = now.ToString();
    }

    public void OnClickButton(int index)
    {
        GameManager.Inst().UiManager.OnClickWeaponTypeSortBtn(index);
    }

    public void OnClickInfoBackBtn()
    {
        InfoWindow.SetActive(false);

        GameManager.Inst().UiManager.MainUI.Center.Weapon.ResetData();

        for (int i = 0; i < 3; i++)
        {
            AddGauges[i].GetComponent<RectTransform>().sizeDelta = new Vector2(0, 30);

            switch (i)
            {
                case 0:
                    Gauges[i].GetComponent<RectTransform>().sizeDelta = new Vector2((float)GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetAtk() / GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetMaxAtk() * 160, 30);
                    GaugeTexts[i].text = GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetAtk().ToString();
                    break;
                case 1:
                    Gauges[i].GetComponent<RectTransform>().sizeDelta = new Vector2((float)GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetHp() / GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetMaxHp() * 160, 30);
                    GaugeTexts[i].text = GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetHp().ToString();
                    break;
                case 2:
                    Gauges[i].GetComponent<RectTransform>().sizeDelta = new Vector2((float)GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetSpd() / GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetMaxSpd() * 160, 30);
                    GaugeTexts[i].text = GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetSpd().ToString();
                    break;
            }
        }
    }

    public void OnClickFeedBtn()
    {
        Player.EqData eq = GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurEquip();
        AddGauges[eq.Type].GetComponent<RectTransform>().sizeDelta = new Vector2(0, 30);
        GameManager.Inst().UiManager.MainUI.Center.Weapon.SetStatChange();

        StatCount.text = "+ 0";
        LeftCount.text = "0";

        switch (eq.Type)
        {
            case 0:
                GaugeTexts[eq.Type].text = GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetAtk().ToString();
                break;
            case 1:
                GaugeTexts[eq.Type].text = GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetHp().ToString();
                break;
            case 2:
                GaugeTexts[eq.Type].text = GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Weapon.GetCurBulletType()].GetSpd().ToString();
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