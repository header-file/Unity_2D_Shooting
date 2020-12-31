using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScroll : MonoBehaviour
{
    public GameObject Contents;
    public GameObject None;

    public InventorySlot GetSlot(int index) { return Contents.transform.GetChild(index + 1).gameObject.GetComponent<InventorySlot>(); }

    public void SetSlotIndex(int index)
    {
        Contents.transform.GetChild(index + 1).GetComponent<InventorySlot>().SetIndex(index);
    }

    public void SetSlotType(int type)
    {
        int max = GameManager.Inst().Player.MAXINVENTORY;

        for (int i = 1; i <= max; i++)
            Contents.transform.GetChild(i).GetComponent<InventorySlot>().SetType(type);
    }

    public void MoveBack(int i)
    {
        Contents.transform.GetChild(i + 1).SetSiblingIndex(i + 2);
        //GetSlot(i).SetIndex(i + 1);
        //GetSlot(i + 1).SetIndex(i);
    }

    public void MoveFront(int i)
    {
        Contents.transform.GetChild(i + 1).SetSiblingIndex(i);
        
        //GetSlot(i).SetIndex(i - 1);
        //GetSlot(i - 1).SetIndex(i);
    }

    public void ShowInventory()
    {
        int max = GameManager.Inst().Player.MAXINVENTORY;

        for (int i = 0; i < max; i++)
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
            }
        }
    }
}
