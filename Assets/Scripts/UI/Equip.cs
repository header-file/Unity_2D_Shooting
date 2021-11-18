using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.U2D.Animation;

public class Equip : MonoBehaviour
{
    public GameObject InventoryArea;
    public GameObject[] TypeText;
    public GameObject BeforeText;
    public GameObject AfterText;
    public GameObject BulletText;
    public GameObject SwitchWindows;
    public GameObject ArrowButtons;
    public InfoArea InfoArea;
    public SpriteResolver[] Skins;
    public Sprite OriginalSprite;
    public SwitchWindow EquipArea;

    InventoryScroll Inventories;
    Item_Equipment.EquipmentType SelectableType;
    Color[] GaugeColors;
    Color GlowColor;

    int[,] Selected;
    int SwitchBuffer;
    bool IsShowingSwitch;
    int SelectedIndex;
    int[,] LastIndex;
    bool IsBtoB;
    int SwitchableBulletType;
    bool IsMoving;
    float MoveTimer;
    int CurBulletType;
    int ShowBulletType;
    int[] SlotIndices;
    float[] SlotPoses;
    float TargetX;

    public int GetSwitchBuffer() { return SwitchBuffer; }
    public int GetSelected(int currentBulletType) { return Selected[currentBulletType, (int)SelectableType]; }
    public int GetCurBulletType() { return CurBulletType; }

    public void SetSwichBuffer(int i) { SwitchBuffer = i; }
    public void SetIsShowingSwitch(bool b) { IsShowingSwitch = b; }
    public void SetIsBtoB(bool b) { IsBtoB = false; }
    public void SetCurBulletType(int type) { CurBulletType = type; }

    public void DisableSelectedSlot()
    {
        if (SelectedIndex <= -1)
            return;

        //for (int i = 0; i < GameManager.Inst().Player.MAXINVENTORY; i++)
        //{
        //    if (Inventories.GetSlot(i).GetItemUID() == GameManager.Inst().Player.GetItem(SelectedIndex).UID)
        //        Inventories.GetSlot(i).SetSelected(false);
        //}
        Inventories.GetSlot(Inventories.GetSwitchedIndex(SelectedIndex)).SetSelected(false);
    }

    void Start()
    {
        SelectableType = 0;
        
        GaugeColors = new Color[3];
        GaugeColors[0] = Color.red;
        GaugeColors[1] = new Color(0.5f, 0.0f, 1.0f);
        GaugeColors[2] = new Color(0.1882353f, 0.8862746f, 0.7372549f);

        Selected = new int[Constants.MAXBULLETS, 3];
        for (int i = 0; i < Constants.MAXBULLETS; i++)
        {
            Selected[i, 0] = -1;
            Selected[i, 1] = -1;
            Selected[i, 2] = -1;
        }

        SwitchBuffer = -1;
        IsShowingSwitch = false;
        gameObject.SetActive(false);
        LastIndex = new int[Constants.MAXBULLETS, 3];
        for (int j = 0; j < Constants.MAXBULLETS; j++)
        {
            for (int i = 0; i < 3; i++)
                LastIndex[j, i] = -1;
        }

        GlowColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        IsBtoB = false;
        SelectedIndex = -1;
        SwitchableBulletType = -1;
        IsMoving = false;
        MoveTimer = 0.0f;

        SlotIndices = new int[3];
        SlotIndices[0] = Constants.MAXBULLETS - 1;
        SlotIndices[1] = 0;
        SlotIndices[2] = 1;

        SlotPoses = new float[3];
        SlotPoses[0] = -720.0f;
        SlotPoses[1] = 0.0f;
        SlotPoses[2] = 720.0f;
        TargetX = 0.0f;

        ShowBulletType = 1;
    }

    void Update()
    {
        if (IsShowingSwitch)
            Flickering();

        if (IsMoving)
            Moving();
    }

    void Flickering()
    {
        GlowColor.a -= 0.1f;

        if (GlowColor.a < 0.0f)
            GlowColor.a = 1.0f;

        int before = 0;
        if (Selected[CurBulletType, (int)SelectableType] > -1)
            before = (int)GameManager.Inst().Player.GetItem(Selected[CurBulletType, (int)SelectableType]).Value / 10;

        int after = (int)GameManager.Inst().Player.GetItem(SelectedIndex).Value / 10;

        //SwitchWindows.transform.GetChild(ShowBulletType).GetComponent<SwitchWindow>()
        //EquipArea.FlickerGauge(before, after, (int)SelectableType, GaugeColors[(int)SelectableType], GlowColor.a);
    }

    void Moving()
    {
        MoveTimer += Time.deltaTime * 5.0f;
        Vector3 pos = SwitchWindows.GetComponent<RectTransform>().anchoredPosition;
        pos.x = Mathf.Lerp(pos.x, TargetX , MoveTimer);

        SwitchWindows.GetComponent<RectTransform>().anchoredPosition = pos;

        if (MoveTimer <= 0.5f)
            InfoArea.SetAlpha(1.0f - MoveTimer * 2.0f);
        else if (MoveTimer <= 1.0f)
            InfoArea.SetAlpha(MoveTimer * 2.0f - 1.0f);

        if(MoveTimer - 0.5f <= 0.001f)
            ShowInfoArea();

        if (MoveTimer >= 1.0f)
        {
            IsMoving = false;
            MoveTimer = 0.0f;

            for (int i = 0; i < 2; i++)
                ArrowButtons.transform.GetChild(i).GetComponent<Button>().interactable = true;

            InfoArea.SetWeaponName(SlotIndices[ShowBulletType]);
            InfoArea.ShowDetail(SlotIndices[ShowBulletType]);

            for (int i = 0; i < 3; i++)
            {
                if (SwitchWindows.transform.GetChild(i).position.x > 6.0f)
                {
                    Vector2 newPos = SwitchWindows.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition;
                    newPos.x -= 720.0f * 3;

                    SwitchWindows.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = newPos;

                    SlotIndices[i] += 4;
                    if (SlotIndices[i] >= Constants.MAXBULLETS)
                        SlotIndices[i] -= Constants.MAXBULLETS;
                    Skins[i].SetCategoryAndLabel("Skin", GameManager.Inst().Player.Types[SlotIndices[i]]);
                    InfoArea.Anim[i].SetInteger("Color", InfoArea.DefaultColor[SlotIndices[i]]);
                    Show(SlotIndices[i], i);
                }

                if (SwitchWindows.transform.GetChild(i).position.x < -6.0f)
                {
                    Vector2 newPos = SwitchWindows.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition;
                    newPos.x += 720.0f * 3;

                    SwitchWindows.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = newPos;

                    SlotIndices[i] -= 4;
                    if (SlotIndices[i] < 0)
                        SlotIndices[i] += Constants.MAXBULLETS;
                    Skins[i].SetCategoryAndLabel("Skin", GameManager.Inst().Player.Types[SlotIndices[i]]);
                    InfoArea.Anim[i].SetInteger("Color", InfoArea.DefaultColor[SlotIndices[i]]);
                    Show(SlotIndices[i], i);
                }
            }
                
        }
    }

    public void ShowUI()
    {
        SlotIndices[1] = CurBulletType;
        SlotIndices[0] = CurBulletType - 1;
        if (SlotIndices[0] < 0)
            SlotIndices[0] = Constants.MAXBULLETS - 1;
        SlotIndices[2] = CurBulletType + 1;
        if (SlotIndices[2] >= Constants.MAXBULLETS)
            SlotIndices[2] = 0;

        for (int i = 0; i < 3; i++)
        {
            Show(SlotIndices[i], i);
            Skins[i].SetCategoryAndLabel("Skin", GameManager.Inst().Player.Types[SlotIndices[i]]);
            InfoArea.Anim[i].SetInteger("Color", InfoArea.DefaultColor[SlotIndices[i]]);
        }

        ShowInventory();

        ShowBulletType = 1;
        ShowInfoArea();
    }

    void ShowInfoArea()
    {
        InfoArea.SetWeaponName(SlotIndices[ShowBulletType]);
        InfoArea.UpgradeBtn.interactable = GameManager.Inst().UpgManager.BData[SlotIndices[ShowBulletType]].GetActive();
        InfoArea.EquipAreaBtn.interactable = GameManager.Inst().UpgManager.BData[SlotIndices[ShowBulletType]].GetActive();

        for(int i = 0; i < 3; i++)
            InfoArea.PaintGauge(i, ShowBulletType);

        //
        //{
        //    if (Selected[SlotIndices[ShowBulletType], i] > -1)
        //    {
        //        Sprite img = GameManager.Inst().Player.GetItem(Selected[SlotIndices[ShowBulletType], i]).Icon;
        //        int grade = GameManager.Inst().Player.GetItem(Selected[SlotIndices[ShowBulletType], i]).Rarity;
        //        int value = (int)GameManager.Inst().Player.GetItem(Selected[SlotIndices[ShowBulletType], i]).Value;

        //        InfoArea.SetSlots(i, true, img, grade);
        //        InfoArea.PaintGauge(i, );
        //    }
        //    else
        //    {
        //        //InfoArea.SetSlots(i, false, OriginalSprite, -1);

        //    }
        //}
    }

    void Show(int BulletType, int index)
    {
        ShowEquipDetail(BulletType, index);

        ShowEquipped(BulletType, index);
    }

    void ShowEquipDetail(int BulletType, int index)
    {
        //SwitchWindows.transform.GetChild(index).GetComponent<SwitchWindow>().SetCurrentBulletImg(GameManager.Inst().UiManager.WeaponImages[BulletType]);

        for(int i = 0; i < 3; i++)
            PaintGauge(i, BulletType);
        //{
        //    if (Selected[BulletType, i] > -1)
        //    {
        //        int value = (int)GameManager.Inst().Player.GetItem(Selected[BulletType, i]).Value;

        //        PaintGauge(index, i, value);
        //    }
        //    else
        //        PaintGauge(index, i, 0);
        //}

        //스페셜 할 예정?
    }

    void ShowEquipped(int BulletType, int index)
    {
        for (int i = 0; i < 3; i++)
        {
            if (Selected[BulletType, i] > -1)
            {
                Sprite img = GameManager.Inst().Player.GetItem(Selected[BulletType, i]).Icon;
                int grade = GameManager.Inst().Player.GetItem(Selected[BulletType, i]).Rarity;

                //SwitchWindows.transform.GetChild(index).GetComponent<SwitchWindow>()
                EquipArea.SetButtons(i, true, img, grade);
            }
            else
                //SwitchWindows.transform.GetChild(index).GetComponent<SwitchWindow>()
                EquipArea.SetButtons(i, false, OriginalSprite, -1);
        }
    }

    void ShowInventory()
    {
        SelectableType = 0;

        Inventories = GameManager.Inst().UiManager.InventoryScroll.GetComponent<InventoryScroll>();
        Inventories.transform.SetParent(InventoryArea.transform, false);
        Inventories.SetSlotType(1);

        Inventories.ShowInventory();
    }

    public void ShowSwitch(int index)
    {
        for (int i = 0; i < 2; i++)
        {
            Text text = TypeText[i].GetComponent<Text>();
            switch ((int)SelectableType)
            {
                case 0:
                    text.text = "ATK";
                    break;
                case 1:
                    text.text = "RNG";
                    break;
                case 2:
                    text.text = "SPD";
                    break;
            }
            text.color = GaugeColors[(int)SelectableType];
        }

        if (Selected[CurBulletType, (int)SelectableType] != -1)
            BeforeText.GetComponent<Text>().text = GameManager.Inst().Player.GetItem(Selected[CurBulletType, (int)SelectableType]).Value.ToString();
        else
            BeforeText.GetComponent<Text>().text = "0";

        AfterText.GetComponent<Text>().text = GameManager.Inst().Player.GetItem(index).Value.ToString();

        //다른 Bullet이 착용중일 시
        IsBtoB = false;
        BulletText.GetComponent<Text>().text = "";
        for (int i = 0; i < Constants.MAXBULLETS; i++)
        {
            if (Selected[i, (int)SelectableType] == index)
            {
                //BulletText.GetComponent<Text>().text = GameManager.Inst().TxtManager.GetBNames(i);
                SwitchableBulletType = i;
                IsBtoB = true;
                break;
            }
        }
        if (SelectedIndex > -1)
        {
            //for (int i = 0; i < GameManager.Inst().Player.MAXINVENTORY; i++)
            //{
            //    if (Inventories.GetSlot(i).GetItemUID() == GameManager.Inst().Player.GetItem(SelectedIndex).UID)
            //        Inventories.GetSlot(i).SetSelected(false);
            //}
            Inventories.GetSlot(Inventories.GetSwitchedIndex(SelectedIndex)).SetSelected(false);
        }

        SelectedIndex = index;

        //for(int i = 0; i < GameManager.Inst().Player.MAXINVENTORY; i++)
        //{
        //    if(Inventories.GetSlot(i).GetItemUID() == GameManager.Inst().Player.GetItem(index).UID)
        //        Inventories.GetSlot(i).SetSelected(true);
        //}
        Inventories.GetSlot(Inventories.GetSwitchedIndex(index)).SetSelected(true);

        IsShowingSwitch = true;
    }

    public void SortAsType(int type)
    {
        SelectableType = (Item_Equipment.EquipmentType)type;

        SetIsShowingSwitch(false);

        Inventories.ResetInventory();

        for (int i = 0; i < GameManager.Inst().Player.MaxInventory; i++)
        {
            Player.EqData eq = GameManager.Inst().Player.GetItem(i);
            Inventories.GetSlot(i).Selected.SetActive(false);

            if (eq != null)
            {
                if (eq.Type == type)
                {
                    Sprite icon = eq.Icon;
                    InventorySlot slot = Inventories.GetSlot(i);
                    slot.gameObject.SetActive(true);

                    slot.GetNotExist().SetActive(false);
                    slot.GetExist().SetActive(true);
                    slot.SetIcon(icon);
                    slot.SetDisable(false);
                    slot.SetGradeSprite(eq.Rarity);
                    
                    switch (eq.Type)
                    {
                        case 0:
                            slot.GetIcon().transform.rotation = Quaternion.Euler(0.0f, 0.0f, 60.0f);
                            break;
                        case 1:
                            slot.GetIcon().transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                            break;
                        case 2:
                            slot.GetIcon().transform.rotation = Quaternion.Euler(0.0f, 0.0f, -60.0f);
                            break;
                    }
                }
                else
                {
                    Sprite icon = eq.Icon;
                    InventorySlot slot = Inventories.GetSlot(i);
                    slot.gameObject.SetActive(true);

                    slot.GetNotExist().SetActive(false);
                    slot.GetExist().SetActive(true);
                    slot.SetIcon(icon);
                    slot.SetDisable(true);
                    slot.SetGradeSprite(eq.Rarity);

                    switch (eq.Type)
                    {
                        case 0:
                            slot.GetIcon().transform.rotation = Quaternion.Euler(0.0f, 0.0f, 60.0f);
                            break;
                        case 1:
                            slot.GetIcon().transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                            break;
                        case 2:
                            slot.GetIcon().transform.rotation = Quaternion.Euler(0.0f, 0.0f, -60.0f);
                            break;
                    }
                    //Inventories.GetSlot(i).gameObject.SetActive(false);
                }

            }
            else
                break;
        }
        //Inventories.transform.GetChild(0).gameObject.SetActive(true);
        GameManager.Inst().Player.SortOption = type + (int)InventorySlot.SortOption.TYPE_RARITY;

        Inventories.Sort();

        Inventories.None.SetActive(true);
    }


    public void Select(int index, int BulletType)
    {
        
    }

    void PaintGauge(int type, int bulletType)
    {
        //EquipArea.PaintGauge(type, bulletType);
    }

    public void Switch(int index, int BulletType)
    {
        if (IsBtoB)
            SwitchBtoB(index, BulletType);
        else
        {
            //E마크
            //for (int i = 0; i < GameManager.Inst().Player.MAXINVENTORY; i++)
            //{
            //    InventorySlot slot = Inventories.GetSlot(i);
            //    if (slot.GetItemUID() == GameManager.Inst().Player.GetItem(LastIndex[BulletType, (int)SelectableType]).UID)
            //    {
            //        slot.SetEmark(false);
            //        slot.SetSelected(false);
            //        break;
            //    }
            //}
            InventorySlot slot = Inventories.GetSlot(Inventories.GetSwitchedIndex(LastIndex[BulletType, (int)SelectableType]));
            slot.SetEmark(false);
            slot.SetSelected(false);

            Select(index, BulletType);
        }
    }

    public void SwitchCancel()
    {
        //Inventories.GetSlot(Inventories.GetSwitchedIndex(SelectedIndex)).SetSelected(false);
                
        //if (Selected[CurBulletType, (int)SelectableType] > -1)
        //    PaintGauge(ShowBulletType, (int)SelectableType, GameManager.Inst().Player.GetItem(Selected[CurBulletType, (int)SelectableType]).Value);
        //else
        //    PaintGauge(ShowBulletType, (int)SelectableType, 0);
        //SetIsShowingSwitch(false);
        //IsBtoB = false;
    }

    public bool CheckAlreadyEquip(int index)
    {
        for (int i = 0; i < Constants.MAXBULLETS; i++)
        {
            if (Selected[i, (int)SelectableType] == index)
                return true;
        }

        SwitchableBulletType = -1;
        return false;
    }

    public bool CheckAlreadyEquipAll(int index)
    {
        for (int i = 0; i < Constants.MAXBULLETS; i++)
        {
            for (int j = 0; j < 3; j++)
                if (Selected[i, j] == index)
                    return true;
        }

        return false;
    }

    public int GetBulletType(int index)
    {
        for (int i = 0; i < Constants.MAXBULLETS; i++)
        {
            for (int j = 0; j < 3; j++)
                if (Selected[i, j] == index)
                    return i;
        }

        return -1;
    }

    public void SwitchBtoB(int index, int BulletType)
    {
        if (Selected[BulletType, (int)SelectableType] == -1)
        {
            Unequip(SwitchableBulletType);
            Select(index, BulletType);
        }
        else
        {
            Select(Selected[BulletType, (int)SelectableType], SwitchableBulletType);
            Select(index, BulletType);
        }
    }

    public void Unequip(int BulletType)
    {
        //EquipArea.GetComponent<SwitchWindow>().SetButtons((int)SelectableType, false, OriginalSprite, -1);

        ////E마크
        //if (LastIndex[BulletType, (int)SelectableType] > -1)
        //{
        //    Inventories.GetSlot(Inventories.GetSwitchedIndex(LastIndex[BulletType, (int)SelectableType])).SetEmark(false);
        //}

        //if (LastIndex[BulletType, (int)SelectableType] != -1 && Selected[BulletType, (int)SelectableType] == LastIndex[BulletType, (int)SelectableType])
        //    Selected[BulletType, (int)SelectableType] = -1;
        
        //for(int i = 0; i < 3; i++)
        //{
        //    if(SlotIndices[i] == BulletType)
        //    {
        //        EquipArea.SetButtons((int)SelectableType, false, OriginalSprite, -1);
        //        PaintGauge(i, (int)SelectableType, 0.0f);
        //        break;
        //    }
        //}

        ////실제 총알에 데이터 적용
        //switch ((int)SelectableType)
        //{
        //    case 0:
        //        GameManager.Inst().UpgManager.BData[BulletType].SetAtk(0);
        //        break;
        //    case 1:
        //        GameManager.Inst().UpgManager.BData[BulletType].SetHp(0);
        //        break;
        //    case 2:
        //        GameManager.Inst().UpgManager.BData[BulletType].SetSpd(0);
        //        break;
        //}
    }

    public void Unequip(int BulletType, int EquipType)
    {
        //EquipArea.SetButtons(EquipType, false, OriginalSprite, -1);

        ////E마크
        //if (LastIndex[BulletType, EquipType] > -1)
        //{
        //    Inventories.GetSlot(Inventories.GetSwitchedIndex(LastIndex[BulletType, EquipType])).SetEmark(false);
        //}

        //if (LastIndex[BulletType, EquipType] != -1 && Selected[BulletType, EquipType] == LastIndex[BulletType, EquipType])
        //    Selected[BulletType, EquipType] = -1;

        //for (int i = 0; i < 3; i++)
        //{
        //    if (SlotIndices[i] == BulletType)
        //    {
        //        EquipArea.SetButtons(EquipType, false, OriginalSprite, -1);
        //        PaintGauge(i, EquipType, 0.0f);
        //        break;
        //    }
        //}

        ////실제 총알에 데이터 적용
        //switch (EquipType)
        //{
        //    case 0:
        //        GameManager.Inst().UpgManager.BData[BulletType].SetAtk(0);
        //        break;
        //    case 1:
        //        GameManager.Inst().UpgManager.BData[BulletType].SetHp(0);
        //        break;
        //    case 2:
        //        GameManager.Inst().UpgManager.BData[BulletType].SetSpd(0);
        //        break;
        //}
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
    }

    public void ResetEquip()
    {
        SwitchWindows.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        SwitchWindows.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-720.0f, 0.0f);
        SwitchWindows.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        SwitchWindows.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(720.0f, 0.0f);

        TargetX = 0.0f;

        Inventories.ResetInventory();
    }
}


/*
 public void Select(int index, int BulletType)
    {
        //if ((int)SelectableType < 0)
        //    return;

        //Player.EqData eq = Player.GetItem(index);
        ////if (GameManager.Inst().UpgManager.GetBData(CurBulletType).GetRarity() < eq.Rarity)
        ////    return;

        //if (eq.Type == (int)SelectableType)
        //{
        //    //Detail에 데이터 적용
        //    for (int i = 0; i < 3; i++)
        //    {
        //        if (SlotIndices[i] == BulletType)
        //        {
        //            //SwitchWindows.transform.GetChild(i).GetComponent<SwitchWindow>()
        //            EquipArea.SetButtons((int)SelectableType, true, eq.Icon, eq.Rarity);
        //            PaintGauge(i, (int)SelectableType, eq.Value);
        //            break;
        //        }
        //    }
        //    //SwitchWindows.transform.GetChild(ShowBulletType).GetComponent<SwitchWindow>().SetButtons((int)SelectableType, true, eq.Icon);

        //    LastIndex[BulletType, (int)SelectableType] = index;

        //    //E마크
        //    //for(int i = 0; i < GameManager.Inst().Player.MAXINVENTORY; i++)
        //    //{
        //    //    InventorySlot slot = Inventories.GetSlot(i);
        //    //    if (slot.GetItemUID() == eq.UID)
        //    //    {
        //    //        slot.SetEmark(true);
        //    //        slot.SetSelected(false);
        //    //        break;
        //    //    }
        //    //}
        //    InventorySlot slot = Inventories.GetSlot(Inventories.GetSwitchedIndex(index));
        //    slot.SetEmark(true);
        //    slot.SetSelected(false);

        //    //PaintGauge(ShowBulletType, (int)SelectableType, eq.Value);            

        //    //실제 총알에 데이터 적용
        //    Selected[BulletType, (int)SelectableType] = index;

        //    switch (SelectableType)
        //    {
        //        case Item_Equipment.EquipmentType.ATTACK:
        //            GameManager.Inst().UpgManager.BData[BulletType].SetAtk((int)eq.Value);
        //            break;
        //        case Item_Equipment.EquipmentType.HP:
        //            GameManager.Inst().UpgManager.BData[BulletType].SetHp((int)eq.Value);
        //            break;
        //        case Item_Equipment.EquipmentType.SPEED:
        //            GameManager.Inst().UpgManager.BData[BulletType].SetSpd((int)eq.Value);
        //            break;
        //    }
        //}

        //SetIsShowingSwitch(false);
    }
     */
