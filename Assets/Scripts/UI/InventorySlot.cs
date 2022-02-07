using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class InventorySlot : MonoBehaviour, IComparable<InventorySlot>
{
    public enum SortOption
    {
        BASE = 0,
        RARITY = 1,
        TYPE_RARITY = 10,
        SAMERARITY = 100,
        SYNTHESIS = 1000,
        SYNTHESIS_EQUIP = 2000,
    }

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
    int ItemQuantity = 0;
    int Weight = 0;

    public int GetIndex() { return Index; }
    public GameObject GetIcon() { return Icon; }
    public GameObject GetExist() { return Exist; }
    public GameObject GetNotExist() { return NotExist; }
    public int GetItemUID() { return ItemUID; }
    public int GetSlotType() { return Type; }

    public void SetIndex(int i) { Index = i; }
    public void SetType(int t) { Type = t; }
    public void SetEmark(bool b) { EMark.SetActive(b); }
    public void SetSelected(bool b) { Selected.SetActive(b); }
    public void SetChecked(bool b) { Checked.SetActive(b); }
    public void SetIcon(Sprite icon) { Icon.GetComponent<Image>().sprite = icon; }
    public void SetGradeSprite(int index) {  Grade.GetComponent<Image>().sprite = Grades[index]; }
    public void SetItemRarity(int rarity) { ItemRarity = rarity; }
    public void SetItemType(int type) { ItemType = type; }
    public void SetItemQuantity(int quantity) { ItemQuantity = quantity; }
    public void SetItemUID(int id) { ItemUID = id; }

    public int CompareTo(InventorySlot obj)
    {
        Weight = 0;

        if (GameManager.Inst().Player.SortOption >= (int)SortOption.SYNTHESIS_EQUIP)
        {
            if (ItemRarity == GameManager.Inst().Player.SortOption - (int)SortOption.SYNTHESIS_EQUIP &&
                obj.ItemRarity != GameManager.Inst().Player.SortOption - (int)SortOption.SYNTHESIS_EQUIP)
                Weight += 16;
            else if (ItemRarity != GameManager.Inst().Player.SortOption - (int)SortOption.SYNTHESIS_EQUIP &&
                obj.ItemRarity == GameManager.Inst().Player.SortOption - (int)SortOption.SYNTHESIS_EQUIP)
                Weight -= 16;
        }
        else if (GameManager.Inst().Player.SortOption >= (int)SortOption.SYNTHESIS)
        {
            if (ItemRarity >= 0 && ItemRarity < Constants.MAXRARITY - 1 &&
                obj.ItemRarity == Constants.MAXRARITY - 1)
                Weight += 32;
            else if (ItemRarity == Constants.MAXRARITY - 1 &&
                obj.ItemRarity >= 0 && obj.ItemRarity != Constants.MAXRARITY - 1)
                Weight -= 32;

            if (ItemQuantity >= 3 && obj.ItemQuantity < 3)
                Weight += 16;
            else if (ItemQuantity < 3 && obj.ItemQuantity >= 3)
                Weight -= 16;
        }
        else if (GameManager.Inst().Player.SortOption >= (int)SortOption.SAMERARITY)
        {
            if (ItemRarity == GameManager.Inst().Player.SortOption - (int)SortOption.SAMERARITY &&
                obj.ItemRarity != GameManager.Inst().Player.SortOption - (int)SortOption.SAMERARITY)
                Weight += 16;
            else if (ItemRarity != GameManager.Inst().Player.SortOption - (int)SortOption.SAMERARITY &&
                obj.ItemRarity == GameManager.Inst().Player.SortOption - (int)SortOption.SAMERARITY)
                Weight -= 16;
        }
        else if (GameManager.Inst().Player.SortOption >= (int)SortOption.TYPE_RARITY)
        {
            if (ItemType == GameManager.Inst().Player.SortOption - (int)SortOption.TYPE_RARITY &&
                obj.ItemType != GameManager.Inst().Player.SortOption - (int)SortOption.TYPE_RARITY)
                Weight += 16;
            else if (ItemType != GameManager.Inst().Player.SortOption - (int)SortOption.TYPE_RARITY &&
                obj.ItemType == GameManager.Inst().Player.SortOption - (int)SortOption.TYPE_RARITY)
                Weight -= 16;
        }
        else if (GameManager.Inst().Player.SortOption == (int)SortOption.BASE)
        {
            if (int.Parse(gameObject.name) < int.Parse(obj.name))
                Weight += 16;
            else if (int.Parse(gameObject.name) > int.Parse(obj.name))
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

        //if (int.Parse(gameObject.name) < int.Parse(obj.name))
        //    Weight += 2;
        //else if (int.Parse(gameObject.name) > int.Parse(obj.name))
        //    Weight -= 2;

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
        switch(Type)
        {
            case 0:
                GameManager.Inst().UiManager.MainUI.Center.Inventory.ShowInventoryDetail(Index);
                break;
            case 1:
                GameManager.Inst().UiManager.MainUI.Center.Weapon.OnClickEquipSelectBtn(Index);
                break;
            case 2:
                GameManager.Inst().UiManager.MainUI.Center.Synthesis.OnClickSynthesisSelectBtn(Index);
                break;
            default:
                break;
        }

        if (SceneManager.GetActiveScene().name == "Stage0" && (GameManager.Inst().Tutorials.Step == 36 || GameManager.Inst().Tutorials.Step == 39 || GameManager.Inst().Tutorials.Step == 45 ||
            GameManager.Inst().Tutorials.Step == 49 || GameManager.Inst().Tutorials.Step == 52 || GameManager.Inst().Tutorials.Step == 53 || GameManager.Inst().Tutorials.Step == 54  ||
            GameManager.Inst().Tutorials.Step == 57 || GameManager.Inst().Tutorials.Step == 58 || GameManager.Inst().Tutorials.Step == 59))
            GameManager.Inst().Tutorials.Step++;
    }
}
