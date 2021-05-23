using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Toggle[] Toggles;
    public GameObject[] Pages;
    public ShopSlot[] Expands;
    public ShopSlot[] Resources;
    public ShopSlot[] Packages;
    public Sprite[] ResourceIcons;
    public Sprite[] ExpandIcons;
    public Sprite[] PackageIcons;
    public Text AdLeftText;
    public int AdLeft;

    //Confirm
    public GameObject Confirm;
    public Image Icon;
    public Text NameText;
    public GameObject Money;
    public GameObject Jewel;
    public Text PriceText;

    const int Ads = 5;
    List<Dictionary<string, object>> Data;
    string[] ExpandNames;
    string[] ResourceNames;
    string[] PackageNames;
    int[,] ExpandDatas;
    int[,] ResourceDatas;
    int[,] PackageDatas;
    int CurrentPage;
    int CurrentItem;

    void Awake()
    {
        SetExpandDatas();
        SetResourceDatas();
        SetPackageDatas();
    }

    void Start()
    {
        CurrentPage = 0;
        CurrentItem = -1;
        AdLeft = Ads;
        Confirm.SetActive(false);
        gameObject.SetActive(false);
    }

    public void ShowPage(int index)
    {
        switch(index)
        {
            case 0:
                
                break;
            case 1:
                for (int i = 0; i < Expands.Length; i++)
                {
                    Expands[i].NameText.text = ExpandNames[i];
                    Expands[i].PriceText.text = ExpandDatas[i, 1].ToString();
                    Expands[i].Icon.sprite = ExpandIcons[i];
                    Expands[i].Jewel.SetActive(true);
                }
                break;
            case 2:
                for (int i = 0; i < Resources.Length; i++)
                {
                    Resources[i].NameText.text = ResourceNames[i];
                    Resources[i].PriceText.text = ResourceDatas[i, 1].ToString();
                    Resources[i].Icon.sprite = ResourceIcons[ResourceDatas[i, 2]];
                    Resources[i].Jewel.SetActive(true);
                }
                break;
            case 3:
                for (int i = 0; i < Packages.Length; i++)
                {
                    Packages[i].NameText.text = PackageNames[i];
                    Packages[i].PriceText.text = PackageDatas[i, 1].ToString();
                    Packages[i].Icon.sprite = PackageIcons[i];
                    Packages[i].Jewel.SetActive(true);
                }
                break;
        }
    }

    void SetExpandDatas()
    {
        Data = CSVReader.Read("Datas/ShopExpandData");
        ExpandNames = new string[Constants.MAX_SHOP_EXPAND];
        ExpandDatas = new int[Constants.MAX_SHOP_EXPAND, Constants.SHOP_EXPAND_TYPES];

        for(int i = 0; i < Constants.MAX_SHOP_EXPAND; i++)
        {
            ExpandNames[i] = Data[i]["Name"].ToString();

            ExpandDatas[i, 0] = int.Parse(Data[i]["Amount"].ToString());
            ExpandDatas[i, 1] = int.Parse(Data[i]["Price"].ToString());
        }
    }

    void SetResourceDatas()
    {
        Data = CSVReader.Read("Datas/ShopResourceData");
        ResourceNames = new string[Constants.MAX_SHOP_RESOURCE];
        ResourceDatas = new int[Constants.MAX_SHOP_RESOURCE, Constants.SHOP_RESOURCE_TYPES];

        for (int i = 0; i < Constants.MAX_SHOP_RESOURCE; i++)
        {
            ResourceNames[i] = Data[i]["Name"].ToString();

            ResourceDatas[i, 0] = int.Parse(Data[i]["Amount"].ToString());
            ResourceDatas[i, 1] = int.Parse(Data[i]["Price"].ToString());
            ResourceDatas[i, 2] = int.Parse(Data[i]["Icon"].ToString());
            ResourceDatas[i, 3] = int.Parse(Data[i]["Type"].ToString());
        }
    }

    void SetPackageDatas()
    {
        Data = CSVReader.Read("Datas/ShopPackageData");
        PackageNames = new string[Constants.MAX_SHOP_PACKAGE];
        PackageDatas = new int[Constants.MAX_SHOP_PACKAGE, Constants.SHOP_PACKAGE_TYPES];

        for (int i = 0; i < Constants.MAX_SHOP_PACKAGE; i++)
        {
            PackageNames[i] = Data[i]["Name"].ToString();

            PackageDatas[i, 0] = int.Parse(Data[i]["Amount"].ToString());
            PackageDatas[i, 1] = int.Parse(Data[i]["Price"].ToString());
        }
    }

    public void MinusAdLeft()
    {
        if (AdLeft <= 0)
            return;

        AdLeft--;
        AdLeftText.text = AdLeft.ToString() + " / " + Ads.ToString();
    }

    void BuyExpand()
    {
        if (GameManager.Inst().Jewel < ExpandDatas[CurrentItem, 1])
            return;
        else if (GameManager.Inst().Player.MaxInventory + ExpandDatas[CurrentItem, 0] > 200)
            return;

        GameManager.Inst().AddJewel(-ExpandDatas[CurrentItem, 1]);

        GameManager.Inst().Player.MaxInventory += ExpandDatas[CurrentItem, 0];
        GameManager.Inst().AddInventory();
    }

    void BuyResource()
    {
        if (GameManager.Inst().Jewel < ResourceDatas[CurrentItem, 1])
            return;

        GameManager.Inst().AddJewel(-ResourceDatas[CurrentItem, 1]);

        switch (ResourceDatas[CurrentItem, 3])
        {
            case 0:
                GameManager.Inst().Player.AddCoin(ResourceDatas[CurrentItem, 0]);
                break;
            case 1:
            case 2:
            case 3:
            case 4:
                GameManager.Inst().AddResource(ResourceDatas[CurrentItem, 3], ResourceDatas[CurrentItem, 0]);
                break;
        }
    }

    void BuyPackage()
    {

    }

    void ShowConfirm()
    {
        switch(CurrentPage)
        {
            case 0:
                Money.SetActive(true);
                Jewel.SetActive(false);
                break;
            case 1:
                Icon.sprite = ExpandIcons[CurrentItem];
                NameText.text = ExpandNames[CurrentItem];
                Money.SetActive(false);
                Jewel.SetActive(true);
                PriceText.text = ExpandDatas[CurrentItem, 1].ToString();
                break;
            case 2:
                Icon.sprite = ResourceIcons[CurrentItem];
                NameText.text = ResourceNames[CurrentItem];
                Money.SetActive(false);
                Jewel.SetActive(true);
                PriceText.text = ResourceDatas[CurrentItem, 1].ToString();
                break;
            case 3:
                Icon.sprite = PackageIcons[CurrentItem];
                NameText.text = PackageNames[CurrentItem];
                Money.SetActive(false);
                Jewel.SetActive(true);
                PriceText.text = PackageDatas[CurrentItem, 1].ToString();
                break;
        }
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
        Confirm.SetActive(false);

        switch(CurrentPage)
        {
            case 1:
                BuyExpand();
                break;
            case 2:
                BuyResource();
                break;
            case 3:
                BuyPackage();
                break;
        }
    }

    public void OnClickConfirmCancel()
    {
        Confirm.SetActive(false);
    }

    public void OnClickBuyJewel(int index)
    {

    }
}