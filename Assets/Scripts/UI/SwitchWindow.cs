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
    //public Image CurrentBuletImg;
    //public GameObject[] Grades;

    //public void SetCurrentBulletImg(Sprite sprite) { CurrentBuletImg.sprite = sprite; }

    public void SetButtons(int index, bool b, Sprite img, int grade)
    {
        //if (b)
        //{
        //    Buttons[index].transform.GetChild(2).gameObject.SetActive(false);
        //    Buttons[index].transform.GetChild(1).GetComponent<Image>().sprite = img;

        //    for (int i = 0; i < 5; i++)
        //        Grades[index].transform.GetChild(i).gameObject.SetActive(false);

        //    Grades[index].transform.GetChild(grade).gameObject.SetActive(true);
        //}
        //else
        //{
        //    Buttons[index].transform.GetChild(2).gameObject.SetActive(true);
        //    Buttons[index].transform.GetChild(1).GetComponent<Image>().sprite = img;

        //    for (int i = 0; i < 5; i++)
        //        Grades[index].transform.GetChild(i).gameObject.SetActive(false);
        //}
    }

    public void PaintGauge(int type, int bulletType)
    {
        switch(type)
        {
            case 0:
                GaugeTexts[type].text = GameManager.Inst().UpgManager.BData[bulletType].GetAtk().ToString();
                //Gauges[type].fillAmount = 
                Gauges[type].GetComponent<RectTransform>().sizeDelta = new Vector2((float)GameManager.Inst().UpgManager.BData[bulletType].GetAtk() / GameManager.Inst().UpgManager.BData[bulletType].GetMaxAtk() * 160, 30);
                break;
            case 1:
                GaugeTexts[type].text = GameManager.Inst().UpgManager.BData[bulletType].GetHp().ToString();
                //Gauges[type].fillAmount = (float)GameManager.Inst().UpgManager.BData[bulletType].GetHp() / GameManager.Inst().UpgManager.BData[bulletType].GetMaxHp();
                Gauges[type].GetComponent<RectTransform>().sizeDelta = new Vector2((float)GameManager.Inst().UpgManager.BData[bulletType].GetHp() / GameManager.Inst().UpgManager.BData[bulletType].GetMaxHp() * 160, 30);
                break;
            case 2:
                GaugeTexts[type].text = GameManager.Inst().UpgManager.BData[bulletType].GetSpd().ToString();
                //Gauges[type].fillAmount = (float)GameManager.Inst().UpgManager.BData[bulletType].GetSpd() / GameManager.Inst().UpgManager.BData[bulletType].GetMaxSpd();
                Gauges[type].GetComponent<RectTransform>().sizeDelta = new Vector2((float)GameManager.Inst().UpgManager.BData[bulletType].GetSpd() / GameManager.Inst().UpgManager.BData[bulletType].GetMaxSpd() * 160, 30);
                break;
        }
    }

    public void FlickeringGauge(int type, int bulletType, int count, Color newColor)
    {
        switch (type)
        {
            case 0:
                GaugeTexts[type].text = GameManager.Inst().UpgManager.BData[bulletType].GetAtk().ToString();
                //Gauges[type].fillAmount = 
                AddGauges[type].GetComponent<RectTransform>().sizeDelta = new Vector2((float)count / GameManager.Inst().UpgManager.BData[bulletType].GetMaxAtk() * 160, 30);
                AddGauges[type].color = newColor;
                break;
            case 1:
                GaugeTexts[type].text = GameManager.Inst().UpgManager.BData[bulletType].GetHp().ToString();
                //Gauges[type].fillAmount = (float)GameManager.Inst().UpgManager.BData[bulletType].GetHp() / GameManager.Inst().UpgManager.BData[bulletType].GetMaxHp();
                AddGauges[type].GetComponent<RectTransform>().sizeDelta = new Vector2((float)count / GameManager.Inst().UpgManager.BData[bulletType].GetMaxHp() * 160, 30);
                AddGauges[type].color = newColor;
                break;
            case 2:
                GaugeTexts[type].text = GameManager.Inst().UpgManager.BData[bulletType].GetSpd().ToString();
                //Gauges[type].fillAmount = (float)GameManager.Inst().UpgManager.BData[bulletType].GetSpd() / GameManager.Inst().UpgManager.BData[bulletType].GetMaxSpd();
                AddGauges[type].GetComponent<RectTransform>().sizeDelta = new Vector2((float)count / GameManager.Inst().UpgManager.BData[bulletType].GetMaxSpd() * 160, 30);
                AddGauges[type].color = newColor;
                break;
        }
    }

    public void OnClickButton(int index)
    {
        GameManager.Inst().UiManager.OnClickEquipSlotBtn(index);
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