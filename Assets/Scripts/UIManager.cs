using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject SubWeapon;
    public GameObject Turret;
    public GameObject Background;
    public GameObject Panel;
    public GameObject Slot;

    public GameObject MainUI;
    public GameObject Detail;
    public GameObject[] Slots;

    MainUI MainUi;
    Detail DetailUI;
    Slot[] SlotUI;

    Vector3 PlayerPosOrigin;
    Vector3 SubWeaponPosOrigin;
    Vector3 TurretPosOrigin;
    Vector3 BackgroudnPosOrigin;
    Vector3 PanelPosOrigin;

    Vector3 PlayerPosUI;
    Vector3 SubWeaponPosUI;
    Vector3 TurretPosUI;
    Vector3 BackgroundPosUI;
    Vector3 PanelPosUI;

    bool IsMoveUp;
    bool IsShowingDetail;
    float TickCount;
    int CurrentBulletType;

    public void SetIsMoveUp(bool b) { IsMoveUp = b; }

    void Awake()
    {
        PlayerPosOrigin = Player.transform.position;
        SubWeaponPosOrigin = SubWeapon.transform.position;
        TurretPosOrigin = Turret.transform.position;
        BackgroudnPosOrigin = Background.transform.position;
        PanelPosOrigin = Panel.transform.position;
        Debug.Log(TurretPosOrigin);

        PlayerPosUI = new Vector3(0.0f, 3.5f, 0.0f);
        SubWeaponPosUI = new Vector3(0.0f, PlayerPosUI.y - 0.24f, 0.0f);
        TurretPosUI = new Vector3(0.0f, 6.3f, 90.0f);
        BackgroundPosUI = new Vector3(0.0f, PlayerPosUI.y + 3.3f, 0.0f);
        PanelPosUI = new Vector3(0.0f, 1.5f, 90.0f);

        TickCount = 1.0f / 6.0f;

        IsMoveUp = false;
        IsShowingDetail = false;
        CurrentBulletType = -1;

        MainUi = MainUI.GetComponent<MainUI>();
        DetailUI = Detail.GetComponent<Detail>();
        SlotUI = new Slot[Bullet.MAXBULLETS];
        for (int i = 0; i < Bullet.MAXBULLETS; i++)
            SlotUI[i] = Slots[i].GetComponent<Slot>();
    }

    void Update()
    {
        if (IsMoveUp)
            MoveUp();

        if (IsShowingDetail)
            ShowingDetail();
    }

    void MoveUp()
    {
        Player.transform.position = Vector3.MoveTowards(Player.transform.position, PlayerPosUI, TickCount);
        SubWeapon.transform.position = Vector3.MoveTowards(SubWeapon.transform.position, SubWeaponPosUI, TickCount);
        Turret.transform.position = Vector3.MoveTowards(Turret.transform.position, TurretPosUI, TickCount);
        Background.transform.position = Vector3.MoveTowards(Background.transform.position, BackgroundPosUI, TickCount);
        Panel.transform.position = Vector3.MoveTowards(Panel.transform.position, PanelPosUI, TickCount);

        if (TickCount >= 1.0f)
            IsMoveUp = false;
    }

    void ShowingDetail()
    {

    }

    public void OnClickUpgradeBtn()
    {
        MainUi.OnClickUpgradeBtn();
    }

    public void OnClickManageBtn()
    {
        MainUi.OnClickManageBtn();
        IsMoveUp = true;

        for(int i = 0; i < Bullet.MAXBULLETS; i++)
        {
            SlotUI[i].SetSlotDetail(i);
        }
    }

    public void ShowDetail(int Type)
    {
        Slot.SetActive(false);
        Detail.SetActive(true);
        DetailUI.SetBulletType(Type);
        DetailUI.SetDetails();
    }

    public void OnClickBulletUpgradeBtn()
    {
        DetailUI.OnClickUpgradeBtn();
    }
}
