﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public enum NewWindowType
    {
        WEAPON = 0,
        DETAIL = 1,
        BUYSUBWEAPON = 2
    };

    //움직임용
    public GameObject Player;
    public GameObject SubWeapon;
    public GameObject Turret;
    public GameObject Background;
    public GameObject Panel;

    //기능 구현용
    public GameObject MainUI;
    public GameObject[] Slots;
    public GameObject Color;
    public GameObject ScrollView;
    public GameObject Inventory;
    public GameObject InventoryDetail;
    public GameObject Equip;
    public GameObject ConfirmSwitch;

    //새 윈도우
    public GameObject[] NewWindows;

    MainUI MainUi;
    Detail DetailUI;
    Slot[] SlotUI;
    BuySubWeapon BuySWUI;
    LoopScroll ScrollViewUI;
    Inventory InventoryUI;
    InventoryDetail InvDetailUI;
    Equip EquipUI;

    Vector3 PlayerPosOrigin;
    Vector3 SubWeaponPosOrigin;
    Vector3 TurretPosOrigin;
    Vector3 BackgroundPosOrigin;
    Vector3 PanelPosOrigin;

    Vector3 PlayerPosUI;
    Vector3 SubWeaponPosUI;
    Vector3 TurretPosUI;
    Vector3 BackgroundPosUI;
    Vector3 PanelPosUI;

    bool IsMoveUp;
    bool IsMoveDown;
    float Timer;
    float TickCount;
    int CurrentBulletType;
    int CurrentWeapon;


    public BuySubWeapon GetBuySWUI() { return BuySWUI; }

    public void SetIsMoveUp(bool b) { IsMoveUp = b; }

    void Awake()
    {
        PlayerPosOrigin = Player.transform.position;
        SubWeaponPosOrigin = SubWeapon.transform.position;
        TurretPosOrigin = Turret.transform.position;
        BackgroundPosOrigin = Background.transform.position;
        PanelPosOrigin = Panel.transform.position;

        PlayerPosUI = new Vector3(0.0f, 3.3f, 0.0f);
        SubWeaponPosUI = new Vector3(0.0f, PlayerPosUI.y - 0.24f, 0.0f);
        TurretPosUI = new Vector3(0.0f, 6.15f, 90.0f);
        BackgroundPosUI = new Vector3(0.0f, PlayerPosUI.y + 3.3f, 0.0f);
        PanelPosUI = new Vector3(0.0f, 2.0f, 90.0f);

        Timer = 0.0f;
        TickCount = 1.0f / 12.0f;

        IsMoveUp = false;
        IsMoveDown = false;
        CurrentBulletType = -1;

        MainUi = MainUI.GetComponent<MainUI>();
        DetailUI = NewWindows[(int)NewWindowType.DETAIL].GetComponent<Detail>();
        SlotUI = new Slot[Bullet.MAXBULLETS];
        for (int i = 0; i < Bullet.MAXBULLETS; i++)
            SlotUI[i] = Slots[i].GetComponent<Slot>();
        BuySWUI = NewWindows[(int)NewWindowType.BUYSUBWEAPON].GetComponent<BuySubWeapon>();
        ScrollViewUI = ScrollView.GetComponent<LoopScroll>();
        InventoryUI = Inventory.GetComponent<Inventory>();
        InvDetailUI = InventoryDetail.GetComponent<InventoryDetail>();
        EquipUI = Equip.GetComponent<Equip>();
    }

    void Update()
    {
        if (IsMoveUp)
            MoveUp();
        if (IsMoveDown)
            MoveDown();
    }

    //UI Interact
    void MoveUp()
    {
        Player.transform.position = Vector3.MoveTowards(Player.transform.position, PlayerPosUI, Timer);
        SubWeapon.transform.position = Vector3.MoveTowards(SubWeapon.transform.position, SubWeaponPosUI, Timer);
        Turret.transform.position = Vector3.MoveTowards(Turret.transform.position, TurretPosUI, Timer);
        Background.transform.position = Vector3.MoveTowards(Background.transform.position, BackgroundPosUI, Timer);
        Panel.transform.position = Vector3.MoveTowards(Panel.transform.position, PanelPosUI, Timer);

        Timer += TickCount;

        if (Timer >= 1.0f)
        {
            IsMoveUp = false;
            GameManager.Inst().IptManager.SetIsAbleControl(false);
            GameManager.Inst().IptManager.SetIsAbleSWControl(false);
            
        }  
    }

    void MoveDown()
    {
        Player.transform.position = Vector3.MoveTowards(Player.transform.position, PlayerPosOrigin, Timer);
        SubWeapon.transform.position = Vector3.MoveTowards(SubWeapon.transform.position, SubWeaponPosOrigin, Timer);
        Turret.transform.position = Vector3.MoveTowards(Turret.transform.position, TurretPosOrigin, Timer);
        Background.transform.position = Vector3.MoveTowards(Background.transform.position, BackgroundPosOrigin, Timer);
        Panel.transform.position = Vector3.MoveTowards(Panel.transform.position, PanelPosOrigin, Timer);

        Timer += TickCount;

        if (Timer >= 1.0f)
        {
            IsMoveDown = false;

            for(int i = 0; i < NewWindows.Length; i++)
                NewWindows[i].SetActive(false);

            GameManager.Inst().IptManager.SetIsAbleControl(true);
            GameManager.Inst().IptManager.SetIsAbleSWControl(true);

            Time.timeScale = 1.0f;
        } 
    }

    public void SetSubWeaponInteratable(bool b)
    {
        Turret.transform.GetChild(BuySWUI.GetSelectedIndex()).GetChild(0).gameObject.GetComponent<Button>().interactable = b;
        Turret.transform.GetChild(BuySWUI.GetSelectedIndex()).GetChild(0).gameObject.SetActive(false);
    }

    public void SelectBullet(int BulletType)
    {
        switch(CurrentWeapon)
        {
            case 2:
                GameManager.Inst().Player.SetBulletType(BulletType);
                break;

            case 0:
            case 1:
            case 3:
            case 4:
                if (CurrentWeapon > 1)
                    GameManager.Inst().GetSubweapons(CurrentWeapon - 1).SetBulletType(BulletType);
                else
                    GameManager.Inst().GetSubweapons(CurrentWeapon).SetBulletType(BulletType);
                break;
        }

        CurrentBulletType = BulletType;

        SetSlotDetail();
    }

    void SetSlotDetail()
    {
        //UI
        for (int i = 0; i < Bullet.MAXBULLETS; i++)
        {
            Slots[i].transform.GetChild(1).gameObject.SetActive(false);
            Slots[i].transform.GetChild(4).gameObject.SetActive(false);
        }

        Slots[CurrentBulletType].transform.GetChild(1).gameObject.SetActive(true);
        Slots[CurrentBulletType].transform.GetChild(4).gameObject.SetActive(true);
    }


    //Button Interact
    public void OnClickUpgradeBtn()
    {
        MainUi.OnClickUpgradeBtn();
    }

    public void OnClickManageBtn(int Type)
    {
        //Time.timeScale = 0.0f;
        GameManager.Inst().IptManager.SetIsAbleControl(false);

        CurrentWeapon = Type;
        ScrollViewUI.SetCurrentCharacter(CurrentWeapon);

        if (!IsMoveUp)
            Timer = 0.0f;
        IsMoveUp = true;

        NewWindows[(int)NewWindowType.WEAPON].SetActive(true);
        NewWindows[(int)NewWindowType.DETAIL].SetActive(false);
        NewWindows[(int)NewWindowType.BUYSUBWEAPON].SetActive(false);
        
        for (int i = 0; i < 5; i++)
            MainUi.Arrows.transform.GetChild(i).gameObject.SetActive(false);
        
        MainUi.Arrows.transform.GetChild(Type).gameObject.SetActive(true);

        switch(CurrentWeapon)
        {
            case 2:
                CurrentBulletType = GameManager.Inst().Player.GetBulletType();
                break;

            case 0:
            case 1:
            case 3:
            case 4:
                if (CurrentWeapon > 1)
                    CurrentBulletType = GameManager.Inst().GetSubweapons(CurrentWeapon - 1).GetBulletType();
                else
                    CurrentBulletType = GameManager.Inst().GetSubweapons(CurrentWeapon).GetBulletType();
                break;
        }

        SetSlotDetail();

        ScrollViewUI.MoveToSelected(CurrentBulletType);
    }

    public void OnClickManageCancel()
    {
        if (!IsMoveDown)
            Timer = 0.0f;
        IsMoveDown = true;

        for (int i = 0; i < 5; i++)
            MainUi.Arrows.transform.GetChild(i).gameObject.SetActive(false);
    }

    public void ShowDetail(int Type)
    {
        NewWindows[(int)NewWindowType.WEAPON].SetActive(false);
        NewWindows[(int)NewWindowType.DETAIL].SetActive(true);
        DetailUI.SetBulletType(Type);
        DetailUI.SetDetails();
    }

    public void OnClickDetailCancel()
    {
        NewWindows[(int)NewWindowType.DETAIL].SetActive(false);
        OnClickManageBtn(CurrentWeapon);
    }

    public void OnClickBulletUpgradeBtn()
    {
        DetailUI.OnClickUpgradeBtn();
    }

    public void OnClickSubWeapon(int index)
    {
        BuySWUI.ShowBuy(index);
        if (!IsMoveUp)
            Timer = 0.0f;
        IsMoveUp = true;

        NewWindows[(int)NewWindowType.WEAPON].SetActive(false);
        NewWindows[(int)NewWindowType.DETAIL].SetActive(false);
        NewWindows[(int)NewWindowType.BUYSUBWEAPON].SetActive(true);

        for (int i = 0; i < 5; i++)
            MainUi.Arrows.transform.GetChild(i).gameObject.SetActive(false);

        if (index > 1)
            index++;
        MainUi.Arrows.transform.GetChild(index).gameObject.SetActive(true);
    }

    public void OnClickBuyBtn()
    {
        BuySWUI.Buy();
    }

    public void OnClickSubWeaponCancel()
    {
        if (!IsMoveDown)
            Timer = 0.0f;
        IsMoveDown = true;

        for(int i = 0; i < 5; i++)
            MainUi.Arrows.transform.GetChild(i).gameObject.SetActive(false);
    }

    public void OnClickColorBtn(int index)
    {
        GameManager.Inst().SetColorSelection(CurrentWeapon, index);
    }

    public void OnClickInventoryBtn()
    {
        InventoryUI.ShowInventory();

        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);
    }

    public void OnClickInventoryBackBtn()
    {
        Inventory.SetActive(false);

        GameManager.Inst().IptManager.SetIsAbleControl(true);
        GameManager.Inst().IptManager.SetIsAbleSWControl(true);
    }

    public void OnClickInventoryDetailBtn(int index)
    {
        InventoryDetail.SetActive(true);

        InvDetailUI.ShowDetail(index);
    }

    public void OnClickInventoryDetailBackBtn()
    {
        InventoryDetail.SetActive(false);
    }

    public void OnClickEquipBtn()
    {
        Inventory.SetActive(false);
        InventoryDetail.SetActive(false);
        Equip.SetActive(true);
        
        EquipUI.Show(CurrentBulletType);
    }

    public void OnClickEquipBackBtn()
    {
        EquipUI.ResetMaterial();
        EquipUI.SetIsShowingSwitch(false);
        ConfirmSwitch.SetActive(false);
        Equip.SetActive(false);

        GameManager.Inst().IptManager.SetIsAbleControl(true);
        GameManager.Inst().IptManager.SetIsAbleSWControl(true);
    }

    public void OnClickEquipSlotBtn(int index)
    {
        EquipUI.SortAsType(index);
    }

    public void OnClickEquipSelectBtn(int index)
    {
        if (EquipUI.GetSelected(CurrentBulletType) == -1)
            EquipUI.Select(index, CurrentBulletType);
        else
        {
            if (EquipUI.GetSelected(CurrentBulletType) == EquipUI.GetIndices(index))
                return;

            EquipUI.ShowSwitch(index, CurrentBulletType);
            ConfirmSwitch.SetActive(true);
            EquipUI.SetSwichBuffer(index);
        }
    }

    public void OnClickUnequipBtn()
    {
        EquipUI.Unequip(CurrentBulletType);
    }

    public void OnClickSwitchBtn()
    {
        EquipUI.Switch(EquipUI.GetSwitchBuffer(), CurrentBulletType);
        ConfirmSwitch.SetActive(false);
    }

    public void OnClickSwitchCancelBtn()
    {
        EquipUI.SetSwichBuffer(-1);
        EquipUI.SwitchCancel();
        ConfirmSwitch.SetActive(false);
    }
}
