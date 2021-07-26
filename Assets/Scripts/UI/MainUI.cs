using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    public GameObject Arrows;
    public Menu Menu;
    public BossGauge BossGauge;


    void Awake()
    {
        for (int i = 0; i < 5; i++)
            Arrows.transform.GetChild(i).gameObject.SetActive(false);
    }

    public void OnClickUniverse()
    {
        GameManager.Inst().UiManager.OnClickSpaceBtn();
    }

    public void OnClickWeapon()
    {
        GameManager.Inst().UiManager.OnClickWeaponBtn();
    }

    public void OnClickInventory()
    {
        GameManager.Inst().UiManager.OnClickInventoryBtn();
    }

    public void OnClickSynthesis()
    {
        GameManager.Inst().UiManager.OnClickSynthesisBtn();
    }

    public void OnClickShop()
    {
        GameManager.Inst().UiManager.OnClickShopBtn();
    }

    public void OnClickHome()
    {
        GameManager.Inst().UiManager.OnClickHomeBtn();
    }

    public void OnClickMenu()
    {
        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);

        Menu.gameObject.SetActive(true);
    }
}
