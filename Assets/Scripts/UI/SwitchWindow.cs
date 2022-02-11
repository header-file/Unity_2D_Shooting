using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.U2D.Animation;

public class SwitchWindow : MonoBehaviour
{
    public SpriteResolver Skin;
    public CanvasGroup InfoGroup;
    public SpriteRenderer Player;
    public Animator PlayerAnim;
    public GameObject Lock;
    public Text LockText;
    
    Color PlayerColor;
    int CurType;


    void Start()
    {
        PlayerColor = Color.white;

        Lock.SetActive(false);
    }

    public void SetAlpha(float alpha)
    {
        InfoGroup.alpha = alpha;
        PlayerColor.a = alpha;
        Player.color = PlayerColor;
    }

    public void SetButtons(int index, bool b, Sprite img, int grade)
    {
        
    }

    public void ShowEquipInfo(int Type)
    {
        CurType = Type;
        Skin.SetCategoryAndLabel("Skin", GameManager.Inst().Player.Types[Type]);
        PlayerAnim.SetInteger("Color", GameManager.Inst().ShtManager.BaseColor[Type] + 1);

        //for (int i = 0; i < 3; i++)
        //    PaintGauge(i, Type, 0);

        GameManager.Inst().UiManager.MainUI.Center.Weapon.SetWeaponUI(Type);
    }

    //public void PaintGauge(int type, int bulletType, int count)
    //{
    //    switch(type)
    //    {
    //        case 0:
    //            GaugeTexts[type].text = (GameManager.Inst().UpgManager.BData[bulletType].GetAtk() + count).ToString();
    //            Gauges[type].GetComponent<RectTransform>().sizeDelta = new Vector2((float)GameManager.Inst().UpgManager.BData[bulletType].GetAtk() / GameManager.Inst().UpgManager.BData[bulletType].GetMaxAtk() * 160, 30);
    //            break;
    //        case 1:
    //            GaugeTexts[type].text = (GameManager.Inst().UpgManager.BData[bulletType].GetHp() + count).ToString();
    //            Gauges[type].GetComponent<RectTransform>().sizeDelta = new Vector2((float)GameManager.Inst().UpgManager.BData[bulletType].GetHp() / GameManager.Inst().UpgManager.BData[bulletType].GetMaxHp() * 160, 30);
    //            break;
    //        case 2:
    //            GaugeTexts[type].text = (GameManager.Inst().UpgManager.BData[bulletType].GetSpd() + count).ToString();
    //            Gauges[type].GetComponent<RectTransform>().sizeDelta = new Vector2((float)GameManager.Inst().UpgManager.BData[bulletType].GetSpd() / GameManager.Inst().UpgManager.BData[bulletType].GetMaxSpd() * 160, 30);
    //            break;
    //    }
    //}

    //public void FlickeringGauge(int type, int bulletType, int count, Color newColor)
    //{
    //    switch (type)
    //    {
    //        case 0:
    //            if (GameManager.Inst().UpgManager.BData[bulletType].GetAtk() + count > GameManager.Inst().UpgManager.BData[bulletType].GetMaxAtk())
    //                GaugeTexts[type].text = GameManager.Inst().UpgManager.BData[bulletType].GetMaxAtk().ToString();
    //            else
    //                GaugeTexts[type].text = (GameManager.Inst().UpgManager.BData[bulletType].GetAtk() + count).ToString();
    //            AddGauges[type].GetComponent<RectTransform>().sizeDelta = new Vector2((float)count / GameManager.Inst().UpgManager.BData[bulletType].GetMaxAtk() * 160, 30);
    //            AddGauges[type].color = newColor;
    //            break;
    //        case 1:
    //            if (GameManager.Inst().UpgManager.BData[bulletType].GetHp() + count > GameManager.Inst().UpgManager.BData[bulletType].GetMaxHp())
    //                GaugeTexts[type].text = GameManager.Inst().UpgManager.BData[bulletType].GetMaxHp().ToString();
    //            else
    //                GaugeTexts[type].text = (GameManager.Inst().UpgManager.BData[bulletType].GetHp() + count).ToString();
    //            AddGauges[type].GetComponent<RectTransform>().sizeDelta = new Vector2((float)count / GameManager.Inst().UpgManager.BData[bulletType].GetMaxHp() * 160, 30);
    //            AddGauges[type].color = newColor;
    //            break;
    //        case 2:
    //            if (GameManager.Inst().UpgManager.BData[bulletType].GetSpd() + count > GameManager.Inst().UpgManager.BData[bulletType].GetMaxSpd())
    //                GaugeTexts[type].text = GameManager.Inst().UpgManager.BData[bulletType].GetMaxSpd().ToString();
    //            else
    //                GaugeTexts[type].text = (GameManager.Inst().UpgManager.BData[bulletType].GetSpd() + count).ToString();
    //            AddGauges[type].GetComponent<RectTransform>().sizeDelta = new Vector2((float)count / GameManager.Inst().UpgManager.BData[bulletType].GetMaxSpd() * 160, 30);
    //            AddGauges[type].color = newColor;
    //            break;
    //    }
    //}

    public void OnClickButton(int index)
    {
        GameManager.Inst().UiManager.MainUI.Center.Weapon.OnClickWeaponTypeSortBtn(index);
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