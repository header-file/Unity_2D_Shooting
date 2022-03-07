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
    public BackGround Background;

    //기능 구현용
    public MainUI MainUI;

    //기타 UI
    public GameObject RedMask;
    public PlayerHitArea[] PlayerHitAreas;
    public GameObject InventoryScroll;
    public GameObject[] SubPositions;

    //UI용 이미지
    public Sprite[] WeaponImages;
    public Sprite[] FoodImages;
    public Sprite[] EquipImages;

    //메인 UI Pref
    public GameObject MainUIPref;

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

    bool IsMoveUp;
    bool IsMoveDown;
    float Timer;
    float TickCount;
    bool IsEquip;


    public bool GetIsMoveUp() { return IsMoveUp; }
    public bool GetIsMoveDown() { return IsMoveDown; }

    public void SetCoinText(int coin) { MainUI.CoinText.text = string.Format("{0:#,###}", coin); }
    public void SetHitAreas(GameObject sub, int index) { PlayerHitAreas[index].Object = sub; }
    public void SetIsMoveUp(bool b) { IsMoveUp = b; }
    public void SetIsMoveDown(bool b) { IsMoveDown = b; }
    public void SetTimer(float time) { Timer = time; }
    public void SetIsEquip(bool b) { IsEquip = b; }

    void Awake()
    {
        GameManager.Inst().UiManager = gameObject.GetComponent<UIManager>();

        MainUI = Instantiate(MainUIPref).GetComponent<MainUI>();

        PlayerPosOrigin = GameManager.Inst().Player.transform.position;
        SubWeaponPosOrigin = SubWeapon.transform.position;
        TurretPosOrigin = MainUI.Center.Turret.transform.position;
        BackgroundPosOrigin = Background.transform.position;
        PanelPosOrigin = MainUI.Bottom.Panel.transform.position;

        PlayerPosUI = new Vector3(0.0f, 3.3f, 0.0f);
        SubWeaponPosUI = new Vector3(0.0f, PlayerPosUI.y - 0.24f, 0.0f);
        TurretPosUI = new Vector3(0.0f, 6.15f, 90.0f);
        BackgroundPosUI = new Vector3(0.0f, PlayerPosUI.y - 2.0f, 0.0f);
        PanelPosUI = new Vector3(0.0f, 2.0f, 90.0f);

        Timer = 0.0f;
        TickCount = 1.0f / 30.0f;

        IsMoveUp = false;
        IsMoveDown = false;
        CurrentBulletType = 0;
        IsEquip = false;
    }

    void Start()
    {
        PlayerHitAreas[4].Object = GameManager.Inst().Player.gameObject;

        //GameManager.Inst().Player.AddCoin(0);
        //GameManager.Inst().AddJewel(0);
        for (int i = 1; i <= Constants.MAXRESOURCETYPES; i++)
            GameManager.Inst().AddResource(i, 0);
        if (SceneManager.GetActiveScene().name != "Stage0")
        {
            GameManager.Inst().DatManager.GameData.LoadSubWeapon();
            GameManager.Inst().DatManager.GameData.LoadDaily();
        }

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
        MainUI.Center.Turret.transform.position = Vector3.MoveTowards(MainUI.Center.Turret.transform.position, TurretPosUI, Timer);
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
        MainUI.Center.Turret.transform.position = Vector3.MoveTowards(MainUI.Center.Turret.transform.position, TurretPosOrigin, Timer);
        Background.transform.position = Vector3.MoveTowards(Background.transform.position, BackgroundPosOrigin, Timer);
        MainUI.Bottom.Panel.transform.position = Vector3.MoveTowards(MainUI.Bottom.Panel.transform.position, PanelPosOrigin, Timer);

        Timer += TickCount;

        if (Timer >= 1.0f)
        {
            IsMoveDown = false;
            MainUI.Bottom.gameObject.SetActive(false);
            GameManager.Inst().Player.SetUIPosOri();
            
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
        MainUI.Center.Turret.transform.GetChild(GameManager.Inst().UiManager.MainUI.Bottom.BuySW.GetSelectedIndex()).GetChild(0).gameObject.GetComponent<Button>().interactable = b;
        MainUI.Center.Turret.transform.GetChild(GameManager.Inst().UiManager.MainUI.Bottom.BuySW.GetSelectedIndex()).GetChild(0).gameObject.SetActive(false);
    }

    public void SetSubWeaponInteratable(int index, bool b)
    {
        MainUI.Center.Turret.transform.GetChild(index).GetChild(0).gameObject.GetComponent<Button>().interactable = b;
        MainUI.Center.Turret.transform.GetChild(index).GetChild(0).gameObject.SetActive(false);
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

    //Button Interact   
    public void UnlockStage(int index)
    {
        MainUI.Center.StageScroll.Planets[index].SetLock(false);

        if (GameManager.Inst().StgManager.ReachedStage > 1)
            MainUI.SideMenu.MakeSlot();
    }
}
