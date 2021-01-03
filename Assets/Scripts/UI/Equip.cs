using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equip : MonoBehaviour
{
    public GameObject InventoryArea;
    public GameObject[] BulletImgs;
    public GameObject[] TypeText;
    public GameObject BeforeText;
    public GameObject AfterText;
    public GameObject BulletText;
    public GameObject SwitchWindows;
    public GameObject ArrowButtons;
    public Sprite OriginalSprite;

    Player Player;
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
    public int GetSelected(int currentBulletType) { return Selected[currentBulletType, (int)(int)SelectableType]; }
    public int GetCurBulletType() { return CurBulletType; }

    public void SetSwichBuffer(int i) { SwitchBuffer = i; }
    public void SetIsShowingSwitch(bool b) { IsShowingSwitch = b; }
    public void SetIsBtoB(bool b) { IsBtoB = false; }
    public void SetCurBulletType(int type) { CurBulletType = type; }

    public void DisableSelectedSlot()
    {
        if (SelectedIndex <= -1)
            return;

        for (int i = 0; i < GameManager.Inst().Player.MAXINVENTORY; i++)
        {
            if (Inventories.GetSlot(i).GetItemUID() == GameManager.Inst().Player.GetItem(SelectedIndex).UID)
                Inventories.GetSlot(i).SetSelected(false);
        }
        
    }

    void Start()
    {
        SelectableType = 0;
        Player = GameManager.Inst().Player;

        int maxInventory = Player.MAXINVENTORY;
        //Indices = new int[maxInventory + 1];
        /*for (int i = 1; i <= maxInventory; i++)
        {
            //Indices[i] = -1;
            GameObject inventorySlot = GameManager.Inst().ObjManager.MakeObj("InventorySlot");
            //inventorySlot.transform.parent = Inventories.transform;
            inventorySlot.transform.SetParent(Inventories.gameObject.transform, false);
            InventorySlot slot = inventorySlot.GetComponent<InventorySlot>();
            slot.SetIndex(i);
            slot.SetType(1);
        }*/
        
        GaugeColors = new Color[3];
        GaugeColors[0] = Color.red;
        GaugeColors[1] = new Color(0.5f, 0.0f, 1.0f);
        GaugeColors[2] = new Color(0.1882353f, 0.8862746f, 0.7372549f);

        Selected = new int[Bullet.MAXBULLETS, 3];
        for (int i = 0; i < Bullet.MAXBULLETS; i++)
        {
            Selected[i, 0] = -1;
            Selected[i, 1] = -1;
            Selected[i, 2] = -1;
        }

        SwitchBuffer = -1;
        IsShowingSwitch = false;
        gameObject.SetActive(false);
        LastIndex = new int[Bullet.MAXBULLETS, 3];
        for (int j = 0; j < Bullet.MAXBULLETS; j++)
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
        SlotIndices[0] = Bullet.MAXBULLETS - 1;
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

        SwitchWindows.transform.GetChild(ShowBulletType).GetComponent<SwitchWindow>().FlickerGauge(before, after, (int)SelectableType, GaugeColors[(int)SelectableType], GlowColor.a);
    }

    void Moving()
    {
        MoveTimer += Time.deltaTime * 5.0f;
        Vector3 pos = SwitchWindows.GetComponent<RectTransform>().anchoredPosition;
        pos.x = Mathf.Lerp(pos.x, TargetX , MoveTimer);

        SwitchWindows.GetComponent<RectTransform>().anchoredPosition = pos;

        if(MoveTimer >= 1.0f)
        {
            IsMoving = false;
            MoveTimer = 0.0f;

            for (int i = 0; i < 2; i++)
                ArrowButtons.transform.GetChild(i).GetComponent<Button>().interactable = true;

            for (int i = 0; i < 3; i++)
            {
                if (SwitchWindows.transform.GetChild(i).position.x > 6.0f)
                {
                    Vector2 newPos = SwitchWindows.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition;
                    newPos.x -= 720.0f * 3;

                    SwitchWindows.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = newPos;

                    SlotIndices[i] += 4;
                    if (SlotIndices[i] >= Bullet.MAXBULLETS)
                        SlotIndices[i] -= Bullet.MAXBULLETS;
                    Show(SlotIndices[i], i);
                }

                if (SwitchWindows.transform.GetChild(i).position.x < -6.0f)
                {
                    Vector2 newPos = SwitchWindows.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition;
                    newPos.x += 720.0f * 3;

                    SwitchWindows.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = newPos;

                    SlotIndices[i] -= 4;
                    if (SlotIndices[i] < 0)
                        SlotIndices[i] += Bullet.MAXBULLETS;
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
            SlotIndices[0] = Bullet.MAXBULLETS - 1;
        SlotIndices[2] = CurBulletType + 1;
        if (SlotIndices[2] >= Bullet.MAXBULLETS)
            SlotIndices[2] = 0;

        for (int i = 0; i < 3; i++)
            Show(SlotIndices[i], i);

        ShowInventory();
    }

    void Show(int BulletType, int index)
    {
        ShowEquipDetail(BulletType, index);

        ShowEquipped(BulletType, index);
    }

    void ShowEquipDetail(int BulletType, int index)
    {
        SwitchWindows.transform.GetChild(index).GetComponent<SwitchWindow>().SetCurrentBulletImg(BulletImgs[BulletType].GetComponent<SpriteRenderer>().sprite);

        for(int i = 0; i < 3; i++)
        {
            if (Selected[BulletType, i] > -1)
            {
                int value = (int)GameManager.Inst().Player.GetItem(Selected[BulletType, i]).Value;

                PaintGauge(index, i, value);
            }
            else
                PaintGauge(index, i, 0);
        }

        //스페셜 할 예정
    }

    void ShowEquipped(int BulletType, int index)
    {
        for (int i = 0; i < 3; i++)
        {
            if (Selected[BulletType, i] > -1)
            {
                Sprite img = GameManager.Inst().Player.GetItem(Selected[BulletType, i]).Icon;

                SwitchWindows.transform.GetChild(index).GetComponent<SwitchWindow>().SetButtons(i, true, img);
            }
            else
                SwitchWindows.transform.GetChild(index).GetComponent<SwitchWindow>().SetButtons(i, false, OriginalSprite);
        }
    }

    void ShowInventory()
    {
        SelectableType = 0;

        Inventories = GameManager.Inst().Inventory.GetComponent<InventoryScroll>();
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
        for (int i = 0; i < Bullet.MAXBULLETS; i++)
        {
            if (Selected[i, (int)SelectableType] == index)
            {
                BulletText.GetComponent<Text>().text = GameManager.Inst().TxtManager.GetBNames(i);
                SwitchableBulletType = i;
                IsBtoB = true;
                break;
            }
        }
        if (SelectedIndex > -1)
        {
            for (int i = 0; i < GameManager.Inst().Player.MAXINVENTORY; i++)
            {
                if (Inventories.GetSlot(i).GetItemUID() == GameManager.Inst().Player.GetItem(SelectedIndex).UID)
                    Inventories.GetSlot(i).SetSelected(false);
            }
        }

        SelectedIndex = index;

        for(int i = 0; i < GameManager.Inst().Player.MAXINVENTORY; i++)
        {
            if(Inventories.GetSlot(i).GetItemUID() == GameManager.Inst().Player.GetItem(index).UID)
                Inventories.GetSlot(i).SetSelected(true);
        }
        
        IsShowingSwitch = true;
    }

    public void SortAsType(int type)
    {
        SelectableType = (Item_Equipment.EquipmentType)type;

        SetIsShowingSwitch(false);

        Inventories.ResetInventory();

        for (int i = 0; i < Player.MAXINVENTORY; i++)
        {
            Player.EqData eq = Player.GetItem(i);
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
        GameManager.Inst().Player.InputGrade = (type + 11);

        Inventories.Sort();

        Inventories.None.SetActive(true);
    }


    public void Select(int index, int BulletType)
    {
        if ((int)SelectableType < 0)
            return;

        Player.EqData eq = Player.GetItem(index);
        if (eq.Type == (int)SelectableType)
        {
            //Detail에 데이터 적용
            for (int i = 0; i < 3; i++)
            {
                if (SlotIndices[i] == BulletType)
                {
                    SwitchWindows.transform.GetChild(i).GetComponent<SwitchWindow>().SetButtons((int)SelectableType, true, eq.Icon);
                    PaintGauge(i, (int)SelectableType, eq.Value);
                    break;
                }
            }
            //SwitchWindows.transform.GetChild(ShowBulletType).GetComponent<SwitchWindow>().SetButtons((int)SelectableType, true, eq.Icon);

            LastIndex[BulletType, (int)SelectableType] = index;

            //E마크
            for(int i = 0; i < GameManager.Inst().Player.MAXINVENTORY; i++)
            {
                InventorySlot slot = Inventories.GetSlot(i);
                if (slot.GetItemUID() == eq.UID)
                {
                    slot.SetEmark(true);
                    slot.SetSelected(false);
                    break;
                }
            }            

            //PaintGauge(ShowBulletType, (int)SelectableType, eq.Value);            

            //실제 총알에 데이터 적용
            Selected[BulletType, (int)SelectableType] = index;

            switch (SelectableType)
            {
                case Item_Equipment.EquipmentType.ATTACK:
                    GameManager.Inst().UpgManager.GetBData(BulletType).SetAtk((int)eq.Value);
                    break;
                case Item_Equipment.EquipmentType.RANGE:
                    GameManager.Inst().UpgManager.GetBData(BulletType).SetRng((int)eq.Value);
                    break;
                case Item_Equipment.EquipmentType.SPEED:
                    GameManager.Inst().UpgManager.GetBData(BulletType).SetSpd((int)eq.Value);
                    break;
            }
        }

        SetIsShowingSwitch(false);
    }

    void PaintGauge(int index, int type, float value)
    {
        SwitchWindows.transform.GetChild(index).GetComponent<SwitchWindow>().PaintGauge(type, value, GaugeColors[type]);
    }

    public void Switch(int index, int BulletType)
    {
        if (IsBtoB)
            SwitchBtoB(index, BulletType);
        else
        {
            //E마크
            for (int i = 0; i < GameManager.Inst().Player.MAXINVENTORY; i++)
            {
                InventorySlot slot = Inventories.GetSlot(i);
                if (slot.GetItemUID() == GameManager.Inst().Player.GetItem(LastIndex[BulletType, (int)SelectableType]).UID)
                {
                    slot.SetEmark(false);
                    slot.SetSelected(false);
                    break;
                }
            }

            Select(index, BulletType);
        }
    }

    public void SwitchCancel()
    {
        for (int i = 0; i < GameManager.Inst().Player.MAXINVENTORY; i++)
        {
            if (Inventories.GetSlot(i).GetItemUID() == GameManager.Inst().Player.GetItem(SelectedIndex).UID)
                Inventories.GetSlot(i).SetSelected(false);
        }
                
        if (Selected[CurBulletType, (int)SelectableType] > -1)
            PaintGauge(ShowBulletType, (int)SelectableType, GameManager.Inst().Player.GetItem(Selected[CurBulletType, (int)SelectableType]).Value);
        else
            PaintGauge(ShowBulletType, (int)SelectableType, 0);
        SetIsShowingSwitch(false);
        IsBtoB = false;
    }

    public bool CheckAlreadyEquip(int index)
    {
        for (int i = 0; i < Bullet.MAXBULLETS; i++)
        {
            if (Selected[i, (int)SelectableType] == index)
                return true;
        }

        SwitchableBulletType = -1;
        return false;
    }

    public bool CheckAlreadyEquipAll(int index)
    {
        for (int i = 0; i < Bullet.MAXBULLETS; i++)
        {
            for (int j = 0; j < 3; j++)
                if (Selected[i, j] == index)
                    return true;
        }

        return false;
    }

    public int GetBulletType(int index)
    {
        for (int i = 0; i < Bullet.MAXBULLETS; i++)
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
        SwitchWindows.transform.GetChild(ShowBulletType).GetComponent<SwitchWindow>().SetButtons((int)SelectableType, false, OriginalSprite);
        //PaintGauge(ShowBulletType, , 0.0f);

        //E마크
        if (LastIndex[BulletType, (int)SelectableType] > -1)
        {
            for (int i = 0; i < GameManager.Inst().Player.MAXINVENTORY; i++)
            {
                InventorySlot slot = Inventories.GetSlot(i);
                if (slot.GetItemUID() == GameManager.Inst().Player.GetItem(LastIndex[BulletType, (int)SelectableType]).UID)
                {
                    slot.SetEmark(false);
                    break;
                }
            }
        }

        if (LastIndex[BulletType, (int)SelectableType] != -1 && Selected[BulletType, (int)SelectableType] == LastIndex[BulletType, (int)SelectableType])
            Selected[BulletType, (int)SelectableType] = -1;
        
        for(int i = 0; i < 3; i++)
        {
            if(SlotIndices[i] == BulletType)
            {
                SwitchWindows.transform.GetChild(i).GetComponent<SwitchWindow>().SetButtons((int)SelectableType, false, OriginalSprite);
                PaintGauge(i, (int)SelectableType, 0.0f);
                break;
            }
        }

        //실제 총알에 데이터 적용
        switch ((int)SelectableType)
        {
            case 0:
                GameManager.Inst().UpgManager.GetBData(BulletType).SetAtk(0);
                break;
            case 1:
                GameManager.Inst().UpgManager.GetBData(BulletType).SetRng(0);
                break;
            case 2:
                GameManager.Inst().UpgManager.GetBData(BulletType).SetSpd(0);
                break;
        }
    }

    public void Unequip(int BulletType, int EquipType)
    {
        SwitchWindows.transform.GetChild(ShowBulletType).GetComponent<SwitchWindow>().SetButtons(EquipType, false, OriginalSprite);
        //PaintGauge(ShowBulletType, , 0.0f);

        //E마크
        if (LastIndex[BulletType, EquipType] > -1)
        {
            for (int i = 0; i < GameManager.Inst().Player.MAXINVENTORY; i++)
            {
                InventorySlot slot = Inventories.GetSlot(i);
                if (slot.GetItemUID() == GameManager.Inst().Player.GetItem(LastIndex[BulletType, EquipType]).UID)
                {
                    slot.SetEmark(false);
                    break;
                }
            }
        }

        if (LastIndex[BulletType, EquipType] != -1 && Selected[BulletType, EquipType] == LastIndex[BulletType, EquipType])
            Selected[BulletType, EquipType] = -1;

        for (int i = 0; i < 3; i++)
        {
            if (SlotIndices[i] == BulletType)
            {
                SwitchWindows.transform.GetChild(i).GetComponent<SwitchWindow>().SetButtons(EquipType, false, OriginalSprite);
                PaintGauge(i, EquipType, 0.0f);
                break;
            }
        }

        //실제 총알에 데이터 적용
        switch (EquipType)
        {
            case 0:
                GameManager.Inst().UpgManager.GetBData(BulletType).SetAtk(0);
                break;
            case 1:
                GameManager.Inst().UpgManager.GetBData(BulletType).SetRng(0);
                break;
            case 2:
                GameManager.Inst().UpgManager.GetBData(BulletType).SetSpd(0);
                break;
        }
    }

    public void Next(bool IsNext)
    {
        IsMoving = true;

        if (IsNext)
        {
            CurBulletType++;
            ShowBulletType++;
            TargetX -= 720.0f;

            if (CurBulletType >= Bullet.MAXBULLETS)
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
                CurBulletType = Bullet.MAXBULLETS - 1;

            if (ShowBulletType < 0)
                ShowBulletType = 2;
        }
        
        for (int i = 0; i < 2; i++)
            ArrowButtons.transform.GetChild(i).GetComponent<Button>().interactable = false;
    }

    public void Reset()
    {
        SwitchWindows.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        SwitchWindows.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-720.0f, 0.0f);
        SwitchWindows.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        SwitchWindows.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(720.0f, 0.0f);

        TargetX = 0.0f;

        Inventories.ResetInventory();
    }
}
