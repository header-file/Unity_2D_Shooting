﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventoryDetail : MonoBehaviour
{
    public Image Icon;
    public GameObject Type;
    public Text Value;
    public GameObject[] Grades;
    public Image Frame;
    public GameObject Values;
    public GameObject Detail;
    public Text DetailText;
    public GameObject Grade;
    public SellConfirm SellConfirm;

    int Index;


    public void ShowDetail(int index)
    {
        Player.EqData equip = GameManager.Inst().Player.GetItem(index);
        Index = index;

        Icon.sprite = equip.Icon;
        
        int rarity = equip.Rarity;
        SetRarityColor(rarity);

        int type = equip.Type;
        SetTypeName(type);

        if (equip.UID / 100 == 3)
        {
            Icon.gameObject.SetActive(false);
            int val = (int)equip.Value;
            Grade.SetActive(true);
            SetValue(val);
        }
        else if (equip.UID / 100 == 6)
        {
            Icon.gameObject.SetActive(true);
            Grade.SetActive(false);
            SetDetail(equip.Type, equip.Rarity, equip.Value);
        }
    }    

    void SetRarityColor(int rarity)
    {
        //Item_Equipment.Rarity rare = (Item_Equipment.Rarity)rarity;
        for (int i = 0; i < 5; i++)
            Grades[i].SetActive(false);

        Grades[rarity].SetActive(true);
    }

    void SetTypeName(int type)
    {
        Item_Equipment.EquipmentType t = (Item_Equipment.EquipmentType)type;
        Text text = Type.GetComponent<Text>();
        switch (t)
        {
            case Item_Equipment.EquipmentType.ATTACK:
                text.text = "ATTACK";
                break;

            case Item_Equipment.EquipmentType.HP:
                text.text = "RANGE";
                break;

            case Item_Equipment.EquipmentType.SPEED:
                text.text = "SPEED";
                break;
        }
    }

    void SetValue(int val)
    {
        Values.SetActive(true);
        Detail.SetActive(false);

        Value.text = val.ToString();
    }

    void SetDetail(int type, int rarity, float value)
    {
        Values.SetActive(false);
        Detail.SetActive(true);

        string detail = "";
        if (GameManager.Inst().EquipDatas[type, rarity, 0] > 0)
            detail += GameManager.Inst().EquipDatas[type, rarity, 0].ToString();
        detail += GameManager.Inst().TxtManager.EquipDetailFront[type];
        if (value > 0)
            detail += value.ToString();
        detail += GameManager.Inst().TxtManager.EquipDetailBack[type];

        DetailText.text = detail;
    }

    public void ShowSell()
    {
        Player.EqData equip = GameManager.Inst().Player.GetItem(Index);

        SellConfirm.gameObject.SetActive(true);

        SellConfirm.PriceText.text = Mathf.Pow(10, equip.Rarity + 1).ToString();
        SellConfirm.QuantityText.text = "1";

        if(equip.UID / 100 == 3)
        {
            if(equip.Quantity > 1)
                SellConfirm.UpBtn.interactable = true;
            SellConfirm.DownBtn.interactable = false;
        }
        else if (equip.UID / 100 == 6)
        {
            SellConfirm.UpBtn.interactable = false;
            SellConfirm.DownBtn.interactable = false;
        }

        if (SceneManager.GetActiveScene().name == "Stage0" && (GameManager.Inst().Tutorials.Step == 5 || GameManager.Inst().Tutorials.Step == 41))
            GameManager.Inst().Tutorials.Step++;
    }

    public void OnClickUpBtn()
    {
        Player.EqData equip = GameManager.Inst().Player.GetItem(Index);
        int quantity = int.Parse(SellConfirm.QuantityText.text);
        quantity++;

        SellConfirm.QuantityText.text = quantity.ToString();
        SellConfirm.PriceText.text = (quantity * Mathf.Pow(10, equip.Rarity + 1)).ToString();

        SellConfirm.DownBtn.interactable = true;
        if (quantity >= equip.Quantity)
            SellConfirm.UpBtn.interactable = false;
    }

    public void OnClickDownBtn()
    {
        Player.EqData equip = GameManager.Inst().Player.GetItem(Index);
        int quantity = int.Parse(SellConfirm.QuantityText.text);
        quantity--;

        SellConfirm.QuantityText.text = quantity.ToString();
        SellConfirm.PriceText.text = (quantity * Mathf.Pow(10, equip.Rarity + 1)).ToString();

        SellConfirm.UpBtn.interactable = true;
        if (quantity <= 1)
            SellConfirm.DownBtn.interactable = false;
    }

    public void OnClickYesBtn()
    {
        int quantity = int.Parse(SellConfirm.QuantityText.text);
        GameManager.Inst().Player.RemoveItem(Index, quantity);

        int price = int.Parse(SellConfirm.PriceText.text);
        GameManager.Inst().Player.AddCoin(price);

        SellConfirm.gameObject.SetActive(false);
        Inventory inv = GameManager.Inst().UiManager.Inventory.GetComponent<Inventory>();
        inv.CloseInventory();
        inv.ShowInventory();
        gameObject.SetActive(false);
    }

    public void OnClickNoBtn()
    {
        SellConfirm.gameObject.SetActive(false);
    }
}
