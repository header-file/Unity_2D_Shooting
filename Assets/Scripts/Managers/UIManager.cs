using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public GameObject Weapon;
    public GameObject ConfirmSwitch;
    public GameObject Synthesis;
    public GameObject Resource;
    public GameObject SideMenu;
    public GameObject StageScroll;
    public GameObject InfoArea;
    public GameObject EquipArea;
    public GameObject ZzinBottom;
    public GameObject Shop;
    public GameObject Cheat;

    public Weapon WeaponUI;
    public Shop ShopUI;
    public ReviveAlert ReviveAlertUI;
    public DailyJewel DailyJewelUI;
    public DailyLeft DailyLeft;
    public DailyLeft DailyPlusLeft;
    public Alarm Alarm;
    public Tutorial Tutorial;

    //기타 UI
    public Text CoinText;
    public Turret[] Turrets;
    public GameObject RedMask;
    public PlayerHitArea[] PlayerHitAreas;
    public GameObject InventoryScroll;
    public GameObject[] SubPositions;
    public Text JewelText;
    public Animator BgAnim;

    //플레이어용 UI
    public GameObject PlayerUI;
    public Image PlayerTimerImg;
    public Text PlayerTimerText;
    public GameObject PlayerHPUI;
    public Image PlayerHPBar;
    public GameObject PlayerEquipUI;
    public Image PlayerEquipBar;
    public Image PlayerEquipIcon;

    //보스용 UI
    public GameObject BossHPBarCanvas;
    public Image BossHPBar;
    public Text BossHPBarText;
    public GameObject BossGauge;
    public Image BossGaugeBar;
    public Animator WarningAnim;
    public GameObject TurretUI;

    //새 윈도우
    public GameObject[] NewWindows;

    //UI용 이미지
    public Sprite[] WeaponImages;
    public Sprite[] FoodImages;
    public Sprite[] EquipImages;


    MainUI MainUi;
    Slot[] SlotUI;
    BuySubWeapon BuySWUI;
    LoopScroll ScrollViewUI;
    Inventory InventoryUI;
    InventoryDetail InvDetailUI;
    Synthesis SynthesisUI;
    SideMenu SideMenuUI;
    StageLoop StageScrollUI;
    InfoArea InfoAreaUI;
    ZzinBottom ZzinBottomUI;
    Cheat CheatUI;

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

    public void SetCoinText(int coin) { CoinText.text = coin.ToString(); }
    public void SetHitAreas(GameObject sub, int index) { PlayerHitAreas[index].Object = sub; }
    public void SetIsMoveUp(bool b) { IsMoveUp = b; }

    void Awake()
    {
        GameManager.Inst().UiManager = gameObject.GetComponent<UIManager>();

        PlayerPosOrigin = GameManager.Inst().Player.transform.position;
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
        SlotUI = new Slot[Constants.MAXBULLETS];
        for (int i = 0; i < Constants.MAXBULLETS; i++)
            SlotUI[i] = Slots[i].GetComponent<Slot>();
        BuySWUI = NewWindows[(int)NewWindowType.BUYSUBWEAPON].GetComponent<BuySubWeapon>();
        ScrollViewUI = ScrollView.GetComponent<LoopScroll>();
        InventoryUI = Inventory.GetComponent<Inventory>();
        InvDetailUI = InventoryDetail.GetComponent<InventoryDetail>();
        //EquipUI = Equip.GetComponent<Equip>();
        WeaponUI = Weapon.GetComponent<Weapon>();
        SynthesisUI = Synthesis.GetComponent<Synthesis>();
        SideMenuUI = SideMenu.GetComponent<SideMenu>();
        StageScrollUI = StageScroll.GetComponent<StageLoop>();
        InfoAreaUI = InfoArea.GetComponent<InfoArea>();
        ZzinBottomUI = ZzinBottom.GetComponent<ZzinBottom>();
        ShopUI = Shop.GetComponent<Shop>();
        CheatUI = Cheat.GetComponent<Cheat>();

        //Player UI Setting
        GameManager.Inst().Player.UI = PlayerUI;
        GameManager.Inst().Player.TimerImage = PlayerTimerImg;
        GameManager.Inst().Player.TimerText = PlayerTimerText;
        GameManager.Inst().Player.HPUI = PlayerHPUI;
        GameManager.Inst().Player.HPBar = PlayerHPBar;
        GameManager.Inst().Player.EquipUI = PlayerEquipUI;
        GameManager.Inst().Player.EquipBar = PlayerEquipBar;
        GameManager.Inst().Player.EquipIcon = PlayerEquipIcon;
    }

    void Start()
    {
        PlayerHitAreas[4].Object = GameManager.Inst().Player.gameObject;

        GameManager.Inst().Player.AddCoin(0);
        GameManager.Inst().AddJewel(0);
        for (int i = 1; i <= Constants.MAXSTAGES; i++)
            GameManager.Inst().AddResource(i, 0);
        GameManager.Inst().DatManager.GameData.LoadSubWeapon();
        GameManager.Inst().DatManager.GameData.LoadDaily();
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
        GameManager.Inst().Player.transform.position = Vector3.MoveTowards(GameManager.Inst().Player.transform.position, PlayerPosUI, Timer);
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
        GameManager.Inst().Player.transform.position = Vector3.MoveTowards(GameManager.Inst().Player.transform.position, PlayerPosOrigin, Timer);
        SubWeapon.transform.position = Vector3.MoveTowards(SubWeapon.transform.position, SubWeaponPosOrigin, Timer);
        Turret.transform.position = Vector3.MoveTowards(Turret.transform.position, TurretPosOrigin, Timer);
        Background.transform.position = Vector3.MoveTowards(Background.transform.position, BackgroundPosOrigin, Timer);
        Panel.transform.position = Vector3.MoveTowards(Panel.transform.position, PanelPosOrigin, Timer);

        Timer += TickCount;

        if (Timer >= 1.0f)
        {
            IsMoveDown = false;

            for (int i = 0; i < NewWindows.Length; i++)
                NewWindows[i].SetActive(false);

            if (!IsEquip)
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

    public void SetSubWeaponInteratable(int index, bool b)
    {
        Turret.transform.GetChild(index).GetChild(0).gameObject.GetComponent<Button>().interactable = b;
        Turret.transform.GetChild(index).GetChild(0).gameObject.SetActive(false);
    }

    public void SelectBullet(int BulletType)
    {
        switch (CurrentWeapon)
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
        for (int i = 0; i < Constants.MAXBULLETS; i++)
        {
            //SlotUI[i].Selected.SetActive(false);
            SlotUI[i].DetailBtn.gameObject.SetActive(false);
        }

        //SlotUI[CurrentBulletType].Selected.SetActive(true);
        SlotUI[curBulletType].DetailBtn.gameObject.SetActive(true);
    }

    public void SetSlotsActive(int index, bool isActive, int unlockStage)
    {
        SlotUI[index].Locked.SetActive(!isActive);
        SlotUI[index].StageName.text = "Stage" + unlockStage.ToString();
    }


    //Button Interact
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
        NewWindows[(int)NewWindowType.INFO].GetComponent<Info>().SetColorSelected(GameManager.Inst().ShtManager.GetColorSelection(Type));
        //NewWindows[(int)NewWindowType.INFO].SetActive(false);

        for (int i = 0; i < 5; i++)
            MainUi.Arrows.transform.GetChild(i).gameObject.SetActive(false);

        MainUi.Arrows.transform.GetChild(Type).gameObject.SetActive(true);

        switch (CurrentWeapon)
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

        for (int i = 0; i < Constants.MAXBULLETS; i++)
            SlotUI[i].Show(i);

        ShowEquipBtn(CurrentBulletType);
        SetBulletSelected();

        ScrollViewUI.MoveToSelected(CurrentBulletType);

        GameManager.Inst().Player.ShowEquipUI();
        for (int i = 0; i < Constants.MAXSUBWEAPON; i++)
        {
            if (GameManager.Inst().GetSubweapons(i) != null)
                GameManager.Inst().GetSubweapons(i).ShowEquipUI();
        }

        if (SceneManager.GetActiveScene().name == "Stage0" && (GameManager.Inst().Tutorials.Step == 5 || GameManager.Inst().Tutorials.Step == 41))
            GameManager.Inst().Tutorials.Step++;
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
        for (int i = 0; i < Constants.MAXBULLETS; i++)
            SlotUI[i].Selected.SetActive(false);

        SlotUI[CurrentBulletType].Selected.SetActive(true);
    }

    public void UnlockStage(int index)
    {
        StageScrollUI.Planets[index].Lock.SetActive(false);

        if (GameManager.Inst().StgManager.ReachedStage > 1)
            SideMenuUI.MakeSlot();
    }

    public SideMenuSlot GetSideMenuSlot(int i)
    {
        return SideMenuUI.Slots[i];
    }

    public void ShowReviveAlert(int index)
    {
        ReviveAlertUI.gameObject.SetActive(true);

        ReviveAlertUI.Show(index);
    }


    //Click Interactions
    public void OnClickManageCancel()
    {
        if (!IsMoveDown)
            Timer = 0.0f;
        IsMoveDown = true;

        for (int i = 0; i < 5; i++)
            MainUi.Arrows.transform.GetChild(i).gameObject.SetActive(false);

        GameManager.Inst().Player.EquipUI.SetActive(false);
        for (int i = 0; i < Constants.MAXSUBWEAPON; i++)
        {
            if (GameManager.Inst().GetSubweapons(i) != null)
                GameManager.Inst().UiManager.Turrets[i].EquipUI.SetActive(false);
        }
    }

    public void OnClickToWeapon()
    {
        NewWindows[(int)NewWindowType.INFO].SetActive(false);
        NewWindows[(int)NewWindowType.WEAPON].SetActive(true);

        for (int i = 0; i < Constants.MAXBULLETS; i++)
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
        CurrentBulletType = WeaponUI.GetCurBulletType();
        GameManager.Inst().UpgManager.AddLevel(CurrentBulletType);

        if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 32)
            GameManager.Inst().Tutorials.Step++;
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

        if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 8)
            GameManager.Inst().Tutorials.Step++;
    }

    public void OnClickBuySWBtn()
    {
        BuySWUI.Buy();

        OnClickManageBtn(CurrentWeapon);

        if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 9)
            GameManager.Inst().Tutorials.Step++;
    }

    public void OnClickUpgradeSWBtn()
    {
        NewWindows[(int)NewWindowType.INFO].GetComponent<Info>().Buy(CurrentWeapon);
    }

    public void OnClickSubWeaponCancel()
    {
        if (!IsMoveDown)
            Timer = 0.0f;
        IsMoveDown = true;

        for (int i = 0; i < 5; i++)
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

        if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 7)
            GameManager.Inst().Tutorials.Step++;
    }

    public void OnClickInventoryBtn()
    {
        OnClickHomeBtn();

        InventoryUI.ShowInventory();

        //ZzinBottomUI.HomeIcon.alpha = 1.0f;
        ZzinBottomUI.HomeBtn.SetActive(true);

        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);
    }

    public void OnClickInventoryBackBtn()
    {
        InventoryUI.CloseInventory();
        Inventory.SetActive(false);

        //ZzinBottomUI.HomeIcon.alpha = 0.0f;
        ZzinBottomUI.HomeBtn.SetActive(false);

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

    public void OnClickWeaponBtn()
    {
        OnClickHomeBtn();

        Inventory.SetActive(false);
        InventoryDetail.SetActive(false);
        //Equip.SetActive(true);
        Weapon.SetActive(true);

        //EquipUI.SetCurBulletType(GameManager.Inst().GameManager.Inst().Player.GetBulletType());
        //EquipUI.ShowUI();
        CurrentBulletType = GameManager.Inst().Player.GetBulletType();
        WeaponUI.SetCurBulletType(CurrentBulletType);
        WeaponUI.ShowUI();
        IsEquip = true;

        InfoArea.SetActive(true);
        EquipArea.SetActive(false);

        ZzinBottomUI.WeaponIcon[0].SetActive(false);
        ZzinBottomUI.WeaponIcon[1].SetActive(true);
        //ZzinBottomUI.HomeIcon.alpha = 1.0f;
        ZzinBottomUI.HomeBtn.SetActive(true);

        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);
    }

    public void OnClickWeaponBackBtn()
    {
        //EquipUI.DisableSelectedSlot();
        //EquipUI.ResetEquip();
        //EquipUI.SetIsShowingSwitch(false);
        //ConfirmSwitch.SetActive(false);
        //Equip.SetActive(false);
        WeaponUI.ResetData();
        WeaponUI.ResetInventory();
        Weapon.SetActive(false);
        IsEquip = false;

        ZzinBottomUI.WeaponIcon[0].SetActive(true);
        ZzinBottomUI.WeaponIcon[1].SetActive(false);
        //ZzinBottomUI.HomeIcon.alpha = 0.0f;
        ZzinBottomUI.HomeBtn.SetActive(false);

        GameManager.Inst().IptManager.SetIsAbleControl(true);
        GameManager.Inst().IptManager.SetIsAbleSWControl(true);
    }

    public void OnClickEquipAreaBtn()
    {
        WeaponUI.ShowEquipArea();

        if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 34)
            GameManager.Inst().Tutorials.Step++;
    }

    public void OnClickWeaponTypeSortBtn(int index)
    {
        //EquipUI.SortAsType(index);
        WeaponUI.SortAsType(index);
    }

    public void OnClickEquipSelectBtn(int index)
    {
        //if (EquipUI.GetSelected(EquipUI.GetCurBulletType()) == -1 &&
        //    !EquipUI.CheckAlreadyEquip(index))
        //    EquipUI.Select(index, EquipUI.GetCurBulletType());            
        //else
        //{
        //    if (EquipUI.GetSelected(EquipUI.GetCurBulletType()) == index)
        //        return;

        //    EquipUI.ShowSwitch(index);
        //    ConfirmSwitch.SetActive(true);
        //    EquipUI.SetSwichBuffer(index);
        //}
        WeaponUI.Select(index, WeaponUI.GetCurBulletType());
    }

    public void OnClickUnequipBtn()
    {
        //EquipUI.Unequip(EquipUI.GetCurBulletType());
    }

    public void OnClickSwitchBtn()
    {
        WeaponUI.Switch();
    }

    public void OnClickSwitchCancelBtn()
    {
        WeaponUI.EquipSwitch.gameObject.SetActive(false);
    }

    public void OnClickNextButton(bool IsNext)
    {
        //EquipUI.Next(IsNext);
        WeaponUI.Next(IsNext);
    }

    public void OnClickSynthesisBtn()
    {
        OnClickHomeBtn();

        InventoryDetail.SetActive(false);
        Inventory.SetActive(false);

        //ZzinBottomUI.HomeIcon.alpha = 1.0f;
        ZzinBottomUI.SynthesisIcon[0].SetActive(false);
        ZzinBottomUI.SynthesisIcon[1].SetActive(true);
        ZzinBottomUI.HomeBtn.SetActive(true);

        Synthesis.SetActive(true);
        SynthesisUI.ShowInventory();

        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);
    }

    public void OnClickSynthesisBackBtn()
    {
        SynthesisUI.ResetSprites();
        Synthesis.SetActive(false);

        ZzinBottomUI.SynthesisIcon[0].SetActive(true);
        ZzinBottomUI.SynthesisIcon[1].SetActive(false);
        ZzinBottomUI.HomeBtn.SetActive(false);

        GameManager.Inst().IptManager.SetIsAbleControl(true);
        GameManager.Inst().IptManager.SetIsAbleSWControl(true);
    }

    public void OnClickSynthesisSlotBtn(int index)
    {
        //SynthesisUI.SetCurrentIndex(index);

        //int count = SynthesisUI.CheckInputTypes();
        //if (count == 0)
        //    SynthesisUI.DiscardMaxGrade();
        //else if (count == 1)
        //{
        //    if(index == SynthesisUI.GetLastIndex())
        //        SynthesisUI.DiscardMaxGrade();
        //    else
        //        SynthesisUI.SortAsGrade(SynthesisUI.GetGrade());
        //}
        //else if(count < 3)
        if (index < 3)
            SynthesisUI.SortAsDefault();
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
        SynthesisUI.SetButtons(index);
    }

    public void OnClickSynthesisUnselectBtn()
    {
        SynthesisUI.CancelSelect();

        if (SceneManager.GetActiveScene().name == "Stage0" && (GameManager.Inst().Tutorials.Step == 51 || GameManager.Inst().Tutorials.Step == 56))
            GameManager.Inst().Tutorials.Step++;
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
        //int bulletType = EquipUI.GetBulletType(index);
        int equipType = GameManager.Inst().Player.GetItem(index).Type;

        //EquipUI.Unequip(bulletType, equipType);
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
        {
            Resource.SetActive(true);

            if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 3)
                GameManager.Inst().Tutorials.Step++;
        }            
    }

    public void OnClickSideBarBtn()
    {
        if (SideMenuUI.IsOpen)
        {
            OnClickSideBarBackBtn();
            return;
        }

        SideMenuUI.IsOpen = true;
        SideMenuUI.SideMenuOpen();

        for(int i = 0; i < GameManager.Inst().StgManager.ReachedStage - 1; i++)
            GetSideMenuSlot(i).ShowGatheringArea(i);

        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);

        if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 18)
            GameManager.Inst().Tutorials.Step++;
    }

    public void OnClickSideBarBackBtn()
    {
        SideMenuUI.IsOpen = false;
        SideMenuUI.SideMenuClose();

        GameManager.Inst().IptManager.SetIsAbleControl(true);
        GameManager.Inst().IptManager.SetIsAbleSWControl(true);

        if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 22)
            GameManager.Inst().Tutorials.Step++;
    }
   
    public void OnClickSpaceBtn()
    {
        OnClickHomeBtn();

        StageScroll.SetActive(true);
        StageScrollUI.Show();

        ZzinBottomUI.UniverseIcon[0].SetActive(false);
        ZzinBottomUI.UniverseIcon[1].SetActive(true);
        //ZzinBottomUI.HomeIcon.alpha = 1.0f;
        ZzinBottomUI.HomeBtn.SetActive(true);

        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);
    }

    public void OnClickSpaceBackBtn()
    {
        StageScroll.SetActive(false);

        ZzinBottomUI.UniverseIcon[0].SetActive(true);
        ZzinBottomUI.UniverseIcon[1].SetActive(false);
        //ZzinBottomUI.HomeIcon.alpha = 0.0f;
        ZzinBottomUI.HomeBtn.SetActive(false);

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

    public void OnClickChangeEquipBtn()
    {
        int num = ScrollViewUI.OnClickYesBtn();

        CurrentBulletType = num;

        SetBulletSelected();
    }

    public void OnClickHomeBtn()
    {
        if (StageScroll.activeSelf)
            OnClickSpaceBackBtn();
        if (Weapon.activeSelf)
            OnClickWeaponBackBtn();
        if (Inventory.activeSelf)
            OnClickInventoryBackBtn();
        if (Synthesis.activeSelf)
            OnClickSynthesisBackBtn();
        if (Shop.activeSelf)
            OnClickShopBackBtn();
        if (Cheat.activeSelf)
            OnClickCheatBackBtn();

        if (SceneManager.GetActiveScene().name == "Stage0" && 
            (GameManager.Inst().Tutorials.Step == 23 || GameManager.Inst().Tutorials.Step == 26 || GameManager.Inst().Tutorials.Step == 27 || GameManager.Inst().Tutorials.Step ==  23 ||
            GameManager.Inst().Tutorials.Step == 40 || GameManager.Inst().Tutorials.Step == 43 || GameManager.Inst().Tutorials.Step == 47 || GameManager.Inst().Tutorials.Step == 48 ||
            GameManager.Inst().Tutorials.Step == 61))
            GameManager.Inst().Tutorials.Step++;
    }

    public void OnClickWeaponInfoBtn()
    {
        if (!WeaponUI.InfoWindow.gameObject.activeSelf)
        {
            WeaponUI.InfoWindow.gameObject.SetActive(true);
            CurrentBulletType = WeaponUI.GetCurBulletType();
            WeaponUI.InfoWindow.Show(CurrentBulletType);
        }
        else
            WeaponUI.InfoWindow.gameObject.SetActive(false);

        if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 31)
            GameManager.Inst().Tutorials.Step++;
    }

    public void OnClickLandingBtn()
    {
        StageScrollUI.MoveScene();
    }

    public void OnClickShopBtn()
    {
        OnClickHomeBtn();
        Debug.Log("ShopBtn");

        Shop.SetActive(true);
        ShopUI.OnSelectToggle(0);

        ZzinBottomUI.ShopIcon[0].SetActive(false);
        ZzinBottomUI.ShopIcon[1].SetActive(true);
        //ZzinBottomUI.HomeIcon.alpha = 1.0f;
        ZzinBottomUI.HomeBtn.SetActive(true);

        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);
    }

    public void OnClickShopBackBtn()
    {
        Shop.SetActive(false);

        ZzinBottomUI.ShopIcon[0].SetActive(true);
        ZzinBottomUI.ShopIcon[1].SetActive(false);
        //ZzinBottomUI.HomeIcon.alpha = 0.0f;
        ZzinBottomUI.HomeBtn.SetActive(false);

        GameManager.Inst().IptManager.SetIsAbleControl(true);
        GameManager.Inst().IptManager.SetIsAbleSWControl(true);
    }

    public void OnClickCheatBtn()
    {
        OnClickHomeBtn();

        Cheat.gameObject.SetActive(true);

        //ZzinBottomUI.HomeIcon.alpha = 1.0f;
        ZzinBottomUI.HomeBtn.SetActive(true);

        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);
    }

    public void OnClickCheatBackBtn()
    {
        Cheat.SetActive(false);

        //ZzinBottomUI.HomeIcon.alpha = 0.0f;
        ZzinBottomUI.HomeBtn.SetActive(false);

        GameManager.Inst().IptManager.SetIsAbleControl(true);
        GameManager.Inst().IptManager.SetIsAbleSWControl(true);
    }

    public void OnClickReviveBtn()
    {
        GameManager.Inst().AddJewel(-1);
        ReviveAlertUI.gameObject.SetActive(false);

        GameManager.Inst().GetSubweapons(ReviveAlertUI.Index).CoolTime = 0;
    }

    public void OnClickUploadDataBtn()
    {
        //GameManager.Inst().DatManager.GameData.UploadSave();
        GameManager.Inst().DatManager.UploadSaveData();
    }

    public void OnClickDownloadDataBtn()
    {
        GameManager.Inst().DatManager.DownloadSaveData();
    }
}
