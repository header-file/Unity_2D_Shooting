using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InventorySlot : MonoBehaviour, IComparable<InventorySlot>
{
    public GameObject EMark;
    public GameObject Selected;
    public GameObject Exist;
    public GameObject NotExist;
    public GameObject Icon;
    public GameObject Disable;
    public Sprite[] Grades;
    public GameObject Grade;
    public GameObject Checked;
    public GameObject Quantity;
    public Text QuantityText;

    int Index = 0;
    int Type = 0;

    int ItemRarity = -1;
    int ItemType = -1;
    int ItemUID = -1;
    int Weight = 0;

    public int GetIndex() { return Index; }
    public GameObject GetIcon() { return Icon; }
    public GameObject GetExist() { return Exist; }
    public GameObject GetNotExist() { return NotExist; }
    public int GetItemUID() { return ItemUID; }

    public void SetIndex(int i) { Index = i; }
    public void SetType(int t) { Type = t; }
    public void SetEmark(bool b) { EMark.SetActive(b); }
    public void SetSelected(bool b) { Selected.SetActive(b); }
    public void SetChecked(bool b) { Checked.SetActive(b); }
    public void SetIcon(Sprite icon) { Icon.GetComponent<Image>().sprite = icon; }
    public void SetGradeSprite(int index) {  Grade.GetComponent<Image>().sprite = Grades[index]; }
    public void SetItemRarity(int rarity) { ItemRarity = rarity; }
    public void SetItemType(int type) { ItemType = type; }
    public void SetItemUID(int id) { ItemUID = id; }

    public int CompareTo(InventorySlot obj)
    {
        Weight = 0;

        if (GameManager.Inst().Player.InputGrade > 10)
        {
            if (ItemType == GameManager.Inst().Player.InputGrade - 11 &&
                obj.ItemType != GameManager.Inst().Player.InputGrade - 11)
                Weight += 16;
            else if (ItemType != GameManager.Inst().Player.InputGrade - 11 &&
                obj.ItemType == GameManager.Inst().Player.InputGrade - 11)
                Weight -= 16;
        }
        else if (GameManager.Inst().Player.InputGrade == 10)
        {
            if (int.Parse(gameObject.name) < int.Parse(obj.name))
                Weight += 16;
            else if (int.Parse(gameObject.name) > int.Parse(obj.name))
                Weight -= 16;
        }
        else if (GameManager.Inst().Player.InputGrade >= 0)
        {
            if (ItemRarity == GameManager.Inst().Player.InputGrade &&
                obj.ItemRarity != GameManager.Inst().Player.InputGrade)
                Weight += 16;
            else if (ItemRarity != GameManager.Inst().Player.InputGrade &&
                obj.ItemRarity == GameManager.Inst().Player.InputGrade)
                Weight -= 16;
        }

        if (ItemRarity > obj.ItemRarity)
            Weight += 8;
        else if (ItemRarity < obj.ItemRarity)
            Weight -= 8;

        if (ItemType < obj.ItemType)
            Weight += 4;
        else if (ItemType > obj.ItemType)
            Weight -= 4;

        if (int.Parse(gameObject.name) < int.Parse(obj.name))
            Weight += 2;
        else if (int.Parse(gameObject.name) > int.Parse(obj.name))
            Weight -= 2;

        if (Weight > 0)
            return 1;
        else if (Weight == 0)
            return 0;
        else
            return -1;
    }

    public void SetDisable(bool b)
    {
        if (b)
        {
            Disable.SetActive(true);
            Exist.GetComponent<Button>().interactable = false;
        }
        else
        {
            Disable.SetActive(false);
            Exist.GetComponent<Button>().interactable = true;
        }
    }

    public void OnClick()
    {
        Debug.Log(Index);
        switch(Type)
        {
            case 0:
                GameManager.Inst().UiManager.OnClickInventoryDetailBtn(Index);
                break;
            case 1:
                GameManager.Inst().UiManager.OnClickEquipSelectBtn(Index);
                break;
            case 2:
                GameManager.Inst().UiManager.OnClickSynthesisSelectBtn(Index);
                break;
        }
    }
}
