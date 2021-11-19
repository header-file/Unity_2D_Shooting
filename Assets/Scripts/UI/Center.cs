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
    public GameObject Turret;
    public Turret[] Turrets;


    public void PlayBossWarning()
    {
        BossWarning.gameObject.SetActive(true);
        BossWarning.Play();

        //GameManager.Inst().SodManager.PlayEffect("Warning boss");

        Invoke("EndPlayWarning", 2.0f);
    }

    public void PlayInventoryFull()
    {
        InventoryFull.gameObject.SetActive(true);
        InventoryFull.Play();

        GameManager.Inst().SodManager.PlayEffect("Warning inventoryFull");

        Invoke("EndPlayWarning", 0.45f);
    }

    void EndPlayWarning()
    {
        //GameManager.Inst().SodManager.StopEffect("Warning boss");
        GameManager.Inst().SodManager.StopEffect("Warning inventoryFull");

        BossWarning.gameObject.SetActive(false);
        InventoryFull.gameObject.SetActive(false);
    }

    public void ShowReviveAlert(int index)
    {
        ReviveAlert.gameObject.SetActive(true);

        ReviveAlert.Show(index);
    }

    public void OnClickReviveBtn()
    {
        GameManager.Inst().AddJewel(-1);
        ReviveAlert.gameObject.SetActive(false);

        GameManager.Inst().GetSubweapons(ReviveAlert.Index).CoolTime = 0;
    }
}
