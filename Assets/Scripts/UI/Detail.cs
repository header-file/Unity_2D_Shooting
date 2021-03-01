using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Detail : MonoBehaviour
{
    public Image WpImg;
    public GameObject Name;
    public GameObject CurrentLevel;
    public GameObject NextLevel;
    public GameObject Arrow;
    public GameObject Price;
    public Image CoinImg;
    public Sprite[] ResourceImgs;

    Sprite Coin;
    int BulletType;
    Color[] Colors;

    public void SetBulletType(int Type) { BulletType = Type; }

    void Awake()
    {
        BulletType = -1;
        Coin = CoinImg.sprite;

        Colors = new Color[5];
        Colors[0] = Color.white;
        Colors[1] = Color.green;
        Colors[2] = Color.blue;
        Colors[3] = new Color(1.0f, 0.0f, 1.0f);
        Colors[4] = Color.yellow;
    }

    public void SetDetails()
    {
        WpImg.sprite = GameManager.Inst().UiManager.WeaponImages[BulletType];
        Text nameText = Name.GetComponent<Text>();
        nameText.text = GameManager.Inst().TxtManager.GetBNames(BulletType) + " + " + GameManager.Inst().UpgManager.BData[BulletType].GetPowerLevel();
        string lv = GameManager.Inst().TxtManager.GetBLevels(BulletType);
        Text curLevel = CurrentLevel.GetComponent<Text>();
        curLevel.text = lv;
        int rarity = GameManager.Inst().UpgManager.BData[BulletType].GetRarity();
        curLevel.color = Colors[rarity];
        nameText.color = Colors[rarity];

        Price.GetComponent<Text>().text = GameManager.Inst().TxtManager.GetBPrices(BulletType);
        CoinImg.sprite = Coin;

        int level = GameManager.Inst().UpgManager.BData[BulletType].GetPowerLevel();
        if (level < GameManager.Inst().UpgManager.BData[BulletType].GetMaxBulletLevel() - 1)
            NextLevel.GetComponent<Text>().text = "Lv" + (level + 1).ToString();
        else if (level == GameManager.Inst().UpgManager.BData[BulletType].GetMaxBulletLevel() - 1)
            NextLevel.GetComponent<Text>().text = "Lv" + "MAX";
        else
        {
            if (rarity < 4)
            {
                Text nextText = NextLevel.GetComponent<Text>();
                nextText.text = "0";
                nextText.color = Colors[rarity + 1];                

                CoinImg.sprite = ResourceImgs[rarity];
                Price.GetComponent<Text>().text = "10";
            }
            else
            {
                NextLevel.SetActive(false);
                Arrow.SetActive(false);
            }
        }
            
    }

    public void OnClickUpgradeBtn()
    {
        GameManager.Inst().UpgManager.AddLevel(BulletType);
    }
}
