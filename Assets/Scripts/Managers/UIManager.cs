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
    public BackGround Background;

    //기능 구현용
    public MainUI MainUI;
    public Tutorial Tutorial;

    //기타 UI
    public GameObject RedMask;
    public PlayerHitArea[] PlayerHitAreas;
    public GameObject InventoryScroll;
    public GameObject[] SubPositions;

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

    public int CurrentBulletType;
    public int CurrentPlayer;

    bool IsMoveUp;
    bool IsMoveDown;
    float Timer;
    float TickCount;
    bool IsEquip;


    public BuySubWeapon GetBuySWUI() { return BuySWUI; }
    public bool GetIsMoveUp() { return IsMoveUp; }
    public bool GetIsMoveDown() { return IsMoveDown; }

    public void SetCoinText(int coin) { MainUI.CoinText.text = coin.ToString(); }
    public void SetHitAreas(GameObject sub, int index) { PlayerHitAreas[index].Object = sub; }
    public void SetIsMoveUp(bool b) { IsMoveUp = b; }
    public void SetIsMoveDown(bool b) { IsMoveDown = b; }
    public void SetTimer(float time) { Timer = time; }
    public void SetIsEquip(bool b) { IsEquip = b; }

    void Awake()
    {
        GameManager.Inst().UiManager = gameObject.GetComponent<UIManager>();

        PlayerPosOrigin = GameManager.Inst().Player.transform.position;
        SubWeaponPosOrigin = SubWeapon.transform.position;
        TurretPosOrigin = Turret.transform.position;
        BackgroundPosOrigin = Background.transform.position;
        PanelPosOrigin = MainUI.Bottom.Panel.transform.position;

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

        //GameManager.Inst().Player.AddCoin(0);
        //GameManager.Inst().AddJewel(0);
        for (int i = 1; i <= Constants.MAXRESOURCETYPES; i++)
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
        MainUI.Bottom.Panel.transform.position = Vector3.MoveTowards(MainUI.Bottom.Panel.transform.position, PanelPosUI, Timer);

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
        MainUI.Bottom.Panel.transform.position = Vector3.MoveTowards(MainUI.Bottom.Panel.transform.position, PanelPosOrigin, Timer);

        Timer += TickCount;

        if (Timer >= 1.0f)
        {
            IsMoveDown = false;
            
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
    public void ShowInfoArea(int Type)
    {
        MainUI.Center.Weapon.InfoArea.ShowDetail(Type);
    }

    public void InfoAreaTrigger(string trigger)
    {
        MainUI.Center.Weapon.InfoArea.SetAnimTrigger(trigger);
    }

    public void UnlockStage(int index)
    {
        MainUI.Center.StageScroll.Planets[index].Lock.SetActive(false);

        if (GameManager.Inst().StgManager.ReachedStage > 1)
            MainUI.SideMenu.MakeSlot();
    }

    public SideMenuSlot GetSideMenuSlot(int i)
    {
        if(MainUI.SideMenu.Slots.Length == Constants.MAXSTAGES)
            return MainUI.SideMenu.Slots[i];

        return null;
    }

    public void ShowReviveAlert(int index)
    {
        MainUI.Center.ReviveAlert.gameObject.SetActive(true);

        MainUI.Center.ReviveAlert.Show(index);
    }


    //Click Interactions
    public void OnClickInventoryDetailBtn(int index)
    {
        MainUI.Center.Inventory.InventoryDetail.gameObject.SetActive(true);

        MainUI.Center.Inventory.InventoryDetail.ShowDetail(index);
    }

    public void OnClickInventoryDetailBackBtn()
    {
        MainUI.Center.Inventory.InventoryDetail.gameObject.SetActive(false);
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
   
    public void OnClickBulletEquipBtn()
    {
        int num = MainUI.Bottom.WeaponScroll.OnClickEquipBtn();
        if (num == -1)
            return;

        CurrentBulletType = num;

        MainUI.Bottom.SetBulletSelected();
    }

    public void OnClickChangeEquipBtn()
    {
        int num = MainUI.Bottom.WeaponScroll.OnClickYesBtn();

        CurrentBulletType = num;

        MainUI.Bottom.SetBulletSelected();
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
        MainUI.ZzinBottom.OnClickHomeBtn();

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
