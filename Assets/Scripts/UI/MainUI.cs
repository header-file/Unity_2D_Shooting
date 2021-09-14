using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    //t
    public Alarm Alarm;
    public Text CoinText;
    public Text JewelText;

    public BossGauge BossGauge;
    public SideMenu SideMenu;
    public GameObject Resource;

    public Bottom Bottom;
    public Center Center;
    public ZzinBottom ZzinBottom;

    void Awake()
    {
        for (int i = 0; i < 5; i++)
            Bottom.Arrows.transform.GetChild(i).gameObject.SetActive(false);
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

        Center.Menu.gameObject.SetActive(true);
    }
}
