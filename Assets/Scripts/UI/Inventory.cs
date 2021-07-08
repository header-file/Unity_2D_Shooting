using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

        Inventories = GameManager.Inst().UiManager.InventoryScroll.GetComponent<InventoryScroll>();
        Inventories.transform.SetParent(InventoryArea.transform, false);
        Inventories.SetSlotType(0);

        Inventories.ShowInventory();
    }

    public void CloseInventory()
    {
        Inventories.ResetInventory();
    }

    public void OnClickSell()
    {
        GameManager.Inst().UiManager.InventoryDetail.GetComponent<InventoryDetail>().ShowSell();

        if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 45)
            GameManager.Inst().Tutorials.Step++;
    }
}
