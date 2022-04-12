using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public ShopSlot[] Jewels;
    public ShopSlot[] Coins;
    public Sprite[] JewelIcons;
    public Sprite[] CoinIcons;
    public Text AdLeftText;
    public Text TimerText;
    public Button AdButton;
    public GameObject FullPriceLock;
    public CanvasGroup FailMsg;

    //Confirm
    public GameObject Confirm;
    public Image Icon;
    public Text NameText;
    public GameObject Money;
    public GameObject Jewel;
    public Text PriceText;
    public Text DetailText;

    //Result
    public GameObject Result;
    public Image ResultIcon;
    public Text ResultNameText;

    List<Dictionary<string, object>> Data;
    string[,] JewelDatas;
    string[,] CoinDatas;
    int CurrentPage;
    int CurrentItem;
    bool IsFail;
    float MsgSpeed;

    void Awake()
    {
        SetJewelDatas();
        SetCoinDatas();

        FailMsg.alpha = 0.0f;
        IsFail = false;
        MsgSpeed = 2.0f;
    }

    void Start()
    {
        CurrentPage = 0;
        CurrentItem = -1;

        ShowPage();

        Confirm.SetActive(false);
        FullPriceLock.SetActive(false);
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (GameManager.Inst().AdsManager.AdLeft <= 0)
            SetTimer();

        if (IsFail)
            FailMessage();
    }

    public void ShowPage()
    {
        if (JewelDatas == null)
            return;

        for (int i = 0; i < Jewels.Length; i++)
        {
            Jewels[i].NameText.text = JewelDatas[i, 1];
            Jewels[i].PriceText.text = JewelDatas[i, 2];
            Jewels[i].Icon.sprite = JewelIcons[i];

            if (i > 4)
            {
                if (GameManager.Inst().DatManager.GameData.DailyLeft > 0)
                    Jewels[5].Button.interactable = false;
                else
                    Jewels[5].Button.interactable = true;

                if (GameManager.Inst().DatManager.GameData.DailyPlusLeft > 0)
                    Jewels[6].Button.interactable = false;
                else
                    Jewels[6].Button.interactable = true;
            }
        }

        for (int i = 0; i < Coins.Length; i++)
        {
            Coins[i].NameText.text = CoinDatas[i, 0];
            Coins[i].PriceText.text = CoinDatas[i, 2];
            Coins[i].Icon.sprite = CoinIcons[int.Parse(CoinDatas[i, 3])];
            Coins[i].Jewel.SetActive(true);
        }

        if (GameManager.Inst().IsFullPrice)
            FullPriceLock.SetActive(true);
    }

    void SetJewelDatas()
    {
        Data = CSVReader.Read("Datas/ShopJewelData");
        JewelDatas = new string[Constants.MAX_SHOP_JEWEL, Constants.SHOP_JEWEL_TYPES];

        for (int i = 0; i < Constants.MAX_SHOP_JEWEL; i++)
        {
            JewelDatas[i, 0] = Data[i]["Name"].ToString();
            JewelDatas[i, 1] = Data[i]["Amount"].ToString();
            JewelDatas[i, 2] = Data[i]["Price"].ToString();
            JewelDatas[i, 3] = Data[i]["Detail"].ToString();
        }
    }

    void SetCoinDatas()
    {
        Data = CSVReader.Read("Datas/ShopResourceData");
        CoinDatas = new string[Constants.MAX_SHOP_RESOURCE, Constants.SHOP_RESOURCE_TYPES];

        for (int i = 0; i < Constants.MAX_SHOP_RESOURCE; i++)
        {
            CoinDatas[i, 0] = Data[i]["Name"].ToString();
            CoinDatas[i, 1] = Data[i]["Amount"].ToString();
            CoinDatas[i, 2] = Data[i]["Price"].ToString();
            CoinDatas[i, 3] = Data[i]["Icon"].ToString();
            CoinDatas[i, 4] = Data[i]["Type"].ToString();
            CoinDatas[i, 5] = Data[i]["Detail"].ToString();
        }
    }

    public void MinusAdLeft()
    {
        GameManager.Inst().AdsManager.AdLeft--;
        SetAdLeftText();

        if (GameManager.Inst().AdsManager.AdLeft <= 0)
        {
            DisableAd();
            GameManager.Inst().AdsManager.SetLastTime();
            return;
        }
    }

    void BuyDailyJewel(int type)
    {
        GameManager.Inst().DatManager.GameData.StartDailyJewel();

        if (type == 0)
        {
            GameManager.Inst().DatManager.GameData.IsDaily = true;
            GameManager.Inst().DatManager.GameData.DailyLeft = 30;
            Jewels[5].Button.interactable = false;
        }
        else
        {
            GameManager.Inst().DatManager.GameData.IsDailyPlus = true;
            GameManager.Inst().DatManager.GameData.DailyPlusLeft = 30;
            Jewels[6].Button.interactable = false;
        }

        GameManager.Inst().DatManager.GameData.GiveDaily();
    }

    public void FailBuyJewel()
    {
        IsFail = true;
    }

    void FailMessage()
    {
        FailMsg.alpha += Time.deltaTime * MsgSpeed;

        if (FailMsg.alpha >= 1.0f)
            MsgSpeed *= -1.0f;
        else if (FailMsg.alpha <= 0.0f)
        {
            MsgSpeed *= -1.0f;
            IsFail = false;
        }
    }

    void BuyCoin()
    {
        int price = int.Parse(CoinDatas[CurrentItem, 2]);
        int amount = int.Parse(CoinDatas[CurrentItem, 1]);
        int type = int.Parse(CoinDatas[CurrentItem, 4]);

        if (GameManager.Inst().Jewel < price)
            return;

        GameManager.Inst().AddJewel(-price);
        GameManager.Inst().Player.AddCoin(amount);

        GameManager.Inst().UiManager.MainUI.PopupReward.gameObject.SetActive(true);
        GameManager.Inst().UiManager.MainUI.PopupReward.Show((int)PopupReward.RewardType.COIN, amount);
    }
    

    void ShowConfirm()
    {
        switch (CurrentPage)
        {
            case 0:
                Icon.sprite = JewelIcons[CurrentItem];
                NameText.text = JewelDatas[CurrentItem, 0];
                Money.SetActive(true);
                Jewel.SetActive(false);
                PriceText.text = JewelDatas[CurrentItem, 2];
                DetailText.text = JewelDatas[CurrentItem, 3];
                break;
            case 1:
                Icon.sprite = CoinIcons[CurrentItem];
                NameText.text = CoinDatas[CurrentItem, 0];
                Money.SetActive(false);
                Jewel.SetActive(true);
                PriceText.text = CoinDatas[CurrentItem, 2];
                DetailText.text = CoinDatas[CurrentItem, 5];
                break;
        }
    }

    public void EnableAd()
    {
        TimerText.gameObject.SetActive(false);
        AdLeftText.gameObject.SetActive(true);
        AdButton.interactable = true;
        AdButton.targetGraphic.color = AdButton.colors.normalColor;
    }

    public void DisableAd()
    {
        TimerText.gameObject.SetActive(true);
        AdLeftText.gameObject.SetActive(false);
        AdButton.interactable = false;
        AdButton.targetGraphic.color = AdButton.colors.disabledColor;
    }

    void SetTimer()
    {
        System.TimeSpan gap = GameManager.Inst().AdsManager.LastTime - System.DateTime.Now;

        TimerText.text = gap.Hours.ToString() + " : " +
                        gap.Minutes.ToString() + " : " +
                        gap.Seconds.ToString();

        if (gap.TotalSeconds <= 0)
        {
            EnableAd();
            GameManager.Inst().AdsManager.AdLeft = Constants.ADMAX;
        }
    }

    public void SetAdLeftText()
    {
        AdLeftText.text = GameManager.Inst().AdsManager.AdLeft.ToString() + " / " + Constants.ADMAX.ToString();
    }

    void SetResultData(int index)
    {
        ResultIcon.sprite = JewelIcons[index];
        ResultNameText.text = JewelDatas[index, 0];
    }

    public void BuyFullPrice()
    {
        GameManager.Inst().IsFullPrice = true;
        GameManager.Inst().DatManager.GameData.IsFullPrice = true;
        GameManager.Inst().BufManager.StartBuff(0);

        FullPriceLock.SetActive(true);
        GameManager.Inst().UiManager.MainUI.SpecialBtn.SetActive(false);

        GameManager.Inst().UiManager.MainUI.PopupReward.Show((int)PopupReward.RewardType.COIN, 50000);

        GameManager.Inst().DatManager.SaveData();
        GameManager.Inst().DatManager.UploadSaveData();
    }

    public void OnClickItem(int index)
    {
        CurrentPage = 1;
        CurrentItem = index;

        Confirm.SetActive(true);
        ShowConfirm();
    }

    public void OnClickBuyBtn()
    {
        BuyCoin();

        Confirm.SetActive(false);

        //자동 저장 및 데이터 업로드
        GameManager.Inst().DatManager.AutoSave();
    }

    public void OnClickConfirmCancel()
    {
        Confirm.SetActive(false);
    }

    public void OnClickBuyJewel(int index)
    {
        Confirm.SetActive(false);
        //Result.SetActive(true);
        //SetResultData(index);

        int amount = int.Parse(JewelDatas[index, 1]);

        if (index > 4)
        {
            //Daily 보상팝업
            GameManager.Inst().UiManager.MainUI.PopupReward.gameObject.SetActive(true);
            GameManager.Inst().UiManager.MainUI.PopupReward.Show((int)PopupReward.RewardType.CRYSTAL, amount);

            BuyDailyJewel(index - 5);
        }
        else
        {
            GameManager.Inst().UiManager.MainUI.PopupReward.gameObject.SetActive(true);
            GameManager.Inst().UiManager.MainUI.PopupReward.Show((int)PopupReward.RewardType.CRYSTAL, amount);

            GameManager.Inst().AddJewel(amount);
        }

        //자동 저장 및 데이터 업로드
        GameManager.Inst().DatManager.AutoSave();
    }

    public void OnClickResultOK()
    {
        Result.SetActive(false);
    }

    public void OnClickBuyAdJewel()
    {
        GameManager.Inst().AdsManager.AdvType = AdvertiseManager.AdType.SHOP_JEWEL;
        GameManager.Inst().AdsManager.PlayAd();
    }

    public void OnClickBuyResourcePackage()
    {
        for (int i = 0; i < Constants.MAXRESOURCETYPES; i++)
        {
            GameManager.Inst().AddResource(i + 1, 200);
            GameManager.Inst().UiManager.MainUI.PopupReward.Show(i + (int)PopupReward.RewardType.RESOURCE_1, 200);
        }
    }

    public void OnClickBuyEquipPackage()
    {
        for (int i = 0; i < Constants.MAXEQUIPTYPE; i++)
        {
            GameManager.Inst().MakeEquipData(i, 2);
            GameManager.Inst().UiManager.MainUI.PopupReward.Show(i + (int)PopupReward.RewardType.EQUIP_MAGNET, 0, 2);
        }
    }
}