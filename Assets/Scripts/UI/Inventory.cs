﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour
{
    public GameObject InventoryArea;
    public InventoryDetail InventoryDetail;
    public Text InventoryCount;

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

        InventoryCount.text = GameManager.Inst().Player.CurInventory.ToString() + " / " +
                                GameManager.Inst().Player.MaxInventory.ToString();

        Inventories.ShowInventory();
    }

    public void CloseInventory()
    {
        Inventories.ResetInventory();
    }

    public void ShowInventoryDetail(int index)
    {
        GameManager.Inst().UiManager.MainUI.Center.Inventory.InventoryDetail.gameObject.SetActive(true);

        GameManager.Inst().UiManager.MainUI.Center.Inventory.InventoryDetail.ShowDetail(index);
    }

    public void OnClickInventoryDetailBackBtn()
    {
        GameManager.Inst().UiManager.MainUI.Center.Inventory.InventoryDetail.gameObject.SetActive(false);
    }

    public void OnClickSell()
    {
        InventoryDetail.ShowSell();

        if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 45)
            GameManager.Inst().Tutorials.Step++;
    }
}
