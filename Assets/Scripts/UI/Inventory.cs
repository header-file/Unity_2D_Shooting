using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject Inventories;
    Player Player;
    

    private void Start()
    {
        Player = GameManager.Inst().Player;

        for (int i = 1; i <= Player.MAXINVENTORY; i++)
        {
            GameObject inventorySlot = GameManager.Inst().ObjManager.MakeObj("InventorySlot");
            inventorySlot.transform.SetParent(Inventories.transform, false);
            InventorySlot slot = inventorySlot.GetComponent<InventorySlot>();
            slot.SetIndex(i);
            slot.SetType(0);
        }

        gameObject.SetActive(false);
    }

    public void ShowInventory()
    {
        gameObject.SetActive(true);
        
        for (int i = 1; i <= Player.MAXINVENTORY; i++)
        {
            Player.EqData eq = Player.GetItem(i);
            if (eq.Icon != null)
            {
                Inventories.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
                Inventories.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);

                Sprite icon = eq.Icon;
                InventorySlot slot = Inventories.transform.GetChild(i).GetComponent<InventorySlot>();
                slot.SetIcon(icon);

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
        }
    }
}
