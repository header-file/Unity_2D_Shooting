using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject[] InventoryUI;
    Player Player;
    

    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void ShowInventory()
    {
        gameObject.SetActive(true);

        
        Player = GameManager.Inst().Player;
        for (int i = 0; i < Player.MAXINVENTORY; i++)
        {
            Player.EqData eq = Player.GetItem(i);
            if (eq.Icon != null)
            {
                InventoryUI[i].transform.GetChild(1).gameObject.SetActive(false);
                InventoryUI[i].transform.GetChild(0).gameObject.SetActive(true);

                Sprite icon = eq.Icon;
                InventoryUI[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = icon;
            }
        }
    }
}
