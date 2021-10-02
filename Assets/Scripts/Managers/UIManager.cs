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
    public MainUI MainUI;
    public Tutorial Tutorial;

    //기타 UI
    public GameObject RedMask;
    public PlayerHitArea[] PlayerHitAreas;
    public GameObject InventoryScroll;
    public GameObject[] SubPositions;
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
    public GameObject TurretUI;

    //새 윈도우
    public GameObject[] NewWindows;

    //UI용 이미지
    public Sprite[] WeaponImages;
    public Sprite[] FoodImages;
    public Sprite[] EquipImages;


    BuySubWeapon BuySWUI;

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

    public void SetCoinText(int coin) { MainUI.CoinText.text = coin.ToString(); }
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

        BuySWUI = NewWindows[(int)NewWindowType.BUYSUBWEAPON].GetComponent<BuySubWeapon>();

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

        if (GameManager.Inst().StgManager.Stage > 0)
            GameManager.Inst().StgManager.BeginStage();
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
            MainUI.Bottom.Slots[i].DetailBtn.gameObject.SetActive(false);
        }

        //SlotUI[CurrentBulletType].Selected.SetActive(true);
        MainUI.Bottom.Slots[curBulletType].DetailBtn.gameObject.SetActive(true);
    }

    public void SetSlotsActive(int index, bool isActive, int unlockStage)
    {
        MainUI.Bottom.Slots[index].Locked.SetActive(!isActive);
        MainUI.Bottom.Slots[index].StageName.text = "Stage" + unlockStage.ToString();
    }

    public void InventoryFull()
    {
        MainUI.Center.PlayInventoryFull();
    }

    public void BossWarning()
    {
        MainUI.Center.PlayBossWarning();

        GameManager.Inst().SodManager.PlayEffect("Warning boss");
    }


    //Button Interact
    public void OnClickManageBtn(int Type)
    {
        //Time.timeScale = 0.0f;
        GameManager.Inst().IptManager.SetIsAbleControl(false);

        CurrentWeapon = Type;
        MainUI.Bottom.WeaponScroll.SetCurrentCharacter(CurrentWeapon);

        if (!IsMoveUp)
            Timer = 0.0f;
        IsMoveUp = true;

        NewWindows[(int)NewWindowType.WEAPON].SetActive(true);
        NewWindows[(int)NewWindowType.DETAIL].SetActive(false);
        NewWindows[(int)NewWindowType.BUYSUBWEAPON].SetActive(false);
        NewWindows[(int)NewWindowType.INFO].GetComponent<Info>().SetColorSelected(GameManager.Inst().ShtManager.GetColorSelection(Type));
        //NewWindows[(int)NewWindowType.INFO].SetActive(false);

        for (int i = 0; i < 5; i++)
            MainUI.Bottom.Arrows.transform.GetChild(i).gameObject.SetActive(false);

        MainUI.Bottom.Arrows.transform.GetChild(Type).gameObject.SetActive(true);

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
            MainUI.Bottom.Slots[i].Show(i);

        ShowEquipBtn(CurrentBulletType);
        SetBulletSelected();

        MainUI.Bottom.WeaponScroll.MoveToSelected(CurrentBulletType);

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
        MainUI.Center.Weapon.InfoArea.ShowDetail(Type);
    }

    public void InfoAreaTrigger(string trigger)
    {
        MainUI.Center.Weapon.InfoArea.SetAnimTrigger(trigger);
    }

    void SetBulletSelected()
    {
        for (int i = 0; i < Constants.MAXBULLETS; i++)
            MainUI.Bottom.Slots[i].Selected.SetActive(false);

        MainUI.Bottom.Slots[CurrentBulletType].Selected.SetActive(true);
    }

    public void UnlockStage(int index)
    {
        MainUI.Center.StageScroll.Planets[index].Lock.SetActive(false);

        if (GameManager.Inst().StgManager.ReachedStage > 1)
            MainUI.SideMenu.MakeSlot();
    }

    public SideMenuSlot GetSideMenuSlot(int i)
    {
        return MainUI.SideMenu.Slots[i];
    }

    public void ShowReviveAlert(int index)
    {
        MainUI.Center.ReviveAlert.gameObject.SetActive(true);

        MainUI.Center.ReviveAlert.Show(index);
    }


    //Click Interactions
    public void OnClickManageCancel()
    {
        if (!IsMoveDown)
            Timer = 0.0f;
        IsMoveDown = true;

        for (int i = 0; i < 5; i++)
            MainUI.Bottom.Arrows.transform.GetChild(i).gameObject.SetActive(false);

        GameManager.Inst().Player.EquipUI.SetActive(false);
        for (int i = 0; i < Constants.MAXSUBWEAPON; i++)
        {
            if (GameManager.Inst().GetSubweapons(i) != null)
                GameManager.Inst().UiManager.MainUI.Center.Turrets[i].EquipUI.SetActive(false);
        }
    }

    public void OnClickToWeapon()
    {
        NewWindows[(int)NewWindowType.INFO].SetActive(false);
        NewWindows[(int)NewWindowType.WEAPON].SetActive(true);

        for (int i = 0; i < Constants.MAXBULLETS; i++)
            MainUI.Bottom.Slots[i].Show(i);

        ShowEquipBtn(CurrentBulletType);
        SetBulletSelected();

        MainUI.Bottom.WeaponScroll.MoveToSelected(CurrentBulletType);
    }

    public void OnClickDetailCancel()
    {
        NewWindows[(int)NewWindowType.DETAIL].SetActive(false);
        OnClickManageBtn(CurrentWeapon);
    }

    public void OnClickBulletUpgradeBtn()
    {
        //DetailUI.OnClickUpgradeBtn();
        CurrentBulletType = MainUI.Center.Weapon.GetCurBulletType();
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
            MainUI.Bottom.Arrows.transform.GetChild(i).gameObject.SetActive(false);

        if (index > 1)
            index++;
        MainUI.Bottom.Arrows.transform.GetChild(index).gameObject.SetActive(true);

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
            MainUI.Bottom.Arrows.transform.GetChild(i).gameObject.SetActive(false);
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

        MainUI.Center.Inventory.ShowInventory();

        MainUI.ZzinBottom.HomeBtn.SetActive(true);

        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);
    }

    public void OnClickInventoryBackBtn()
    {
        MainUI.Center.Inventory.CloseInventory();
        MainUI.Center.Inventory.gameObject.SetActive(false);

        MainUI.ZzinBottom.HomeBtn.SetActive(false);

        GameManager.Inst().IptManager.SetIsAbleControl(true);
        GameManager.Inst().IptManager.SetIsAbleSWControl(true);
    }

    public void OnClickInventoryDetailBtn(int index)
    {
        MainUI.Center.Inventory.InventoryDetail.gameObject.SetActive(true);

        MainUI.Center.Inventory.InventoryDetail.ShowDetail(index);
    }

    public void OnClickInventoryDetailBackBtn()
    {
        MainUI.Center.Inventory.InventoryDetail.gameObject.SetActive(false);
    }

    public void OnClickWeaponBtn()
    {
        OnClickHomeBtn();

        MainUI.Center.Inventory.gameObject.SetActive(false);
        MainUI.Center.Inventory.InventoryDetail.gameObject.SetActive(false);
        MainUI.Center.Weapon.gameObject.SetActive(true);

        CurrentBulletType = GameManager.Inst().Player.GetBulletType();
        MainUI.Center.Weapon.SetCurBulletType(CurrentBulletType);
        MainUI.Center.Weapon.ShowUI();
        IsEquip = true;

        MainUI.Center.Weapon.InfoArea.gameObject.SetActive(true);
        MainUI.Center.Weapon.EquipArea.gameObject.SetActive(false);

        MainUI.ZzinBottom.WeaponIcon[0].SetActive(false);
        MainUI.ZzinBottom.WeaponIcon[1].SetActive(true);
        MainUI.ZzinBottom.HomeBtn.SetActive(true);

        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);
    }

    public void OnClickWeaponBackBtn()
    {
        MainUI.Center.Weapon.ResetData();
        MainUI.Center.Weapon.ResetInventory();
        MainUI.Center.Weapon.gameObject.SetActive(false);
        IsEquip = false;

        MainUI.ZzinBottom.WeaponIcon[0].SetActive(true);
        MainUI.ZzinBottom.WeaponIcon[1].SetActive(false);
        MainUI.ZzinBottom.HomeBtn.SetActive(false);

        GameManager.Inst().IptManager.SetIsAbleControl(true);
        GameManager.Inst().IptManager.SetIsAbleSWControl(true);
    }

    public void OnClickEquipAreaBtn()
    {
        MainUI.Center.Weapon.ShowEquipArea();

        if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 34)
            GameManager.Inst().Tutorials.Step++;
    }

    public void OnClickWeaponTypeSortBtn(int index)
    {
        MainUI.Center.Weapon.SortAsType(index);
    }

    public void OnClickEquipSelectBtn(int index)
    {
        MainUI.Center.Weapon.Select(index, MainUI.Center.Weapon.GetCurBulletType());
    }

    public void OnClickUnequipBtn()
    {
        //EquipUI.Unequip(EquipUI.GetCurBulletType());
    }

    public void OnClickSwitchBtn()
    {
        MainUI.Center.Weapon.Switch();
    }

    public void OnClickSwitchCancelBtn()
    {
        MainUI.Center.Weapon.EquipSwitch.gameObject.SetActive(false);
    }

    public void OnClickNextButton(bool IsNext)
    {
        MainUI.Center.Weapon.Next(IsNext);
    }

    public void OnClickSynthesisBtn()
    {
        OnClickHomeBtn();

        MainUI.Center.Inventory.InventoryDetail.gameObject.SetActive(false);
        MainUI.Center.Inventory.gameObject.SetActive(false);

        MainUI.ZzinBottom.SynthesisIcon[0].SetActive(false);
        MainUI.ZzinBottom.SynthesisIcon[1].SetActive(true);
        MainUI.ZzinBottom.HomeBtn.SetActive(true);

        MainUI.Center.Synthesis.gameObject.SetActive(true);
        MainUI.Center.Synthesis.ShowInventory();

        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);
    }

    public void OnClickSynthesisBackBtn()
    {
        MainUI.Center.Synthesis.ResetSprites();
        MainUI.Center.Synthesis.gameObject.SetActive(false);

        MainUI.ZzinBottom.SynthesisIcon[0].SetActive(true);
        MainUI.ZzinBottom.SynthesisIcon[1].SetActive(false);
        MainUI.ZzinBottom.HomeBtn.SetActive(false);

        GameManager.Inst().IptManager.SetIsAbleControl(true);
        GameManager.Inst().IptManager.SetIsAbleSWControl(true);
    }

    public void OnClickSynthesisSlotBtn(int index)
    {
        if (index < 3)
            MainUI.Center.Synthesis.SortAsDefault();
        else if (index == 3)
            MainUI.Center.Synthesis.ShowConfirmWindow();
    }

    public void OnClickSynthesisConfirmBtn()
    {
        MainUI.Center.Synthesis.Synthesize();
    }

    public void OnClickSynthesisConfirmBackBtn()
    {
        MainUI.Center.Synthesis.CancelConfirm();
    }

    public void OnClickSynthesisSelectBtn(int index)
    {
        MainUI.Center.Synthesis.SetButtons(index);
    }

    public void OnClickSynthesisUnselectBtn()
    {
        MainUI.Center.Synthesis.CancelSelect();

        if (SceneManager.GetActiveScene().name == "Stage0" && (GameManager.Inst().Tutorials.Step == 51 || GameManager.Inst().Tutorials.Step == 56))
            GameManager.Inst().Tutorials.Step++;
    }

    public void OnClickSynthesisResultBackBtn()
    {
        MainUI.Center.Synthesis.CloseResult();
    }

    public void OnClickSelectDetailBackBtn()
    {
        MainUI.Center.Synthesis.CloseDetail();
    }

    public void OnClickSynthesisUnequipBtn()
    {
        int index = MainUI.Center.Synthesis.GetUnequipIndex();
        int equipType = GameManager.Inst().Player.GetItem(index).Type;

        MainUI.Center.Synthesis.SetButtons(index);

        MainUI.Center.Synthesis.CloseUnequip();
    }

    public void OnClickUnequipConfirmBackBtn()
    {
        MainUI.Center.Synthesis.CloseUnequip();
    }

    public void OnClickResourceToggleBtn()
    {
        if (MainUI.Resource.activeSelf)
            MainUI.Resource.SetActive(false);
        else
        {
            MainUI.Resource.SetActive(true);

            if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 3)
                GameManager.Inst().Tutorials.Step++;
        }            
    }

    public void OnClickSideBarBtn()
    {
        if (MainUI.SideMenu.IsOpen)
        {
            OnClickSideBarBackBtn();
            return;
        }

        MainUI.SideMenu.IsOpen = true;
        MainUI.SideMenu.SideMenuOpen();

        for(int i = 0; i < GameManager.Inst().StgManager.ReachedStage - 1; i++)
            GetSideMenuSlot(i).ShowGatheringArea(i);

        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);

        if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 18)
            GameManager.Inst().Tutorials.Step++;
    }

    public void OnClickSideBarBackBtn()
    {
        MainUI.SideMenu.IsOpen = false;
        MainUI.SideMenu.SideMenuClose();

        GameManager.Inst().IptManager.SetIsAbleControl(true);
        GameManager.Inst().IptManager.SetIsAbleSWControl(true);

        if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 22)
            GameManager.Inst().Tutorials.Step++;
    }
   
    public void OnClickSpaceBtn()
    {
        OnClickHomeBtn();

        MainUI.Center.StageScroll.gameObject.SetActive(true);
        MainUI.Center.StageScroll.Show();

        MainUI.ZzinBottom.UniverseIcon[0].SetActive(false);
        MainUI.ZzinBottom.UniverseIcon[1].SetActive(true);
        MainUI.ZzinBottom.HomeBtn.SetActive(true);

        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);
    }

    public void OnClickSpaceBackBtn()
    {
        MainUI.Center.StageScroll.gameObject.SetActive(false);

        MainUI.ZzinBottom.UniverseIcon[0].SetActive(true);
        MainUI.ZzinBottom.UniverseIcon[1].SetActive(false);
        MainUI.ZzinBottom.HomeBtn.SetActive(false);

        GameManager.Inst().IptManager.SetIsAbleControl(true);
        GameManager.Inst().IptManager.SetIsAbleSWControl(true);
    }

    public void OnClickBulletEquipBtn()
    {
        int num = MainUI.Bottom.WeaponScroll.OnClickEquipBtn();
        if (num == -1)
            return;

        CurrentBulletType = num;

        SetBulletSelected();
    }

    public void OnClickChangeEquipBtn()
    {
        int num = MainUI.Bottom.WeaponScroll.OnClickYesBtn();

        CurrentBulletType = num;

        SetBulletSelected();
    }

    public void OnClickHomeBtn()
    {
        if (MainUI.Center.StageScroll.gameObject.activeSelf)
            OnClickSpaceBackBtn();
        if (MainUI.Center.Weapon.gameObject.activeSelf)
            OnClickWeaponBackBtn();
        if (MainUI.Center.Inventory.gameObject.activeSelf)
            OnClickInventoryBackBtn();
        if (MainUI.Center.Synthesis.gameObject.activeSelf)
            OnClickSynthesisBackBtn();
        if (MainUI.Center.Shop.gameObject.activeSelf)
            OnClickShopBackBtn();
        if (MainUI.Center.Cheat.gameObject.activeSelf)
            OnClickCheatBackBtn();

        if (SceneManager.GetActiveScene().name == "Stage0" && 
            (GameManager.Inst().Tutorials.Step == 23 || GameManager.Inst().Tutorials.Step == 26 || GameManager.Inst().Tutorials.Step == 27 || GameManager.Inst().Tutorials.Step ==  23 ||
            GameManager.Inst().Tutorials.Step == 40 || GameManager.Inst().Tutorials.Step == 43 || GameManager.Inst().Tutorials.Step == 47 || GameManager.Inst().Tutorials.Step == 48 ||
            GameManager.Inst().Tutorials.Step == 61))
            GameManager.Inst().Tutorials.Step++;
    }

    public void OnClickWeaponInfoBtn()
    {
        if (!MainUI.Center.Weapon.InfoWindow.gameObject.activeSelf)
        {
            MainUI.Center.Weapon.InfoWindow.gameObject.SetActive(true);
            CurrentBulletType = MainUI.Center.Weapon.GetCurBulletType();
            MainUI.Center.Weapon.InfoWindow.Show(CurrentBulletType);
        }
        else
            MainUI.Center.Weapon.InfoWindow.gameObject.SetActive(false);

        if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 31)
            GameManager.Inst().Tutorials.Step++;
    }

    public void OnClickLandingBtn()
    {
        GameManager.Inst().SodManager.PlayEffect("Landing");

        MainUI.Center.StageScroll.MoveScene();
    }

    public void OnClickShopBtn()
    {
        OnClickHomeBtn();
        Debug.Log("ShopBtn");

        MainUI.Center.Shop.gameObject.SetActive(true);
        MainUI.Center.Shop.OnSelectToggle(0);

        MainUI.ZzinBottom.ShopIcon[0].SetActive(false);
        MainUI.ZzinBottom.ShopIcon[1].SetActive(true);
        MainUI.ZzinBottom.HomeBtn.SetActive(true);

        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);
    }

    public void OnClickShopBackBtn()
    {
        MainUI.Center.Shop.gameObject.SetActive(false);

        MainUI.ZzinBottom.ShopIcon[0].SetActive(true);
        MainUI.ZzinBottom.ShopIcon[1].SetActive(false);
        MainUI.ZzinBottom.HomeBtn.SetActive(false);

        GameManager.Inst().IptManager.SetIsAbleControl(true);
        GameManager.Inst().IptManager.SetIsAbleSWControl(true);
    }

    public void OnClickCheatBtn()
    {
        OnClickHomeBtn();

        MainUI.Center.Cheat.gameObject.SetActive(true);

        MainUI.ZzinBottom.HomeBtn.SetActive(true);

        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);
    }

    public void OnClickCheatBackBtn()
    {
        MainUI.Center.Cheat.gameObject.SetActive(false);

        MainUI.ZzinBottom.HomeBtn.SetActive(false);

        GameManager.Inst().IptManager.SetIsAbleControl(true);
        GameManager.Inst().IptManager.SetIsAbleSWControl(true);
    }

    public void OnClickReviveBtn()
    {
        GameManager.Inst().AddJewel(-1);
        MainUI.Center.ReviveAlert.gameObject.SetActive(false);

        GameManager.Inst().GetSubweapons(MainUI.Center.ReviveAlert.Index).CoolTime = 0;
    }

    public void OnClickUploadDataBtn()
    {
        GameManager.Inst().DatManager.UploadSaveData();
    }

    public void OnClickDownloadDataBtn()
    {
        GameManager.Inst().DatManager.DownloadSaveData();
    }
}
