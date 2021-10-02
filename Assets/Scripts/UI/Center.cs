using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Center : MonoBehaviour
{
    public Inventory Inventory;
    public Weapon Weapon;
    public Synthesis Synthesis;
    public StageLoop StageScroll;
    public Shop Shop;
    public Cheat Cheat;
    public ReviveAlert ReviveAlert;
    public DailyLeft DailyLeft;
    public DailyLeft DailyPlusLeft;
    public Menu Menu;
    public Animation InventoryFull;
    public Animation BossWarning;
    public DailyJewel DailyJewelUI;
    public Turret[] Turrets;


    public void PlayBossWarning()
    {
        BossWarning.gameObject.SetActive(true);
        BossWarning.Play();

        Invoke("EndPlayWarning", 2.0f);
    }

    public void PlayInventoryFull()
    {
        InventoryFull.gameObject.SetActive(true);
        InventoryFull.Play();

        Invoke("EndPlayWarning", 0.45f);
    }

    void EndPlayWarning()
    {
        BossWarning.gameObject.SetActive(false);
        InventoryFull.gameObject.SetActive(false);
    }
}
