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
    public Text AdLeftText;
    public int AdLeft;

    const int Ads = 5;
    List<Dictionary<string, object>> Data;
    string[] ExpandNames;
    string[] ResourceNames;
    string[] PackageNames;
    int[,] ExpandDatas;
    int[,] ResourceDatas;
    int[,] PackageDatas;

    void Awake()
    {
        SetExpandDatas();
        SetResourceDatas();
        SetPackageDatas();
    }

    void Start()
    {
        AdLeft = Ads;
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
                }
                break;
            case 2:
                for (int i = 0; i < Resources.Length; i++)
                {
                    Resources[i].NameText.text = ResourceNames[i];
                    Resources[i].PriceText.text = ResourceDatas[i, 1].ToString();
                    Resources[i].Icon.sprite = ResourceIcons[ResourceDatas[i, 2]];
                }
                break;
            case 3:
                for (int i = 0; i < Packages.Length; i++)
                {
                    Packages[i].NameText.text = PackageNames[i];
                    Packages[i].PriceText.text = PackageDatas[i, 1].ToString();
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

    public void OnSelectToggle(int index)
    {
        Pages[index].SetActive(Toggles[index].isOn);
        
        if (Toggles[index].isOn)
            ShowPage(index);
    }
}