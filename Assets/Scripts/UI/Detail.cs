using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Detail : MonoBehaviour
{
    public GameObject Name;
    public GameObject CurrentLevel;
    public GameObject NextLevel;
    public GameObject Arrow;
    public GameObject Price;
    public Image CoinImg;
    public Sprite[] ResourceImgs;

    Sprite Coin;
    int BulletType;

    public void SetBulletType(int Type) { BulletType = Type; }

    void Awake()
    {
        BulletType = -1;
        Coin = CoinImg.sprite;
    }

    public void SetDetails()
    {
        Name.GetComponent<Text>().text = GameManager.Inst().TxtManager.GetBNames(BulletType);
        string lv = GameManager.Inst().TxtManager.GetBLevels(BulletType);
        CurrentLevel.GetComponent<Text>().text = lv;
        Price.GetComponent<Text>().text = GameManager.Inst().TxtManager.GetBPrices(BulletType);

        int level = GameManager.Inst().UpgManager.GetBData(BulletType).GetPowerLevel();
        if (level < GameManager.Inst().UpgManager.GetBData(BulletType).GetMaxBulletLevel() - 1)
            NextLevel.GetComponent<Text>().text = "Lv" + (level + 1).ToString();
        else if (level == GameManager.Inst().UpgManager.GetBData(BulletType).GetMaxBulletLevel() - 1)
            NextLevel.GetComponent<Text>().text = "Lv" + "MAX";
        else
        {
            int rarity = GameManager.Inst().UpgManager.GetBData(BulletType).GetRarity();
            if (rarity < 4)
            {
                Text nextText = NextLevel.GetComponent<Text>();
                nextText.text = "0";

                switch (rarity)
                {
                    case 0:
                        nextText.color = Color.green;
                        break;
                    case 1:
                        nextText.color = Color.blue;
                        break;
                    case 2:
                        nextText.color = new Color(1.0f, 0.0f, 1.0f);
                        break;
                    case 3:
                        nextText.color = Color.yellow;
                        break;
                }

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
