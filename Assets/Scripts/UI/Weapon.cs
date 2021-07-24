using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.U2D.Animation;
using UnityEngine.SceneManagement;

public class Weapon : MonoBehaviour
{
    public GameObject InventoryArea;
    public GameObject SwitchWindow;
    public WeaponSwitchWindow[] SwitchWindows;
    public GameObject ArrowButtons;
    public InfoArea InfoArea;
    public SwitchWindow EquipArea;
    public Color GaugeColor;
    public WeaponInfoWindow InfoWindow;
    public Sprite[] WeaponIcons;
    public WeaponCell[] WeaponCells;
    public LoopScroll2 WeaponCellScroll;
    public Image[] EquipImage;
    public Text[] EquipName;
    public Text[] EquipDetail;
    public EquipSwitch EquipSwitch;

    InventoryScroll Inventories;
    Player.EqData CurEquip;
    Sprite DefaultEquipImg;
    int[] SlotIndices;
    int CurBulletType;
    int ShowBulletType;
    bool IsMoving;
    bool IsFlickering;
    float MoveTimer;
    float TargetX;
    int[] TempCount;
    bool IsDelay;
    Vector2 DefaultWindowPos;


    public int GetCurBulletType() { return CurBulletType; }
    public Player.EqData GetCurEquip() { return CurEquip; }

    public void SetCurBulletType(int type) { CurBulletType = type; }

    void Start()
    {
        IsMoving = false;
        IsFlickering = false;
        MoveTimer = 0.0f;

        SlotIndices = new int[3];
        SlotIndices[0] = Constants.MAXBULLETS - 1;
        SlotIndices[1] = 0;
        SlotIndices[2] = 1;

        TargetX = 0.0f;

        ShowBulletType = 1;

        TempCount = new int[3];
        for(int i = 0; i < 3; i++)
            TempCount[i] = 0;

        IsDelay = false;

        for (int i = 0; i < Constants.MAXBULLETS; i++)
            WeaponCells[i].Icon.sprite = WeaponIcons[i];

        InfoArea.gameObject.SetActive(true);
        EquipArea.gameObject.SetActive(false);
        gameObject.SetActive(false);

        DefaultEquipImg = EquipImage[0].sprite;
        for(int i = 0; i < 2; i++)
        {
            EquipName[i].text = "";
            EquipDetail[i].text = "";
        }
        EquipSwitch.gameObject.SetActive(false);

        DefaultWindowPos = SwitchWindow.GetComponent<RectTransform>().anchoredPosition;
    }

    void Update()
    {
        if (IsMoving)
            Moving();
        else
            WeaponCellScroll.MoveToSelected(SlotIndices[ShowBulletType]);

        if (IsFlickering)
            Flickering();
    }

    void Moving()
    {
        MoveTimer += Time.deltaTime * 5.0f;
        Vector3 pos = SwitchWindow.GetComponent<RectTransform>().anchoredPosition;
        pos.x = Mathf.Lerp(pos.x, TargetX, MoveTimer);

        SwitchWindow.GetComponent<RectTransform>().anchoredPosition = pos;

        if (MoveTimer <= 0.5f)
            InfoArea.SetAlpha(1.0f - MoveTimer * 2.0f);
        else if (MoveTimer <= 1.0f)
            InfoArea.SetAlpha(MoveTimer * 2.0f - 1.0f);

        if (MoveTimer - 0.5f <= 0.001f)
            ShowInfoArea();

        WeaponCellScroll.MoveToSelected(CurBulletType);

        if (MoveTimer >= 1.0f)
        {
            IsMoving = false;
            MoveTimer = 0.0f;
            
            for (int i = 0; i < 2; i++)
                ArrowButtons.transform.GetChild(i).GetComponent<Button>().interactable = true;

            InfoArea.SetWeaponName(SlotIndices[ShowBulletType]);
            InfoArea.ShowDetail(SlotIndices[ShowBulletType]);

            if (GameManager.Inst().UpgManager.BData[CurBulletType].GetEquipIndex() != -1)
            {
                int index = GameManager.Inst().UpgManager.BData[CurBulletType].GetEquipIndex();
                CurEquip = GameManager.Inst().Player.GetItem(index);
                SetWeaponUI();
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    EquipImage[i].sprite = DefaultEquipImg;
                    EquipName[i].text = "";
                    EquipDetail[i].text = "";
                }
            }

            for (int i = 0; i < 3; i++)
            {
                if (SwitchWindows[i].transform.position.x > 6.0f)
                {
                    Vector2 newPos = SwitchWindows[i].GetComponent<RectTransform>().anchoredPosition;
                    newPos.x -= 720.0f * 3;

                    SwitchWindows[i].GetComponent<RectTransform>().anchoredPosition = newPos;

                    SlotIndices[i] += 4;
                    if (SlotIndices[i] >= Constants.MAXBULLETS)
                        SlotIndices[i] -= Constants.MAXBULLETS;
                    SwitchWindows[i].Skin.SetCategoryAndLabel("Skin", GameManager.Inst().Player.Types[SlotIndices[i]]);
                    InfoArea.Anim[i].SetInteger("Color", InfoArea.DefaultColor[SlotIndices[i]]);
                    Show(SlotIndices[i]);
                }

                if (SwitchWindows[i].transform.position.x < -6.0f)
                {
                    Vector2 newPos = SwitchWindows[i].GetComponent<RectTransform>().anchoredPosition;
                    newPos.x += 720.0f * 3;

                    SwitchWindows[i].GetComponent<RectTransform>().anchoredPosition = newPos;

                    SlotIndices[i] -= 4;
                    if (SlotIndices[i] < 0)
                        SlotIndices[i] += Constants.MAXBULLETS;
                    SwitchWindows[i].Skin.SetCategoryAndLabel("Skin", GameManager.Inst().Player.Types[SlotIndices[i]]);
                    InfoArea.Anim[i].SetInteger("Color", InfoArea.DefaultColor[SlotIndices[i]]);
                    Show(SlotIndices[i]);
                }
            }

        }
    }

    void Flickering()
    {
        GaugeColor.a -= 0.1f;
        if (GaugeColor.a < 0.0f)
            GaugeColor.a = 1.0f;

        for(int i = 0; i < 3; i++)
        {
            switch(i)
            {
                case 0:
                    if (TempCount[i] + GameManager.Inst().UpgManager.BData[CurBulletType].GetAtk() > GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxAtk())
                        EquipArea.FlickeringGauge(i, CurBulletType, GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxAtk() - GameManager.Inst().UpgManager.BData[CurBulletType].GetAtk(), GaugeColor);
                    else
                        EquipArea.FlickeringGauge(i, CurBulletType, TempCount[i], GaugeColor);
                    break;
                case 1:
                    if (TempCount[i] + GameManager.Inst().UpgManager.BData[CurBulletType].GetHp() > GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxHp())
                        EquipArea.FlickeringGauge(i, CurBulletType, GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxHp() - GameManager.Inst().UpgManager.BData[CurBulletType].GetHp(), GaugeColor);
                    else
                        EquipArea.FlickeringGauge(i, CurBulletType, TempCount[i], GaugeColor);
                    break;
                case 2:
                    if (TempCount[i] + GameManager.Inst().UpgManager.BData[CurBulletType].GetSpd() > GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxSpd())
                        EquipArea.FlickeringGauge(i, CurBulletType, GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxSpd() - GameManager.Inst().UpgManager.BData[CurBulletType].GetSpd(), GaugeColor);
                    else
                        EquipArea.FlickeringGauge(i, CurBulletType, TempCount[i], GaugeColor);
                    break;
            }
        }
            
    }

    public void ShowUI()
    {
        ResetSwitchWindow();

        for (int i = 0; i < 3; i++)
        {
            SwitchWindows[i].Skin.SetCategoryAndLabel("Skin", GameManager.Inst().Player.Types[SlotIndices[i]]);
            InfoArea.Anim[i].SetInteger("Color", InfoArea.DefaultColor[SlotIndices[i]]);
        }
        Show(SlotIndices[1]);

        
        ShowInfoArea();
        ShowInventory();
    }

    void ResetSwitchWindow()
    {
        SwitchWindow.GetComponent<RectTransform>().anchoredPosition = DefaultWindowPos;
        for (int i = 0; i < 3; i++)
        {
            Vector2 pos = Vector2.zero;
            pos.x = (i - 1) * 720.0f;
            SwitchWindows[i].GetComponent<RectTransform>().anchoredPosition = pos;
        }

        CurBulletType = GameManager.Inst().Player.GetBulletType();
        ShowBulletType = 1;

        SlotIndices[1] = CurBulletType;
        SlotIndices[0] = CurBulletType - 1;
        if (SlotIndices[0] < 0)
            SlotIndices[0] = Constants.MAXBULLETS - 1;
        SlotIndices[2] = CurBulletType + 1;
        if (SlotIndices[2] >= Constants.MAXBULLETS)
            SlotIndices[2] = 0;

        TargetX = 0.0f;

        if (GameManager.Inst().StgManager.CheckBulletUnlocked(SlotIndices[ShowBulletType]))
            SwitchWindows[ShowBulletType].Lock.SetActive(false);
        else
        {
            SwitchWindows[ShowBulletType].Lock.SetActive(true);
            SwitchWindows[ShowBulletType].LockText.text = "Stage" + GameManager.Inst().StgManager.UnlockBulletStages[SlotIndices[ShowBulletType]] + "\n클리어 시 해금";
        }
    }

    void ShowInfoArea()
    {
        InfoArea.SetWeaponName(CurBulletType);
        InfoArea.ShowDetail(CurBulletType);
        InfoArea.UpgradeBtn.interactable = GameManager.Inst().UpgManager.BData[CurBulletType].GetActive();
        InfoArea.EquipAreaBtn.interactable = GameManager.Inst().UpgManager.BData[CurBulletType].GetActive();

        if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 34)
        {
            InfoArea.UpgradeBtn.interactable = true;
            InfoArea.EquipAreaBtn.interactable = true;
        }

        for (int i = 0; i < 3; i++)
            InfoArea.PaintGauge(i, CurBulletType);

        if (GameManager.Inst().UpgManager.BData[CurBulletType].GetEquipIndex() != -1)
        {
            int index = GameManager.Inst().UpgManager.BData[CurBulletType].GetEquipIndex();
            CurEquip = GameManager.Inst().Player.GetItem(index);
            SetWeaponUI();
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                EquipImage[i].sprite = DefaultEquipImg;
                EquipName[i].text = "";
                EquipDetail[i].text = "";
            }
        }
    }

    public void ShowEquipArea()
    {
        InfoArea.gameObject.SetActive(false);
        EquipArea.gameObject.SetActive(true);
        EquipArea.InfoWindow.SetActive(false);

        //ShowInventory();

        for (int i = 0; i < 3; i++)
            Show(CurBulletType);
    }

    void Show(int BulletType)
    {
        for (int i = 0; i < 3; i++)
            PaintGauge(i, BulletType);
    }

    void ShowInventory()
    {
        Inventories = GameManager.Inst().UiManager.InventoryScroll.GetComponent<InventoryScroll>();
        Inventories.transform.SetParent(InventoryArea.transform, false);
        Inventories.SetSlotType(1);

        Inventories.ShowInventory();

        for (int i = 0; i < Constants.MAXBULLETS; i++)
        {
            if (GameManager.Inst().UpgManager.BData[i].GetEquipIndex() != -1)
            {
                InventorySlot slot = Inventories.GetSlot(Inventories.GetSwitchedIndex(GameManager.Inst().UpgManager.BData[i].GetEquipIndex()));
                slot.EMark.SetActive(true);
            }
        }
    }

    public void SortAsType(int type)
    {
        Inventories.ResetInventory();

        for (int i = 0; i < GameManager.Inst().Player.MaxInventory; i++)
        {
            Player.EqData eq = GameManager.Inst().Player.GetItem(i);
            Inventories.GetSlot(i).Selected.SetActive(false);

            if (eq != null)
            {
                if (eq.Type == type)
                {
                    Sprite icon = GameManager.Inst().UiManager.FoodImages[eq.Type + eq.Rarity * Constants.MAXREINFORCETYPE];
                    if (eq.UID / 100 == 6)
                        icon = eq.Icon;

                    InventorySlot slot = Inventories.GetSlot(i);
                    slot.gameObject.SetActive(true);
                    slot.GetNotExist().SetActive(false);
                    slot.GetExist().SetActive(true);
                    slot.SetIcon(icon);
                    slot.SetDisable(false);
                    slot.SetGradeSprite(eq.Rarity);
                }
                else
                {
                    Sprite icon = GameManager.Inst().UiManager.FoodImages[eq.Type + eq.Rarity * Constants.MAXREINFORCETYPE];
                    InventorySlot slot = Inventories.GetSlot(i);
                    slot.gameObject.SetActive(true);

                    slot.GetNotExist().SetActive(false);
                    slot.GetExist().SetActive(true);
                    slot.SetIcon(icon);
                    slot.SetDisable(true);
                    slot.SetGradeSprite(eq.Rarity);
                }

            }
        }

        GameManager.Inst().Player.SortOption = type + (int)InventorySlot.SortOption.TYPE_RARITY;

        Inventories.Sort();
    }

    public void Select(int index, int BulletType)
    {
        CurEquip = GameManager.Inst().Player.GetItem(index);

        if (CurEquip.UID / 100 == 3)
        {
            if (CurEquip.Quantity > 0)
                TempCount[CurEquip.Type] += (int)CurEquip.Value;

            EquipArea.WindowOn(CurEquip, TempCount[CurEquip.Type] / (int)CurEquip.Value);

            IsFlickering = true;
        }
        else if (CurEquip.UID / 100 == 6)
        {
            if (GameManager.Inst().UpgManager.BData[CurBulletType].GetEquipIndex() == -1)
            {
                for (int i = 0; i < Constants.MAXBULLETS; i++)
                {
                    if (GameManager.Inst().UpgManager.BData[i].GetEquipIndex() == index)
                    {
                        EquipSwitch.Show(CurBulletType, index);
                        return;
                    }
                }

                Equip(CurBulletType, index);
            }
            else
                EquipSwitch.Show(CurBulletType, index);                
        }
    }

    public void Equip(int bulletType, int index)
    {
        GameManager.Inst().UpgManager.BData[bulletType].SetEquipIndex(index);

        //Vamp
        if (GameManager.Inst().Player.GetItem(index).Type == (int)Item_ZzinEquipment.EquipType.VAMP)
            EquipVamp();

        //Reinforce
        if (GameManager.Inst().Player.GetItem(index).Type == (int)Item_ZzinEquipment.EquipType.REINFORCE)
            EquipReinforce();

        //Weapon UI
        SetWeaponUI();

        //InventorySlot UI
        if (index == -1)
            return;

        InventorySlot slot = Inventories.GetSlot(Inventories.GetSwitchedIndex(index));
        slot.EMark.SetActive(true);
    }

    public void Switch()
    {
        int eqindex = EquipSwitch.EquippedIndex;
        int selindex = EquipSwitch.SelectedIndex;

        InventorySlot slot = null;
        if (GameManager.Inst().UpgManager.BData[CurBulletType].GetEquipIndex() != -1)
        {
            slot = Inventories.GetSlot(Inventories.GetSwitchedIndex(GameManager.Inst().UpgManager.BData[CurBulletType].GetEquipIndex()));
            slot.EMark.SetActive(false);
        }

        //
        for (int i = 0; i < Constants.MAXBULLETS; i++)
        {
            if (GameManager.Inst().UpgManager.BData[i].GetEquipIndex() == eqindex)
            {
                UnEquipVamp(i);
                UnEquipReinforce(i);

                if (i != CurBulletType && GameManager.Inst().UpgManager.BData[i].GetEquipIndex() != -1)
                    Equip(i, GameManager.Inst().UpgManager.BData[CurBulletType].GetEquipIndex());
            }
        }

        EquipSwitch.gameObject.SetActive(false);

        Equip(CurBulletType, selindex);
    }

    void EquipVamp()
    {
        UnEquipReinforce(CurBulletType);

        GameManager.Inst().UpgManager.BData[CurBulletType].SetIsVamp(true);
    }

    void UnEquipVamp(int bulletType)
    {
        if (GameManager.Inst().UpgManager.BData[CurBulletType].GetIsVamp())
            GameManager.Inst().UpgManager.BData[CurBulletType].SetIsVamp(false);
    }

    void EquipReinforce()
    {
        UnEquipVamp(CurBulletType);

        GameManager.Inst().UpgManager.BData[CurBulletType].SetIsReinforce(true);
    }

    void UnEquipReinforce(int bulletType)
    {
        if (GameManager.Inst().UpgManager.BData[CurBulletType].GetIsReinforce())
            GameManager.Inst().UpgManager.BData[CurBulletType].SetIsReinforce(false);
    }

    void SetWeaponUI()
    {
        for (int i = 0; i < 2; i++)
        {
            EquipImage[i].sprite = CurEquip.Icon;
            EquipName[i].text = GameManager.Inst().TxtManager.EquipName[CurEquip.Type];
        }
        EquipDetail[0].text = GameManager.Inst().TxtManager.EquipDetailSimple[CurEquip.Type];

        string detail = "";
        if(GameManager.Inst().EquipDatas[CurEquip.Type, CurEquip.Rarity, 0] > 0)
            detail += GameManager.Inst().EquipDatas[CurEquip.Type, CurEquip.Rarity, 0].ToString();
        detail += GameManager.Inst().TxtManager.EquipDetailFront[CurEquip.Type];
        if (CurEquip.Value > 0)
            detail += CurEquip.Value.ToString();
        detail += GameManager.Inst().TxtManager.EquipDetailBack[CurEquip.Type];
        EquipDetail[1].text = detail;
    }

    public void AddQuantity()
    {
        if (IsDelay)
            return;

        IsDelay = true;
        Invoke("ResetDelay", 0.1f);

        if (!IsFlickering)
            IsFlickering = true;

        if(CurEquip.Quantity > TempCount[CurEquip.Type] / (int)CurEquip.Value)
        {
            switch(CurEquip.Type)
            {
                case 0:
                    if (TempCount[CurEquip.Type] + GameManager.Inst().UpgManager.BData[CurBulletType].GetAtk() < GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxAtk())
                    {
                        TempCount[CurEquip.Type] += (int)CurEquip.Value;
                        if (TempCount[CurEquip.Type] + GameManager.Inst().UpgManager.BData[CurBulletType].GetAtk() > GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxAtk())
                            EquipArea.SetInfo(GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxAtk() - GameManager.Inst().UpgManager.BData[CurBulletType].GetAtk(), TempCount[CurEquip.Type] / (int)CurEquip.Value);
                        else
                            EquipArea.SetInfo(TempCount[CurEquip.Type], TempCount[CurEquip.Type] / (int)CurEquip.Value);
                    }
                    break;
                case 1:
                    if (TempCount[CurEquip.Type] + GameManager.Inst().UpgManager.BData[CurBulletType].GetHp() < GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxHp())
                    {
                        TempCount[CurEquip.Type] += (int)CurEquip.Value;
                        if (TempCount[CurEquip.Type] + GameManager.Inst().UpgManager.BData[CurBulletType].GetHp() > GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxHp())
                            EquipArea.SetInfo(GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxHp() - GameManager.Inst().UpgManager.BData[CurBulletType].GetHp(), TempCount[CurEquip.Type] / (int)CurEquip.Value);
                        else
                            EquipArea.SetInfo(TempCount[CurEquip.Type], TempCount[CurEquip.Type] / (int)CurEquip.Value);
                    }
                    break;
                case 2:
                    if (TempCount[CurEquip.Type] + GameManager.Inst().UpgManager.BData[CurBulletType].GetSpd() < GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxSpd())
                    {
                        TempCount[CurEquip.Type] += (int)CurEquip.Value;
                        if (TempCount[CurEquip.Type] + GameManager.Inst().UpgManager.BData[CurBulletType].GetSpd() > GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxSpd())
                            EquipArea.SetInfo(GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxSpd() - GameManager.Inst().UpgManager.BData[CurBulletType].GetSpd(), TempCount[CurEquip.Type] / (int)CurEquip.Value);
                        else
                            EquipArea.SetInfo(TempCount[CurEquip.Type], TempCount[CurEquip.Type] / (int)CurEquip.Value);
                    }
                    break;
            }
        }
    }

    public void SubTQuantity()
    {
        if (IsDelay)
            return;

        IsDelay = true;
        Invoke("ResetDelay", 0.1f);

        if (TempCount[CurEquip.Type] / (int)CurEquip.Value > 1)
        {
            TempCount[CurEquip.Type] -= (int)CurEquip.Value;
            EquipArea.SetInfo(TempCount[CurEquip.Type], TempCount[CurEquip.Type] / (int)CurEquip.Value);
        }
    }

    public void SetStatChange()
    {
        switch(CurEquip.Type)
        {
            case 0:
                GameManager.Inst().UpgManager.BData[CurBulletType].SetAtk(GameManager.Inst().UpgManager.BData[CurBulletType].GetAtk() + TempCount[CurEquip.Type]);
                break;
            case 1:
                GameManager.Inst().UpgManager.BData[CurBulletType].SetHp(GameManager.Inst().UpgManager.BData[CurBulletType].GetHp() + TempCount[CurEquip.Type]);
                break;
            case 2:
                GameManager.Inst().UpgManager.BData[CurBulletType].SetSpd(GameManager.Inst().UpgManager.BData[CurBulletType].GetSpd() + TempCount[CurEquip.Type]);
                break;
        }

        CurEquip.Quantity -= (TempCount[CurEquip.Type] / (int)CurEquip.Value);
        TempCount[CurEquip.Type] = 0;
        IsFlickering = false;

        EquipArea.PaintGauge(CurEquip.Type, CurBulletType, TempCount[CurEquip.Type]);
        ShowInventory();
    }

    void ResetDelay()
    {
        IsDelay = false;
    }

    public void ResetData()
    {
        CurEquip = null;

        for(int i = 0; i < 3; i++)
            TempCount[i] = 0;

        IsFlickering = false;
        IsDelay = false;
    }

    public void ResetInventory()
    {
        Inventories.ResetInventory();
    }

    void PaintGauge(int type, int bulletType)
    {
        EquipArea.PaintGauge(type, bulletType, TempCount[type]);
    }

    public void Next(bool IsNext)
    {
        IsMoving = true;

        if (IsNext)
        {
            CurBulletType++;
            ShowBulletType++;
            TargetX -= 720.0f;

            if (CurBulletType >= Constants.MAXBULLETS)
                CurBulletType = 0;

            if (ShowBulletType > 2)
                ShowBulletType = 0;
        }
        else
        {
            CurBulletType--;
            ShowBulletType--;
            TargetX += 720.0f;

            if (CurBulletType < 0)
                CurBulletType = Constants.MAXBULLETS - 1;

            if (ShowBulletType < 0)
                ShowBulletType = 2;
        }

        for (int i = 0; i < 2; i++)
            ArrowButtons.transform.GetChild(i).GetComponent<Button>().interactable = false;

        //Lock
        if (GameManager.Inst().StgManager.CheckBulletUnlocked(SlotIndices[ShowBulletType]))
            SwitchWindows[ShowBulletType].Lock.SetActive(false);
        else
        {
            SwitchWindows[ShowBulletType].Lock.SetActive(true);
            SwitchWindows[ShowBulletType].LockText.text = "Stage" + GameManager.Inst().StgManager.UnlockBulletStages[SlotIndices[ShowBulletType]] + "\n클리어 시 해금";
        }

        if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 30)
            GameManager.Inst().Tutorials.Step++;
    }
}
