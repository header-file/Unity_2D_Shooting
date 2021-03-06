using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public enum NewWindowType
    {
        WEAPON = 0,
        DETAIL = 1,
        BUYSUBWEAPON = 2,
        INFO = 3
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
    public GameObject[] Colors;
    public GameObject ScrollView;
    public GameObject Inventory;
    public GameObject InventoryDetail;
    public GameObject Equip;
    public GameObject ConfirmSwitch;
    public GameObject Synthesis;
    public GameObject Resource;
    public GameObject SideMenu;
    public GameObject StageScroll;
    public GameObject InfoArea;
    public GameObject EquipArea;
    public GameObject ZzinBottom;

    //새 윈도우
    public GameObject[] NewWindows;

    public Sprite[] WeaponImages;

    MainUI MainUi;
    //Detail DetailUI;
    Slot[] SlotUI;
    BuySubWeapon BuySWUI;
    LoopScroll ScrollViewUI;
    Inventory InventoryUI;
    InventoryDetail InvDetailUI;
    Equip EquipUI;
    Synthesis SynthesisUI;
    SideMenu SideMenuUI;
    StageLoop StageScrollUI;
    InfoArea InfoAreaUI;
    ZzinBottom ZzinBottomUI;

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
    bool IsEquip;


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
        CurrentBulletType = 0;
        IsEquip = false;

        MainUi = MainUI.GetComponent<MainUI>();
        //DetailUI = NewWindows[(int)NewWindowType.DETAIL].GetComponent<Detail>();
        SlotUI = new Slot[Bullet.MAXBULLETS];
        for (int i = 0; i < Bullet.MAXBULLETS; i++)
            SlotUI[i] = Slots[i].GetComponent<Slot>();
        BuySWUI = NewWindows[(int)NewWindowType.BUYSUBWEAPON].GetComponent<BuySubWeapon>();
        ScrollViewUI = ScrollView.GetComponent<LoopScroll>();
        InventoryUI = Inventory.GetComponent<Inventory>();
        InvDetailUI = InventoryDetail.GetComponent<InventoryDetail>();
        EquipUI = Equip.GetComponent<Equip>();
        SynthesisUI = Synthesis.GetComponent<Synthesis>();
        SideMenuUI = SideMenu.GetComponent<SideMenu>();
        StageScrollUI = StageScroll.GetComponent<StageLoop>();
        InfoAreaUI = InfoArea.GetComponent<InfoArea>();
        ZzinBottomUI = ZzinBottom.GetComponent<ZzinBottom>();
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

            if(!IsEquip)
            {
                GameManager.Inst().IptManager.SetIsAbleControl(true);
                GameManager.Inst().IptManager.SetIsAbleSWControl(true);
            }

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

        //SetSlotDetail();
    }

    public void ShowEquipBtn(int curBulletType)
    {
        //UI
        for (int i = 0; i < Bullet.MAXBULLETS; i++)
        {
            //SlotUI[i].Selected.SetActive(false);
            SlotUI[i].DetailBtn.gameObject.SetActive(false);
        }

        //SlotUI[CurrentBulletType].Selected.SetActive(true);
        SlotUI[curBulletType].DetailBtn.gameObject.SetActive(true);
    }

    public void SetSlotsActive(int index, bool isActive)
    {
        SlotUI[index].Locked.SetActive(!isActive);
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

        NewWindows[(int)NewWindowType.WEAPON].SetActive(false);
        NewWindows[(int)NewWindowType.DETAIL].SetActive(false);
        NewWindows[(int)NewWindowType.BUYSUBWEAPON].SetActive(false);
        NewWindows[(int)NewWindowType.INFO].SetActive(true);

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

        NewWindows[(int)NewWindowType.INFO].GetComponent<Info>().ShowInfo(Type, CurrentBulletType);
    }

    //public void ShowDetail(int Type)
    //{
    //    NewWindows[(int)NewWindowType.WEAPON].SetActive(false);
    //    NewWindows[(int)NewWindowType.DETAIL].SetActive(true);
    //    DetailUI.SetBulletType(Type);
    //    DetailUI.SetDetails();
    //}
    public void ShowInfoArea(int Type)
    {
        InfoAreaUI.ShowDetail(Type);
    }

    public void InfoAreaTrigger(string trigger)
    {
        InfoAreaUI.SetAnimTrigger(trigger);
    }

    void SetBulletSelected()
    {
        for (int i = 0; i < Bullet.MAXBULLETS; i++)
            SlotUI[i].Selected.SetActive(false);

        SlotUI[CurrentBulletType].Selected.SetActive(true);
    }

    public void UnlockStage(int index)
    {
        StageScrollUI.Planets[index].Lock.SetActive(false);
    }

    public GameObject GetSideMenuSlot(int i)
    {
        return SideMenuUI.Slots[i].gameObject;
    }


    public void OnClickManageCancel()
    {
        if (!IsMoveDown)
            Timer = 0.0f;
        IsMoveDown = true;

        for (int i = 0; i < 5; i++)
            MainUi.Arrows.transform.GetChild(i).gameObject.SetActive(false);
    }

    public void OnClickToWeapon()
    {
        NewWindows[(int)NewWindowType.INFO].SetActive(false);
        NewWindows[(int)NewWindowType.WEAPON].SetActive(true);

        for (int i = 0; i < Bullet.MAXBULLETS; i++)
            SlotUI[i].Show(i);

        ShowEquipBtn(CurrentBulletType);
        SetBulletSelected();

        ScrollViewUI.MoveToSelected(CurrentBulletType);
    }

    public void OnClickDetailCancel()
    {
        NewWindows[(int)NewWindowType.DETAIL].SetActive(false);
        OnClickManageBtn(CurrentWeapon);
    }

    public void OnClickBulletUpgradeBtn()
    {
        //DetailUI.OnClickUpgradeBtn();
        GameManager.Inst().UpgManager.AddLevel(CurrentBulletType);
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
        NewWindows[(int)NewWindowType.INFO].SetActive(false);

        for (int i = 0; i < 5; i++)
            MainUi.Arrows.transform.GetChild(i).gameObject.SetActive(false);

        if (index > 1)
            index++;
        MainUi.Arrows.transform.GetChild(index).gameObject.SetActive(true);

        CurrentWeapon = index;
    }

    public void OnClickBuySWBtn()
    {
        BuySWUI.Buy();

        OnClickManageBtn(CurrentWeapon);
    }

    public void OnClickUpgradeSWBtn()
    {
        NewWindows[(int)NewWindowType.INFO].GetComponent<Info>().Buy();
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
        GameManager.Inst().ShtManager.SetColorSelection(CurrentWeapon, index);
        NewWindows[(int)NewWindowType.INFO].GetComponent<Info>().SetColorSelected(index);

        if (CurrentWeapon == 2)
            GameManager.Inst().Player.SetSkinColor(index);
        else
        {
            int idx = CurrentWeapon;
            if (idx > 1)
                idx--;
            GameManager.Inst().GetSubweapons(idx).SetSkinColor(index);
        }
    }

    public void OnClickInventoryBtn()
    {
        InventoryUI.ShowInventory();

        ZzinBottomUI.InventoryIcon[0].SetActive(false);
        ZzinBottomUI.InventoryIcon[1].SetActive(true);
        ZzinBottomUI.HomeIcon.alpha = 1.0f;

        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);
    }

    public void OnClickInventoryBackBtn()
    {
        InventoryUI.CloseInventory();
        Inventory.SetActive(false);

        ZzinBottomUI.InventoryIcon[0].SetActive(true);
        ZzinBottomUI.InventoryIcon[1].SetActive(false);
        ZzinBottomUI.HomeIcon.alpha = 0.0f;

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

    //public void OnClickEquipBtn()
    //{
    //    if (!IsMoveDown)
    //    {
    //        Timer = 0.0f;
    //        IsMoveDown = true;
    //    }

    //    Inventory.SetActive(false);
    //    InventoryDetail.SetActive(false);
    //    Equip.SetActive(true);

    //    EquipUI.SetCurBulletType(CurrentBulletType);
    //    EquipUI.ShowUI();
    //    IsEquip = true;
    //}

    public void OnClickEquipBtn()
    {
        Inventory.SetActive(false);
        InventoryDetail.SetActive(false);
        Equip.SetActive(true);

        EquipUI.SetCurBulletType(GameManager.Inst().Player.GetBulletType());
        EquipUI.ShowUI();
        IsEquip = true;

        InfoArea.SetActive(true);
        EquipArea.SetActive(false);
        InfoAreaUI.ShowDetail(CurrentBulletType);

        ZzinBottomUI.WeaponIcon[0].SetActive(false);
        ZzinBottomUI.WeaponIcon[1].SetActive(true);
        ZzinBottomUI.HomeIcon.alpha = 1.0f;

        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);
    }

    public void OnClickEquipBackBtn()
    {
        EquipUI.DisableSelectedSlot();
        EquipUI.ResetEquip();
        EquipUI.SetIsShowingSwitch(false);
        ConfirmSwitch.SetActive(false);
        Equip.SetActive(false);
        IsEquip = false;

        ZzinBottomUI.WeaponIcon[0].SetActive(true);
        ZzinBottomUI.WeaponIcon[1].SetActive(false);
        ZzinBottomUI.HomeIcon.alpha = 0.0f;

        GameManager.Inst().IptManager.SetIsAbleControl(true);
        GameManager.Inst().IptManager.SetIsAbleSWControl(true);
    }

    public void OnClickEquipAreaBtn()
    {
        InfoArea.SetActive(false);
        EquipArea.SetActive(true);
    }

    public void OnClickEquipSlotBtn(int index)
    {
        EquipUI.SortAsType(index);
    }

    public void OnClickEquipSelectBtn(int index)
    {
        if (EquipUI.GetSelected(EquipUI.GetCurBulletType()) == -1 &&
            !EquipUI.CheckAlreadyEquip(index))
            EquipUI.Select(index, EquipUI.GetCurBulletType());            
        else
        {
            if (EquipUI.GetSelected(EquipUI.GetCurBulletType()) == index)
                return;

            EquipUI.ShowSwitch(index);
            ConfirmSwitch.SetActive(true);
            EquipUI.SetSwichBuffer(index);
        }
    }

    public void OnClickUnequipBtn()
    {
        EquipUI.Unequip(EquipUI.GetCurBulletType());
    }

    public void OnClickSwitchBtn()
    {
        EquipUI.Switch(EquipUI.GetSwitchBuffer(), EquipUI.GetCurBulletType());
        ConfirmSwitch.SetActive(false);
    }

    public void OnClickSwitchCancelBtn()
    {
        EquipUI.SetSwichBuffer(-1);
        EquipUI.SwitchCancel();
        ConfirmSwitch.SetActive(false);
    }

    public void OnClickNextButton(bool IsNext)
    {
        EquipUI.Next(IsNext);
    }

    public void OnClickSynthesisBtn()
    {
        InventoryDetail.SetActive(false);
        Inventory.SetActive(false);

        Synthesis.SetActive(true);
        SynthesisUI.ShowInventory();

        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);
    }

    public void OnClickSynthesisBackBtn()
    {
        SynthesisUI.ResetSprites();
        Synthesis.SetActive(false);

        GameManager.Inst().IptManager.SetIsAbleControl(true);
        GameManager.Inst().IptManager.SetIsAbleSWControl(true);
    }

    public void OnClickSynthesisSlotBtn(int index)
    {
        SynthesisUI.SetCurrentIndex(index);

        if (index == 0)
            SynthesisUI.ShowInventory();
        else if (index == 1)
            SynthesisUI.SortAsGrade(SynthesisUI.GetGrade());
        else if (index == 3)
            SynthesisUI.ShowConfirmWindow();
    }

    public void OnClickSynthesisConfirmBtn()
    {
        SynthesisUI.Synthesize();
    }

    public void OnClickSynthesisConfirmBackBtn()
    {
        SynthesisUI.CancelConfirm();
    }

    public void OnClickSynthesisSelectBtn(int index)
    {
        if (EquipUI.CheckAlreadyEquipAll(index))
            SynthesisUI.ShowUnEquipConfirm(index);
        else
            SynthesisUI.SetButtons(index);
    }

    public void OnClickSynthesisResultBackBtn()
    {
        SynthesisUI.CloseResult();
    }

    public void OnClickSelectDetailBackBtn()
    {
        SynthesisUI.CloseDetail();
    }

    public void OnClickSynthesisUnequipBtn()
    {
        int index = SynthesisUI.GetUnequipIndex();
        int bulletType = EquipUI.GetBulletType(index);
        int equipType = GameManager.Inst().Player.GetItem(index).Type;

        EquipUI.Unequip(bulletType, equipType);
        SynthesisUI.SetButtons(index);

        SynthesisUI.CloseUnequip();
    }

    public void OnClickUnequipConfirmBackBtn()
    {
        SynthesisUI.CloseUnequip();
    }

    public void OnClickResourceToggleBtn()
    {
        if (Resource.activeSelf)
            Resource.SetActive(false);
        else
            Resource.SetActive(true);
    }

    public void OnClickSideBarBtn()
    {
        SideMenuUI.SideMenuOpen();

        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);
    }

    public void OnClickSideBarBackBtn()
    {
        SideMenuUI.SideMenuClose();

        GameManager.Inst().IptManager.SetIsAbleControl(true);
        GameManager.Inst().IptManager.SetIsAbleSWControl(true);
    }
   
    public void OnClickSpaceBtn()
    {
        StageScroll.SetActive(true);
        StageScrollUI.Show();

        ZzinBottomUI.UniverseIcon[0].SetActive(false);
        ZzinBottomUI.UniverseIcon[1].SetActive(true);
        ZzinBottomUI.HomeIcon.alpha = 1.0f;

        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);
    }

    public void OnClickSpaceBackBtn()
    {
        StageScroll.SetActive(false);

        ZzinBottomUI.UniverseIcon[0].SetActive(true);
        ZzinBottomUI.UniverseIcon[1].SetActive(false);
        ZzinBottomUI.HomeIcon.alpha = 0.0f;

        GameManager.Inst().IptManager.SetIsAbleControl(true);
        GameManager.Inst().IptManager.SetIsAbleSWControl(true);
    }

    public void OnClickBulletEquipBtn()
    {
        int num = ScrollViewUI.OnClickEquipBtn();
        if (num == -1)
            return;

        CurrentBulletType = num;

        SetBulletSelected();
    }

    public void OnClickHomeBtn()
    {
        if (StageScroll.activeSelf)
            OnClickSpaceBackBtn();
        if (Equip.activeSelf)
            OnClickEquipBackBtn();
        if (Inventory.activeSelf)
            OnClickInventoryBackBtn();
    }
}
