using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject[] InventoryUI;
    Player Player;

    bool IsAlreadyMade;

    void Awake()
    {
        gameObject.SetActive(false);
        IsAlreadyMade = false;
    }

    public void ShowInventory()
    {
        gameObject.SetActive(true);

        //if (IsAlreadyMade)
        //{
        //    for (int i = 0; i < Player.MAXINVENTORY; i++)
        //    {
        //        Item_Equipment eq = Player.GetItem(i);
        //        if (eq != null)
        //        {
        //            InventoryUI[i].transform.GetChild(1).gameObject.SetActive(false);
        //            InventoryUI[i].transform.GetChild(0).gameObject.SetActive(true);

        //            //InventoryUI[i / 5, i % 5].transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = eq.GetIcon();
        //        }
        //    }
        //}
        //else
        {
            Player = GameManager.Inst().Player;
            for (int i = 0; i < Player.MAXINVENTORY; i++)
            {
                //Debug.Log("Made InventorySlot");
                //Vector2 pos = Vector2.zero;
                //pos.x = -240 + 120 * (i % 5);
                //pos.y = 200 - 100 * (i / 5);
                //InventoryUI[i / 5, i % 5].GetComponent<RectTransform>().anchoredPosition = pos;

                Item_Equipment eq = Player.GetItem(i);
                if (eq != null)
                {
                    InventoryUI[i].transform.GetChild(1).gameObject.SetActive(false);
                    InventoryUI[i].transform.GetChild(0).gameObject.SetActive(true);

                    //InventoryUI[i / 5, i % 5].transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = eq.GetIcon();
                }
                //else
                //{

                //}
            }

            IsAlreadyMade = false;
        }
    }
}
