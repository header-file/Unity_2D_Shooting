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
        int max = GameManager.Inst().Player.MAXINVENTORY;

        for (int i = 0; i < max; i++)
            Contents.transform.GetChild(i + 1).GetComponent<InventorySlot>().SetType(type);
    }

    void Start()
    {
        Slots = new InventorySlot[GameManager.Inst().Player.MAXINVENTORY];
        SwitchedIndices = new int[GameManager.Inst().Player.MAXINVENTORY];
        for (int i = 0; i < GameManager.Inst().Player.MAXINVENTORY; i++)
            SwitchedIndices[i] = i;
    }

    public void ShowInventory()
    {
        for (int i = 0; i < GameManager.Inst().Player.MAXINVENTORY; i++)
        {
            Player.EqData eq = GameManager.Inst().Player.GetItem(i);
            if (eq != null)
            {
                Sprite icon = eq.Icon;
                InventorySlot slot = GetSlot(i);

                slot.GetNotExist().SetActive(false);
                slot.GetExist().SetActive(true);
                slot.SetIcon(icon);
                slot.SetDisable(false);
                slot.SetGradeSprite(eq.Rarity);

                slot.SetItemRarity(eq.Rarity);
                slot.SetItemType(eq.Type);
                slot.SetItemUID(eq.UID);

                if (eq.Quantity > 0)
                {
                    slot.Quantity.SetActive(true);
                    slot.QuantityText.text = eq.Quantity.ToString();
                }

                switch (eq.Type)
                {
                    case 0:
                        slot.GetIcon().transform.rotation = Quaternion.Euler(0.0f, 0.0f, 60.0f);
                        break;
                    case 1:
                        slot.GetIcon().transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                        break;
                    case 2:
                        slot.GetIcon().transform.rotation = Quaternion.Euler(0.0f, 0.0f, -60.0f);
                        break;
                }
            }
            else
            {
                InventorySlot slot = GetSlot(i);

                slot.GetNotExist().SetActive(true);
                slot.GetExist().SetActive(false);

                slot.SetItemRarity(-1);
                slot.SetItemType(-1);
                slot.SetItemUID(-1);
            }
        }

        GameManager.Inst().Player.InputGrade = -1;

        Sort();
    }

    public void Sort()
    {
        //QuickSort(Slots, 0, GameManager.Inst().Player.MAXINVENTORY - 1);
        Array.Sort(Slots);
        Array.Reverse(Slots);

        for (int i = 0; i < GameManager.Inst().Player.MAXINVENTORY; i++)
        {
            for (int j = 0; j < GameManager.Inst().Player.MAXINVENTORY; j++)
            {
                if (Slots[i] == null)
                    break;

                if (Slots[i].GetItemUID() != -1 &&
                    Slots[i].GetItemUID() == Contents.transform.GetChild(j + 1).GetComponent<InventorySlot>().GetItemUID())
                {
                    Contents.transform.GetChild(j + 1).SetSiblingIndex(i + 1);
                    break;
                }
            }

            SwitchedIndices[int.Parse(Slots[i].name)] = i;
        }
    }

    public void ResetInventory()
    {
        GameManager.Inst().Player.InputGrade = 10;

        //QuickSort(Slots, 0, GameManager.Inst().Player.MAXINVENTORY - 1);
        Array.Sort(Slots);
        Array.Reverse(Slots);

        int max = GameManager.Inst().Player.MAXINVENTORY;

        for (int i = 0; i < max; i++)
        {
            for (int j = 0; j < max; j++)
            {
                if (GameManager.Inst().Player.GetItem(j) == null)
                    continue;

                if (Slots[i].GetItemUID() != -1 &&
                    Slots[i].GetItemUID() == GameManager.Inst().Player.GetItem(j).UID)
                {
                    Contents.transform.GetChild(j + 1).SetSiblingIndex(i + 1);
                }
            }

            SwitchedIndices[i] = i;
        }
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
