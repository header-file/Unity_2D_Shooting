using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject InventoryArea;

    InventoryScroll Inventories;
    Player Player;
    

    private void Start()
    {
        Player = GameManager.Inst().Player;
        
        gameObject.SetActive(false);
    }

    public void ShowInventory()
    {
        gameObject.SetActive(true);

        Inventories = GameManager.Inst().Inventory.GetComponent<InventoryScroll>();
        Inventories.transform.SetParent(InventoryArea.transform, false);
        Inventories.SetSlotType(0);
        
        for (int i = 1; i <= Player.MAXINVENTORY; i++)
        {
            Player.EqData eq = Player.GetItem(i);
            if (eq != null)
            {
                Sprite icon = eq.Icon;
                InventorySlot slot = Inventories.GetSlot(i);

                slot.GetNotExist().SetActive(false);
                slot.GetExist().SetActive(true);
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
