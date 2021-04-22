using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryScroll : MonoBehaviour
{
    public GameObject Contents;
    public GameObject None;

    InventorySlot[] Slots;

    int[] SwitchedIndices;

    public InventorySlot GetSlot(int index) { return Slots[index]; /*return Contents.transform.GetChild(index + 1).gameObject.GetComponent<InventorySlot>();*/ }
    public int GetSwitchedIndex(int index) { return SwitchedIndices[index]; }

    public void SetInventory(int index, InventorySlot slot) { Slots[index] = slot; }

    //public void SetSlotIndex(int index)
    //{
    //    Contents.transform.GetChild(index + 1).GetComponent<InventorySlot>().SetIndex(index);
    //}

    public void SetSlotType(int type)
    {
        int max = Constants.MAXINVENTORY;

        for (int i = 0; i < max; i++)
            Contents.transform.GetChild(i + 1).GetComponent<InventorySlot>().SetType(type);
    }

    void Start()
    {
        Slots = new InventorySlot[Constants.MAXINVENTORY];
        SwitchedIndices = new int[Constants.MAXINVENTORY];
        for (int i = 0; i < Constants.MAXINVENTORY; i++)
            SwitchedIndices[i] = i;
    }

    public void ShowInventory()
    {
        for (int i = 0; i < Constants.MAXINVENTORY; i++)
        {
            Player.EqData eq = GameManager.Inst().Player.GetItem(i);
            if (eq != null)
            {
                Sprite icon = eq.Icon;
                if (eq.UID / 100 == 3)
                    icon = GameManager.Inst().UiManager.FoodImages[eq.Type + eq.Rarity * Constants.MAXREINFORCETYPE];

                InventorySlot slot = GetSlot(i);

                slot.GetNotExist().SetActive(false);
                slot.GetExist().SetActive(true);
                slot.SetIcon(icon);
                slot.SetDisable(false);
                slot.SetGradeSprite(eq.Rarity);

                slot.SetItemRarity(eq.Rarity);
                slot.SetItemType(eq.Type);
                slot.SetItemQuantity(eq.Quantity);
                slot.SetItemUID(eq.UID);

                if (eq.Quantity > 0)
                {
                    slot.Quantity.SetActive(true);
                    slot.QuantityText.text = eq.Quantity.ToString();
                }
            }
            else
                HideSlot(i);
        }

        GameManager.Inst().Player.SortOption = (int)InventorySlot.SortOption.RARITY;

        Sort();
    }

    public void HideSlot(int i)
    {
        InventorySlot slot = GetSlot(i);

        slot.GetNotExist().SetActive(true);
        slot.GetExist().SetActive(false);

        slot.SetItemRarity(-1);
        slot.SetItemType(-1);
        slot.SetItemUID(-1);
        slot.SetItemQuantity(0);
    }

    public void Sort()
    {
        Array.Sort(Slots);
        Array.Reverse(Slots);

        for (int i = 0; i < Constants.MAXINVENTORY; i++)
        {
            for (int j = 0; j < Constants.MAXINVENTORY; j++)
            {
                if (Slots[i].name == Contents.transform.GetChild(j + 1).gameObject.name)
                {
                    Contents.transform.GetChild(j + 1).SetSiblingIndex(i + 1);

                    SwitchedIndices[int.Parse(Slots[i].name)] = i;
                    break;
                }
            }
        }
    }

    public void ResetInventory()
    {
        GameManager.Inst().Player.SortOption = (int)InventorySlot.SortOption.BASE;

        Sort();
    }

    //void QuickSort(InventorySlot[] array, int p, int r)
    //{
    //    if (p < r)
    //    {
    //        int q = Partition(array, p, r);
    //        QuickSort(array, p, q - 1);
    //        QuickSort(array, q + 1, r);
    //    }
    //}

    //int Partition(InventorySlot[] array, int p, int r)
    //{
    //    int q = p;
    //    for (int j = p; j < r; j++)
    //    {
    //        if (array[j].CompareTo(array[r]) == -1)
    //        {
    //            Swap(array, q, j);
    //            q++;
    //        }
    //    }
    //    Swap(array, q, r);
    //    return q;
    //}

    //void Swap(InventorySlot[] array, int beforeIndex, int foreIndex)
    //{
    //    var tmp = array[beforeIndex];
    //    array[beforeIndex] = array[foreIndex];
    //    array[foreIndex] = tmp;
    //}
}

//switch (eq.Type)
//{
//    case 0:
//        slot.GetIcon().transform.rotation = Quaternion.Euler(0.0f, 0.0f, 60.0f);
//        break;
//    case 1:
//        slot.GetIcon().transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
//        break;
//    case 2:
//        slot.GetIcon().transform.rotation = Quaternion.Euler(0.0f, 0.0f, -60.0f);
//        break;
//}



//for (int i = 0; i < Constants.MAXINVENTORY; i++)
//{
//    for (int j = 0; j < Constants.MAXINVENTORY; j++)
//    {
//        if (Slots[i] == null)
//            break;

//        if (Slots[i].GetItemUID() != -1 &&
//            Slots[i].GetItemUID() == Contents.transform.GetChild(j + 1).GetComponent<InventorySlot>().GetItemUID())
//        {
//            Contents.transform.GetChild(j + 1).SetSiblingIndex(i + 1);
//            break;
//        }
//    }

//    SwitchedIndices[int.Parse(Slots[i].name)] = i;
//}
