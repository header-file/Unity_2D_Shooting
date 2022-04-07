using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoArea : MonoBehaviour
{
    public Text Level;
    public Text CoinText;
    public GameObject Coin;
    public GameObject Resource;
    public GameObject[] Resources;
    public Text[] ResourceTexts;
    public Text WeaponName;
    public Text[] GaugeTexts;
    public Image[] Gauges;
    public CanvasGroup[] CanvasGps;
    public Animator[] Anim;
    public Button UpgradeBtn;
    public GameObject MaxLevel;
    public Button EquipAreaBtn;
    public Image[] GradeDeco;
    public Image GradeBg;
    public Animation GradeUpAnim;

    public int[] DefaultColor;
    public Sprite[] Decos;
    public Color[] GradeColors;

    public void SetWeaponName(int index) { WeaponName.text = GameManager.Inst().TxtManager.BulletTypeNames[index]; }

    public void SetAlpha(float alpha)
    {
        for (int i = 0; i < CanvasGps.Length; i++)
            CanvasGps[i].alpha = alpha;
    }

    public void SetAnimTrigger(string trigger)
    {
        for (int i = 0; i < 3; i++)
            Anim[i].SetTrigger(trigger);
    }

    void Start()
    {
        Anim[0].SetInteger("Color", DefaultColor[6]);
        Anim[1].SetInteger("Color", DefaultColor[0]);
        Anim[2].SetInteger("Color", DefaultColor[1]);
    }

    public void ShowDetail(int bulletType)
    {
        Level.text = "Level " + GameManager.Inst().UpgManager.BData[bulletType].GetPowerLevel().ToString();

        for (int i = 0; i < 2; i++)
        {
            GradeDeco[i].sprite = Decos[GameManager.Inst().UpgManager.BData[bulletType].GetRarity()];
            GradeDeco[i].SetNativeSize();
        }
        GradeBg.color = GradeColors[GameManager.Inst().UpgManager.BData[bulletType].GetRarity()];

        if(GameManager.Inst().UpgManager.BData[bulletType].GetPowerLevel() < (GameManager.Inst().UpgManager.BData[bulletType].GetRarity() + 1) * 10)
        {
            Coin.SetActive(true);
            Resource.SetActive(false);
            CoinText.text = GameManager.Inst().UpgManager.BData[bulletType].GetPrice().ToString();

            if (GameManager.Inst().Player.GetCoin() < GameManager.Inst().UpgManager.BData[bulletType].GetPrice())
                CoinText.color = Color.red;
            else
                CoinText.color = Color.white;
        }
        else
        {
            Coin.SetActive(false);
            Resource.SetActive(true);

            for(int i = 0; i < Constants.MAXRESOURCETYPES; i++)
            {
                ResourceTexts[i].text = GameManager.Inst().UpgManager.GetResourceData(GameManager.Inst().UpgManager.BData[bulletType].GetRarity(), i).ToString();

                if (GameManager.Inst().Resources[i] < GameManager.Inst().UpgManager.GetResourceData(GameManager.Inst().UpgManager.BData[bulletType].GetRarity(), i))
                    ResourceTexts[i].color = Color.red;
                else
                    ResourceTexts[i].color = Color.white;

                if (ResourceTexts[i].text == "0")
                    Resources[i].SetActive(false);
                else
                    Resources[i].SetActive(true);
            } 
        }

        for (int i = 0; i < Constants.MAXREINFORCETYPE; i++)
            PaintGauge(i, bulletType);
    }

    public void PaintGauge(int type, int bulletType)
    {
        switch (type)
        {
            case 0:
                GaugeTexts[type].text = GameManager.Inst().UpgManager.BData[bulletType].GetAtk().ToString();
                Gauges[type].fillAmount = (float)GameManager.Inst().UpgManager.BData[bulletType].GetAtk() / GameManager.Inst().UpgManager.BData[bulletType].GetMaxAtk();
                break;
            case 1:
                GaugeTexts[type].text = GameManager.Inst().UpgManager.BData[bulletType].GetHp().ToString();
                Gauges[type].fillAmount = (float)GameManager.Inst().UpgManager.BData[bulletType].GetHp() / GameManager.Inst().UpgManager.BData[bulletType].GetMaxHp();
                break;
            case 2:
                GaugeTexts[type].text = GameManager.Inst().UpgManager.BData[bulletType].GetSpd().ToString();
                Gauges[type].fillAmount = (float)GameManager.Inst().UpgManager.BData[bulletType].GetSpd() / GameManager.Inst().UpgManager.BData[bulletType].GetMaxSpd();
                break;
        }
    }

    public void GradeUp(int bulletType)
    {
        GradeBg.color = GradeColors[GameManager.Inst().UpgManager.BData[bulletType].GetRarity()];
        GradeUpAnim.Play();
    }

    public void ShowMaxLevel(bool b)
    {
        MaxLevel.SetActive(b);
        UpgradeBtn.gameObject.SetActive(!b);
    }
}


//public void SetSlots(int index, bool b, Sprite img, int grade)
//{
//    if (b)
//    {
//        SlotImgs[index].sprite = img;
//        SlotImgs[index + 3].gameObject.SetActive(false);

//        for(int i = 0; i < 5; i++)
//            Grades[index].transform.GetChild(i).gameObject.SetActive(false);

//        Grades[index].transform.GetChild(grade).gameObject.SetActive(true);
//    }
//    else
//    {
//        SlotImgs[index].sprite = img;
//        SlotImgs[index + 3].gameObject.SetActive(true);

//        for (int i = 0; i < 5; i++)
//            Grades[index].transform.GetChild(i).gameObject.SetActive(false);
//    }
//}