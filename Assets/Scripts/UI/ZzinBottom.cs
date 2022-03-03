using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZzinBottom : MonoBehaviour
{
    public GameObject[] UniverseIcon;
    public GameObject[] WeaponIcon;
    public GameObject[] SynthesisIcon;
    public GameObject[] ShopIcon;
    public GameObject HomeBtn;

    void Awake()
    {
        UniverseIcon[0].SetActive(true);
        WeaponIcon[0].SetActive(true);
        SynthesisIcon[0].SetActive(true);
        ShopIcon[0].SetActive(true);

        UniverseIcon[1].SetActive(false);
        WeaponIcon[1].SetActive(false);
        SynthesisIcon[1].SetActive(false);
        ShopIcon[1].SetActive(false);

        HomeBtn.SetActive(false);
    }

    //Button Interact
    public void OnClickSpaceBtn()
    {
        OnClickHomeBtn();

        GameManager.Inst().UiManager.MainUI.Center.StageScroll.gameObject.SetActive(true);
        GameManager.Inst().UiManager.MainUI.Center.StageScroll.Show();

        UniverseIcon[0].SetActive(false);
        UniverseIcon[1].SetActive(true);
        HomeBtn.SetActive(true);

        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);
    }

    public void OnClickSpaceBackBtn()
    {
        GameManager.Inst().UiManager.MainUI.Center.StageScroll.gameObject.SetActive(false);

        UniverseIcon[0].SetActive(true);
        UniverseIcon[1].SetActive(false);
        HomeBtn.SetActive(false);

        GameManager.Inst().IptManager.SetIsAbleControl(true);
        GameManager.Inst().IptManager.SetIsAbleSWControl(true);
    }

    public void OnClickWeaponBtn()
    {
        OnClickHomeBtn();

        GameManager.Inst().UiManager.MainUI.Center.Inventory.gameObject.SetActive(false);
        GameManager.Inst().UiManager.MainUI.Center.Inventory.InventoryDetail.gameObject.SetActive(false);
        GameManager.Inst().UiManager.MainUI.Center.Weapon.gameObject.SetActive(true);

        GameManager.Inst().UiManager.CurrentBulletType = GameManager.Inst().Player.GetBulletType();
        GameManager.Inst().UiManager.MainUI.Center.Weapon.SetCurBulletType(GameManager.Inst().UiManager.CurrentBulletType);
        GameManager.Inst().UiManager.MainUI.Center.Weapon.ShowUI();
        GameManager.Inst().UiManager.SetIsEquip(true);

        GameManager.Inst().UiManager.MainUI.Center.Weapon.InfoArea.gameObject.SetActive(true);
        GameManager.Inst().UiManager.MainUI.Center.Weapon.EquipArea.gameObject.SetActive(false);

        WeaponIcon[0].SetActive(false);
        WeaponIcon[1].SetActive(true);
        HomeBtn.SetActive(true);

        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);
    }

    public void OnClickWeaponBackBtn()
    {
        GameManager.Inst().UiManager.MainUI.Center.Weapon.ResetData();
        GameManager.Inst().UiManager.MainUI.Center.Weapon.ResetInventory();
        GameManager.Inst().UiManager.MainUI.Center.Weapon.gameObject.SetActive(false);
        GameManager.Inst().UiManager.SetIsEquip(false);

        WeaponIcon[0].SetActive(true);
        WeaponIcon[1].SetActive(false);
        HomeBtn.SetActive(false);

        GameManager.Inst().IptManager.SetIsAbleControl(true);
        GameManager.Inst().IptManager.SetIsAbleSWControl(true);
    }

    public void OnClickInventoryBtn()
    {
        OnClickHomeBtn();

        GameManager.Inst().UiManager.MainUI.Center.Inventory.ShowInventory();

        HomeBtn.SetActive(true);

        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);
    }

    public void OnClickInventoryBackBtn()
    {
        GameManager.Inst().UiManager.MainUI.Center.Inventory.CloseInventory();
        GameManager.Inst().UiManager.MainUI.Center.Inventory.gameObject.SetActive(false);

        HomeBtn.SetActive(false);

        GameManager.Inst().IptManager.SetIsAbleControl(true);
        GameManager.Inst().IptManager.SetIsAbleSWControl(true);
    }

    public void OnClickSynthesisBtn()
    {
        OnClickHomeBtn();

        GameManager.Inst().UiManager.MainUI.Center.Inventory.InventoryDetail.gameObject.SetActive(false);
        GameManager.Inst().UiManager.MainUI.Center.Inventory.gameObject.SetActive(false);

        SynthesisIcon[0].SetActive(false);
        SynthesisIcon[1].SetActive(true);
        HomeBtn.SetActive(true);

        GameManager.Inst().UiManager.MainUI.Center.Synthesis.gameObject.SetActive(true);
        GameManager.Inst().UiManager.MainUI.Center.Synthesis.ShowInventory();

        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);
    }

    public void OnClickSynthesisBackBtn()
    {
        GameManager.Inst().UiManager.MainUI.Center.Synthesis.ResetSprites();
        GameManager.Inst().UiManager.MainUI.Center.Synthesis.gameObject.SetActive(false);

        SynthesisIcon[0].SetActive(true);
        SynthesisIcon[1].SetActive(false);
        HomeBtn.SetActive(false);

        GameManager.Inst().IptManager.SetIsAbleControl(true);
        GameManager.Inst().IptManager.SetIsAbleSWControl(true);
    }

    public void OnClickShopBtn()
    {
        OnClickHomeBtn();

        GameManager.Inst().UiManager.MainUI.Center.Shop.gameObject.SetActive(true);
        //GameManager.Inst().UiManager.MainUI.Center.Shop.OnSelectToggle(0);

        ShopIcon[0].SetActive(false);
        ShopIcon[1].SetActive(true);
        HomeBtn.SetActive(true);

        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);
    }

    public void OnClickShopBackBtn()
    {
        GameManager.Inst().UiManager.MainUI.Center.Shop.gameObject.SetActive(false);

        ShopIcon[0].SetActive(true);
        ShopIcon[1].SetActive(false);
        HomeBtn.SetActive(false);

        GameManager.Inst().IptManager.SetIsAbleControl(true);
        GameManager.Inst().IptManager.SetIsAbleSWControl(true);
    }

    public void OnClickHomeBtn()
    {
        if (GameManager.Inst().UiManager.MainUI.Center.StageScroll.gameObject.activeSelf)
            OnClickSpaceBackBtn();
        if (GameManager.Inst().UiManager.MainUI.Center.Weapon.gameObject.activeSelf)
            OnClickWeaponBackBtn();
        if (GameManager.Inst().UiManager.MainUI.Center.Inventory.gameObject.activeSelf)
            OnClickInventoryBackBtn();
        if (GameManager.Inst().UiManager.MainUI.Center.Synthesis.gameObject.activeSelf)
            OnClickSynthesisBackBtn();
        if (GameManager.Inst().UiManager.MainUI.Center.Shop.gameObject.activeSelf)
            OnClickShopBackBtn();
        if (GameManager.Inst().UiManager.MainUI.Center.Cheat.gameObject.activeSelf)
            GameManager.Inst().UiManager.MainUI.OnClickCheatBackBtn();
        if (GameManager.Inst().UiManager.MainUI.SideMenu.IsOpen)
            GameManager.Inst().UiManager.MainUI.SideMenu.OnClickSideBarBackBtn();
        if (GameManager.Inst().UiManager.MainUI.Bottom.WeaponScroll.IsOpen)
            GameManager.Inst().UiManager.MainUI.Bottom.OnClickManageCancel();
        if(GameManager.Inst().UiManager.InventoryScroll.GetComponent<InventoryScroll>().Contents.activeSelf)
            GameManager.Inst().UiManager.InventoryScroll.GetComponent<InventoryScroll>().Contents.SetActive(false);

        if (SceneManager.GetActiveScene().name == "Stage0" &&
            (GameManager.Inst().Tutorials.Step == 19 || GameManager.Inst().Tutorials.Step == 21 || GameManager.Inst().Tutorials.Step == 24))
            GameManager.Inst().Tutorials.Step++;
    }
}
