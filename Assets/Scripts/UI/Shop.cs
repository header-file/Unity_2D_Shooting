using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Toggle[] Toggles;
    public GameObject[] Pages;
    public ShopSlot[] Jewels;
    public ShopSlot[] Resources;
    public ShopSlot[] Packages;
    public Sprite[] JewelIcons;
    public Sprite[] ResourceIcons;
    public Sprite[] PackageIcons;
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
    string[,] ResourceDatas;
    string[,] PackageDatas;
    int CurrentPage;
    int CurrentItem;
    bool IsFail;
    float MsgSpeed;

    void Awake()
    {
        SetJewelDatas();
        SetResourceDatas();
        SetPackageDatas();

        FailMsg.alpha = 0.0f;
        IsFail = false;
        MsgSpeed = 2.0f;
    }

    void Start()
    {
        CurrentPage = 0;
        CurrentItem = -1;

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

    public void ShowPage(int index)
    {
        if (JewelDatas == null)
            return;

        switch (index)
        {
            case 0:
                for (int i = 0; i < Jewels.Length; i++)
                {
                    Jewels[i].NameText.text = JewelDatas[i, 0];
                    Jewels[i].PriceText.text = JewelDatas[i, 2];
                    Jewels[i].Icon.sprite = JewelIcons[i];
                    Jewels[i].Jewel.SetActive(false);

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
                break;
            case 1:
                for (int i = 0; i < Resources.Length; i++)
                {
                    Resources[i].NameText.text = ResourceDatas[i, 0];
                    Resources[i].PriceText.text = ResourceDatas[i, 2];
                    Resources[i].Icon.sprite = ResourceIcons[int.Parse(ResourceDatas[i, 3])];
                    Resources[i].Jewel.SetActive(true);
                }
                break;
            case 2:
                for (int i = 0; i < Packages.Length; i++)
                {
                    Packages[i].NameText.text = PackageDatas[i, 0];
                    Packages[i].PriceText.text = PackageDatas[i, 2];
                    Packages[i].Icon.sprite = PackageIcons[i];
                    Packages[i].Jewel.SetActive(true);
                }
                break;
            case 3:
                if (GameManager.Inst().IsFullPrice)
                    FullPriceLock.SetActive(true);
                break;
        }
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

    void SetResourceDatas()
    {
        Data = CSVReader.Read("Datas/ShopResourceData");
        ResourceDatas = new string[Constants.MAX_SHOP_RESOURCE, Constants.SHOP_RESOURCE_TYPES];

        for (int i = 0; i < Constants.MAX_SHOP_RESOURCE; i++)
        {
            ResourceDatas[i, 0] = Data[i]["Name"].ToString();
            ResourceDatas[i, 1] = Data[i]["Amount"].ToString();
            ResourceDatas[i, 2] = Data[i]["Price"].ToString();
            ResourceDatas[i, 3] = Data[i]["Icon"].ToString();
            ResourceDatas[i, 4] = Data[i]["Type"].ToString();
            ResourceDatas[i, 5] = Data[i]["Detail"].ToString();
        }
    }

    void SetPackageDatas()
    {
        Data = CSVReader.Read("Datas/ShopPackageData");
        PackageDatas = new string[Constants.MAX_SHOP_PACKAGE, Constants.SHOP_PACKAGE_TYPES];

        for (int i = 0; i < Constants.MAX_SHOP_PACKAGE; i++)
        {
            PackageDatas[i, 0] = Data[i]["Name"].ToString();
            PackageDatas[i, 1] = Data[i]["Amount"].ToString();
            PackageDatas[i, 2] = Data[i]["Price"].ToString();
            PackageDatas[i, 3] = Data[i]["Detail"].ToString();
            PackageDatas[i, 4] = Data[i]["Coin"].ToString();
            PackageDatas[i, 5] = Data[i]["A"].ToString();
            PackageDatas[i, 6] = Data[i]["B"].ToString();
            PackageDatas[i, 7] = Data[i]["C"].ToString();
            PackageDatas[i, 8] = Data[i]["D"].ToString();
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

    void BuyResource()
    {
        int price = int.Parse(ResourceDatas[CurrentItem, 1]);
        int amount = int.Parse(ResourceDatas[CurrentItem, 0]);
        int type = int.Parse(ResourceDatas[CurrentItem, 3]);

        if (GameManager.Inst().Jewel < price)
            return;

        GameManager.Inst().AddJewel(-price);

        switch (type)
        {
            case 0:
                GameManager.Inst().Player.AddCoin(amount);
                break;
            case 1:
            case 2:
            case 3:
            case 4:
                GameManager.Inst().AddResource(type, amount);
                break;
        }
    }

    void BuyPackage()
    {
        int price = int.Parse(PackageDatas[CurrentItem, 1]);

        if (GameManager.Inst().Jewel < price)
            return;

        GameManager.Inst().AddJewel(-price);

        GameManager.Inst().Player.AddCoin(int.Parse(PackageDatas[CurrentItem, 4]));
        for (int i = 0; i < Constants.MAXSTAGES; i++)
            GameManager.Inst().AddResource(i, int.Parse(PackageDatas[CurrentItem, i + 5]));
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
                Icon.sprite = ResourceIcons[CurrentItem];
                NameText.text = ResourceDatas[CurrentItem, 0];
                Money.SetActive(false);
                Jewel.SetActive(true);
                PriceText.text = ResourceDatas[CurrentItem, 2];
                DetailText.text = ResourceDatas[CurrentItem, 5];
                break;
            case 2:
                Icon.sprite = PackageIcons[CurrentItem];
                NameText.text = PackageDatas[CurrentItem, 0];
                Money.SetActive(false);
                Jewel.SetActive(true);
                PriceText.text = PackageDatas[CurrentItem, 2];
                DetailText.text = PackageDatas[CurrentItem, 3];
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
        GameManager.Inst().BufManager.StartBuff(0);

        FullPriceLock.SetActive(true);
        GameManager.Inst().UiManager.MainUI.SpecialBtn.SetActive(false);

        GameManager.Inst().DatManager.SaveData();
        GameManager.Inst().DatManager.UploadSaveData();
    }

    public void OnSelectToggle(int index)
    {
        Pages[index].SetActive(Toggles[index].isOn);
        CurrentPage = index;
        
        if (Toggles[index].isOn)
            ShowPage(index);
    }

    public void OnClickItem(int index)
    {
        CurrentItem = index;

        Confirm.SetActive(true);
        ShowConfirm();
    }

    public void OnClickBuyBtn()
    {
        switch(CurrentPage)
        {
            case 1:
                BuyResource();
                break;
            case 2:
                BuyPackage();
                break;
        }

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
        Result.SetActive(true);
        SetResultData(index);

        if (index > 4)
            BuyDailyJewel(index - 5);
        else
            GameManager.Inst().AddJewel(int.Parse(JewelDatas[index, 1]));

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
}