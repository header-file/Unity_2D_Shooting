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
        int max = GameManager.Inst().Player.MAXINVENTORY;

        for (int i = 1; i <= max; i++)
            Contents.transform.GetChild(i).GetComponent<InventorySlot>().SetIndex(index);
    }

    public void SetSlotType(int type)
    {
        int max = GameManager.Inst().Player.MAXINVENTORY;

        for (int i = 1; i <= max; i++)
            Contents.transform.GetChild(i).GetComponent<InventorySlot>().SetType(type);
    }
}
