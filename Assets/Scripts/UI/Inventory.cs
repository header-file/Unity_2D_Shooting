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

        for (int i = 0; i < Player.MAXINVENTORY; i++)
        {
            InventorySlot slot = Inventories.transform.GetChild(i).GetComponent<InventorySlot>();
            slot.SetIndex(i);
            slot.SetType(0);
        }

        gameObject.SetActive(false);
    }

    public void ShowInventory()
    {
        gameObject.SetActive(true);
        
        for (int i = 0; i < Player.MAXINVENTORY; i++)
        {
            Player.EqData eq = Player.GetItem(i);
            if (eq.Icon != null)
            {
                Inventories.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
                Inventories.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);

                Sprite icon = eq.Icon;
                Inventories.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().sprite = icon;

                switch (eq.Type)
                {
                    case 0:
                        Inventories.transform.GetChild(i).GetChild(0).GetChild(0).rotation = Quaternion.Euler(0.0f, 0.0f, 60.0f);
                        break;
                    case 2:
                        Inventories.transform.GetChild(i).GetChild(0).GetChild(0).rotation = Quaternion.Euler(0.0f, 0.0f, -60.0f);
                        break;
                }
            }
        }
    }
}
