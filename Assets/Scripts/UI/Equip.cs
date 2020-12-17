using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equip : MonoBehaviour
{
    public GameObject Inventories;
    public GameObject[] BulletImgs;
    public GameObject[] TypeText;
    public GameObject BeforeText;
    public GameObject AfterText;
    public GameObject BulletText;
    public GameObject SwitchWindows;
    public GameObject ArrowButtons;
    public Sprite OriginalSprite;

    Player Player;    
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
    int[] SlotIndices;
    float[] SlotPoses;
    float TargetX;

    public int GetSwitchBuffer() { return SwitchBuffer; }
    public int GetSelected(int currentBulletType) { return Selected[currentBulletType, (int)(int)SelectableType]; }

    public void SetSwichBuffer(int i) { SwitchBuffer = i; }
    public void SetIsShowingSwitch(bool b) { IsShowingSwitch = b; }
    public void SetIsBtoB(bool b) { IsBtoB = false; }
    public void SetCurBulletType(int type) { CurBulletType = type; }

    public void DisableSelectedSlot() { if (SelectedIndex > -1) Inventories.transform.GetChild(SelectedIndex).GetComponent<InventorySlot>().SetSelected(false); }

    void Start()
    {
        SelectableType = 0;
        Player = GameManager.Inst().Player;

        int maxInventory = Player.MAXINVENTORY;
        //Indices = new int[maxInventory + 1];
        for (int i = 1; i <= maxInventory; i++)
        {
            //Indices[i] = -1;
            GameObject inventorySlot = GameManager.Inst().ObjManager.MakeObj("InventorySlot");
            //inventorySlot.transform.parent = Inventories.transform;
            inventorySlot.transform.SetParent(Inventories.gameObject.transform, false);
            InventorySlot slot = inventorySlot.GetComponent<InventorySlot>();
            slot.SetIndex(i);
            slot.SetType(1);
        }
        
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
        if (Selected[GameManager.Inst().Player.GetBulletType(), (int)SelectableType] > -1)
            before = (int)GameManager.Inst().Player.GetItem(Selected[GameManager.Inst().Player.GetBulletType(), (int)SelectableType]).Value / 10;

        int after = (int)GameManager.Inst().Player.GetItem(SelectedIndex).Value / 10;

        SwitchWindows.transform.GetChild(1).GetComponent<SwitchWindow>().FlickerGauge(before, after, (int)SelectableType, GaugeColors[(int)SelectableType], GlowColor.a);
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
                int value = (int)GameManager.Inst().Player.GetItem(Selected[BulletType, 0]).Value;

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
                Sprite img = GameManager.Inst().Player.GetItem(Selected[BulletType, 0]).Icon;

                SwitchWindows.transform.GetChild(index).GetComponent<SwitchWindow>().SetButtons(i, true, img);
            }
            else
                SwitchWindows.transform.GetChild(index).GetComponent<SwitchWindow>().SetButtons(i, false, OriginalSprite);
        }
    }

    void ShowInventory()
    {
        SelectableType = 0;

        for (int i = 1; i <= Player.MAXINVENTORY; i++)
        {
            Player.EqData eq = Player.GetItem(i);
            if (eq.Icon != null)
            {
                Inventories.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
                Inventories.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);

                Sprite icon = eq.Icon;
                InventorySlot slot = Inventories.transform.GetChild(i).GetComponent<InventorySlot>();
                slot.SetIcon(icon);

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
        }
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
            Inventories.transform.GetChild(SelectedIndex).GetComponent<InventorySlot>().SetSelected(false);
        SelectedIndex = index;
        Inventories.transform.GetChild(index).GetComponent<InventorySlot>().SetSelected(true);
        IsShowingSwitch = true;
    }

    public void SortAsType(int type)
    {
        SelectableType = (Item_Equipment.EquipmentType)type;

        SetIsShowingSwitch(false);

        for (int i = 1; i <= Player.MAXINVENTORY; i++)
        {
            Player.EqData eq = Player.GetItem(i);
            if (eq.Icon != null)
            {
                if (eq.Type == type)
                {
                    Inventories.transform.GetChild(i).gameObject.SetActive(true);

                    Inventories.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
                    Inventories.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);

                    InventorySlot slot = Inventories.transform.GetChild(i).GetComponent<InventorySlot>();
                    slot.SetIcon(eq.Icon);

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
                    for (int j = 0; j < Bullet.MAXBULLETS; j++)
                    {
                        if (Selected[j, (int)SelectableType] == i)
                        {
                            Inventories.transform.GetChild(i).GetComponent<InventorySlot>().SetEmark(true);
                            break;
                        }
                    }
                }
                else
                    Inventories.transform.GetChild(i).gameObject.SetActive(false);
            }
            else
                break;

        }

        Inventories.transform.GetChild(0).gameObject.SetActive(true);
    }


    public void Select(int index, int BulletType)
    {
        if ((int)SelectableType < 0)
            return;

        Player.EqData eq = Player.GetItem(index);
        if (eq.Type == (int)SelectableType)
        {
            SwitchWindows.transform.GetChild(1).GetComponent<SwitchWindow>().SetButtons((int)SelectableType, true, eq.Icon);

            //E마크
            InventorySlot slot = Inventories.transform.GetChild(index).GetComponent<InventorySlot>();
            slot.SetEmark(true);
            slot.SetSelected(false);
            LastIndex[BulletType, (int)SelectableType] = index;

            //Detail에 데이터 적용
            PaintGauge(1, (int)SelectableType, eq.Value);            

            //실제 총알에 데이터 적용
            Selected[BulletType, (int)SelectableType] = index;
            Debug.Log(Selected[BulletType, (int)SelectableType]);
            Debug.Log(BulletType);

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
            Inventories.transform.GetChild(LastIndex[BulletType, (int)SelectableType]).GetComponent<InventorySlot>().SetEmark(false);

            Select(index, BulletType);
        }
    }

    public void SwitchCancel()
    {
        Inventories.transform.GetChild(SelectedIndex).GetComponent<InventorySlot>().SetSelected(false);
        if (Selected[CurBulletType, (int)SelectableType] > -1)
            PaintGauge(1, (int)SelectableType, GameManager.Inst().Player.GetItem(Selected[CurBulletType, (int)SelectableType]).Value);
        else
            PaintGauge(1, (int)SelectableType, 0);
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
        SwitchWindows.transform.GetChild(1).GetComponent<SwitchWindow>().SetButtons((int)SelectableType, false, OriginalSprite);

        //E마크
        if (LastIndex[BulletType, (int)SelectableType] > -1)
            Inventories.transform.GetChild(LastIndex[BulletType, (int)SelectableType]).GetComponent<InventorySlot>().SetEmark(false);

        if (LastIndex[BulletType, (int)SelectableType] != -1 && Selected[BulletType, (int)SelectableType] == LastIndex[BulletType, (int)SelectableType])
            Selected[BulletType, (int)SelectableType] = -1;

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

    public void Next(bool IsNext)
    {
        IsMoving = true;

        if (IsNext)
        {
            CurBulletType++;
            TargetX -= 720.0f;

            if (CurBulletType >= Bullet.MAXBULLETS)
                CurBulletType = 0;
        }
        else
        {
            CurBulletType--;
            TargetX += 720.0f;

            if (CurBulletType < 0)
                CurBulletType = Bullet.MAXBULLETS - 1;
        }

        //for (int i = 0; i < 3; i++)
        //{
        //    int type = CurBulletType + (i - 1);
        //    JudgeSlotIndices(i, type);
        //}

        for (int i = 0; i < 2; i++)
            ArrowButtons.transform.GetChild(i).GetComponent<Button>().interactable = false;
    }

    void JudgeSlotIndices(int index, int BulletType)
    {
        if (BulletType >= Bullet.MAXBULLETS)
            BulletType = 0;

        if (BulletType < 0)
            BulletType = Bullet.MAXBULLETS - 1;

        SlotIndices[index] = BulletType;
        Debug.Log(SlotIndices[index]);
    }
}
