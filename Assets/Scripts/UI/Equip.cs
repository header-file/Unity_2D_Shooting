using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equip : MonoBehaviour
{
    public GameObject Inventories;
    public GameObject Buttons;
    public GameObject[] Gauges;
    public GameObject[] BulletImgs;
    public GameObject CurrentBuletImg;
    public GameObject[] TypeText;
    public GameObject BeforeText;
    public GameObject AfterText;
    public GameObject BulletText;
    
    Player Player;
    Sprite OriginalSprite;
    Item_Equipment.EquipmentType SelectableType;
    //int[] Indices;
    Color[] GaugeColors;
    int[,] Selected;
    int SwitchBuffer;
    bool IsShowingSwitch;
    int SelectedIndex;
    int[,] LastIndex;
    Color GlowColor;
    bool IsBtoB;
    int SwitchableBulletType;

    
    public int GetSwitchBuffer() { return SwitchBuffer; }
    public int GetSelected(int currentBulletType) { return Selected[currentBulletType, (int)(int)SelectableType]; }
    //public int GetIndices(int index) { return Indices[index]; }

    public void SetSwichBuffer(int i) { SwitchBuffer = i; }
    public void SetIsShowingSwitch(bool b) { IsShowingSwitch = b; }
    public void SetIsBtoB(bool b) { IsBtoB = false; }

    public void DisableSelectedSlot() { if(SelectedIndex > -1) Inventories.transform.GetChild(SelectedIndex).GetComponent<InventorySlot>().SetSelected(false); }
    
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
            

        OriginalSprite = Buttons.transform.GetChild(0).gameObject.GetComponent<Image>().sprite;

        GaugeColors = new Color[3];
        GaugeColors[0] = Color.red;
        GaugeColors[1] = new Color(0.5f, 0.0f, 1.0f);
        GaugeColors[2] = new Color(0.1882353f, 0.8862746f, 0.7372549f);

        Selected = new int[Bullet.MAXBULLETS, 3];
        for(int i = 0; i < Bullet.MAXBULLETS; i++)
        {
            Selected[i, 0] = -1;
            Selected[i, 1] = -1;
            Selected[i, 2] = -1;
        }

        SwitchBuffer = -1;
        IsShowingSwitch = false;
        gameObject.SetActive(false);
        LastIndex = new int[Bullet.MAXBULLETS, 3];
        for(int j = 0; j < Bullet.MAXBULLETS; j++)
        {
            for (int i = 0; i < 3; i++)
                LastIndex[j, i] = -1;
        }
        
        GlowColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        IsBtoB = false;
        SelectedIndex = -1;
        SwitchableBulletType = -1;
    }

    void Update()
    {
        if(IsShowingSwitch)
            Flickering();
    }

    void Flickering()
    {
        GlowColor.a -= 0.1f;

        if (GlowColor.a < 0.0f)
            GlowColor.a = 1.0f;

        int before = 0;
        if (Selected[GameManager.Inst().Player.GetBulletType(), (int)SelectableType] > -1)
            before = (int)GameManager.Inst().Player.GetItem(Selected[GameManager.Inst().Player.GetBulletType(), (int)SelectableType]).Value / 10;
        //int after = (int)GameManager.Inst().Player.GetItem(Indices[SelectedIndex]).Value / 10;
        int after = (int)GameManager.Inst().Player.GetItem(SelectedIndex).Value / 10;

        for (int i = 1; i <= 10; i++)
        {
            if (before >= i && after >= i)
                Gauges[(int)SelectableType].transform.GetChild(i - 1).gameObject.GetComponent<Image>().color = GaugeColors[(int)SelectableType];
            else if (before < i && after < i)
                Gauges[(int)SelectableType].transform.GetChild(i - 1).gameObject.GetComponent<Image>().color = Color.white;
            else if (before < i || after < i)
            {
                Color color = GaugeColors[(int)SelectableType];
                color.a = GlowColor.a;
                Gauges[(int)SelectableType].transform.GetChild(i - 1).gameObject.GetComponent<Image>().color = color;
            }
            
        }
    }
    
    public void Show(int CurrentBulletType)
    {
        ShowEquipDetail(CurrentBulletType);

        ShowEquipped(CurrentBulletType);

        ShowInventory();
    }

    void ShowEquipDetail(int CurrentBulletType)
    {
        CurrentBuletImg.GetComponent<Image>().sprite = BulletImgs[CurrentBulletType].GetComponent<SpriteRenderer>().sprite;

        if (Selected[CurrentBulletType, 0] > -1)
        {
            int value = (int)GameManager.Inst().Player.GetItem(Selected[CurrentBulletType, 0]).Value;

            PaintGauge(0, value);
        }
        else
            PaintGauge(0, 0);

        if (Selected[CurrentBulletType, 1] > -1)
        {
            int value = (int)GameManager.Inst().Player.GetItem(Selected[CurrentBulletType, 1]).Value;

            PaintGauge(1, value);
        }
        else
            PaintGauge(1, 0);

        if (Selected[CurrentBulletType, 2] > -1)
        {
            int value = (int)GameManager.Inst().Player.GetItem(Selected[CurrentBulletType, 2]).Value;

            PaintGauge(2, value);
        }
        else
            PaintGauge(2, 0);

        //스페셜 할 예정
    }

    void ShowEquipped(int CurrentBulletType)
    {
        if (Selected[CurrentBulletType, 0] > -1)
        {
            Sprite img = GameManager.Inst().Player.GetItem(Selected[CurrentBulletType, 0]).Icon;
            
            Buttons.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            Buttons.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = img;
        }
        else
        {
            Buttons.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            Buttons.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = OriginalSprite;
        }

        if (Selected[CurrentBulletType, 1] > -1)
        {
            Sprite img = GameManager.Inst().Player.GetItem(Selected[CurrentBulletType, 1]).Icon;

            Buttons.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            Buttons.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = img;
        }
        else
        {
            Buttons.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            Buttons.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = OriginalSprite;
        }

        if (Selected[CurrentBulletType, 2] > -1)
        {
            Sprite img = GameManager.Inst().Player.GetItem(Selected[CurrentBulletType, 2]).Icon;

            Buttons.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
            Buttons.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = img;
        }
        else
        {
            Buttons.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
            Buttons.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = OriginalSprite;
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
                        slot.GetIcon().transform.rotation = Quaternion.Euler(0.0f, 0.0f,  60.0f);
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

    public void ShowSwitch(int index, int CurrentBulletType)
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
        
        if (Selected[CurrentBulletType, (int)SelectableType] != -1)
            BeforeText.GetComponent<Text>().text = GameManager.Inst().Player.GetItem(Selected[CurrentBulletType, (int)SelectableType]).Value.ToString();
        else
            BeforeText.GetComponent<Text>().text = "0";
        //AfterText.GetComponent<Text>().text = GameManager.Inst().Player.GetItem(Indices[index]).Value.ToString();
        AfterText.GetComponent<Text>().text = GameManager.Inst().Player.GetItem(index).Value.ToString();

        //다른 Bullet이 착용중일 시
        IsBtoB = false;
        BulletText.GetComponent<Text>().text = "";
        for (int i = 0; i < Bullet.MAXBULLETS; i++)
        {
            //if(Selected[i, (int)SelectableType] == Indices[index])
            if (Selected[i, (int)SelectableType] == index)
            {
                BulletText.GetComponent<Text>().text = GameManager.Inst().TxtManager.GetBNames(i);
                SwitchableBulletType = i;
                IsBtoB = true;
                break;
            }
        }
        if(SelectedIndex > -1)
            Inventories.transform.GetChild(SelectedIndex).GetComponent<InventorySlot>().SetSelected(false);
        SelectedIndex = index;
        Inventories.transform.GetChild(index).GetComponent<InventorySlot>().SetSelected(true);
        IsShowingSwitch = true;
    }

    //public void SortAsType(int type)
    //{
    //    (int)SelectableType = type;

    //    SetIsShowingSwitch(false);

    //    int slotCount = 1;

    //    for (int i = 0; i < Player.MAXINVENTORY; i++)
    //    {
    //        Inventories.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
    //        Inventories.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
    //    }

    //    for (int i = 0; i < Player.MAXINVENTORY; i++)
    //    {
    //        //E마크
    //        Inventories.transform.GetChild(i).GetComponent<InventorySlot>().SetEmark(false);

    //        Player.EqData eq = Player.GetItem(i);
    //        if(eq.Type == type)
    //        {
    //            Inventories.transform.GetChild(slotCount).GetChild(1).gameObject.SetActive(false);
    //            Inventories.transform.GetChild(slotCount).GetChild(0).gameObject.SetActive(true);

    //            InventorySlot slot = Inventories.transform.GetChild(slotCount).GetComponent<InventorySlot>();
    //            slot.SetIcon(eq.Icon);

    //            switch (eq.Type)
    //            {
    //                case 0:
    //                    slot.GetIcon().transform.rotation = Quaternion.Euler(0.0f, 0.0f, 60.0f);
    //                    break;
    //                case 1:
    //                    slot.GetIcon().transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    //                    break;
    //                case 2:
    //                    slot.GetIcon().transform.rotation = Quaternion.Euler(0.0f, 0.0f, -60.0f);
    //                    break;
    //            }
    //            for(int j = 0; j < Bullet.MAXBULLETS; j++)
    //            {
    //                if (Selected[j, (int)SelectableType] == i)
    //                {
    //                    Inventories.transform.GetChild(slotCount).GetComponent<InventorySlot>().SetEmark(true);
    //                    break;
    //                }   
    //            }


    //            Indices[slotCount] = i;
    //            slotCount++;
    //        }
    //    }

    //    Inventories.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
    //    Inventories.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
    //}
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


    public void Select(int index, int CurrentBulletType)
    {
        if ((int)SelectableType < 0)
            return;

        Player.EqData eq = Player.GetItem(index);
        if (eq.Type == (int)SelectableType)
        {
            Buttons.transform.GetChild((int)SelectableType).GetChild(0).gameObject.SetActive(false);
            Buttons.transform.GetChild((int)SelectableType).gameObject.GetComponent<Image>().sprite = eq.Icon;
            
            //E마크
            InventorySlot slot = Inventories.transform.GetChild(index).GetComponent<InventorySlot>();
            slot.SetEmark(true);
            slot.SetSelected(false);
            LastIndex[CurrentBulletType, (int)SelectableType] = index;

            PaintGauge((int)SelectableType, eq.Value);

            //Detail에 데이터 적용

            //실제 총알에 데이터 적용
            //eq.SetEquip(CurrentBulletType);
            //Selected[CurrentBulletType, (int)SelectableType] = Indices[index];
            Selected[CurrentBulletType, (int)SelectableType] = index;
            Debug.Log(Selected[CurrentBulletType, (int)SelectableType]);
            Debug.Log(CurrentBulletType);

            switch (SelectableType)
            {
                case Item_Equipment.EquipmentType.ATTACK:
                    GameManager.Inst().UpgManager.GetBData(CurrentBulletType).SetAtk((int)eq.Value);
                    break;
                case Item_Equipment.EquipmentType.RANGE:
                    GameManager.Inst().UpgManager.GetBData(CurrentBulletType).SetRng((int)eq.Value);
                    break;
                case Item_Equipment.EquipmentType.SPEED:
                    GameManager.Inst().UpgManager.GetBData(CurrentBulletType).SetSpd((int)eq.Value);
                    break;
            }
        }

        SetIsShowingSwitch(false);
    }

    void PaintGauge(int type, float value)
    {
        for (int i = 1; i <= 10; i++)
        {
            if (value >= i * 10)
                Gauges[type].transform.GetChild(i - 1).gameObject.GetComponent<Image>().color = GaugeColors[type];
            else
                Gauges[type].transform.GetChild(i - 1).gameObject.GetComponent<Image>().color = Color.white;
        }
    }

    public void Switch(int index, int CurrentBulletType)
    {
        if (IsBtoB)
            SwitchBtoB(index, CurrentBulletType);
        else
        {
            //E마크
            Inventories.transform.GetChild(LastIndex[CurrentBulletType, (int)SelectableType]).GetComponent<InventorySlot>().SetEmark(false);

            Select(index, CurrentBulletType);
        }
    }

    public void SwitchCancel()
    {
        Inventories.transform.GetChild(SelectedIndex).GetComponent<InventorySlot>().SetSelected(false);
        if (Selected[GameManager.Inst().Player.GetBulletType(), (int)SelectableType] > -1)
            PaintGauge((int)SelectableType, GameManager.Inst().Player.GetItem(Selected[GameManager.Inst().Player.GetBulletType(), (int)SelectableType]).Value);
        else
            PaintGauge((int)SelectableType, 0);
        SetIsShowingSwitch(false);
        IsBtoB = false;
    }

    public bool CheckAlreadyEquip(int index)
    {
        for(int i = 0; i < Bullet.MAXBULLETS; i++)
        {
            if (Selected[i, (int)SelectableType] == index)
                return true;
        }

        SwitchableBulletType = -1;
        return false;
    }

    public void SwitchBtoB(int index, int CurrentBulletType)
    {
        if(Selected[CurrentBulletType, (int)SelectableType] == -1)
        {
            Unequip(SwitchableBulletType);
            Select(index, CurrentBulletType);
        }
        else
        {
            Select(Selected[CurrentBulletType, (int)SelectableType], SwitchableBulletType);
            Select(index, CurrentBulletType);
        }
    }

    public void Unequip(int CurrentBulletType)
    {
        Buttons.transform.GetChild((int)SelectableType).GetChild(0).gameObject.SetActive(true);
        Buttons.transform.GetChild((int)SelectableType).gameObject.GetComponent<Image>().sprite = OriginalSprite;

        //E마크
        if(LastIndex[CurrentBulletType, (int)SelectableType] > -1)
            Inventories.transform.GetChild(LastIndex[CurrentBulletType, (int)SelectableType]).GetComponent<InventorySlot>().SetEmark(false);

        for (int i = 1; i <= 10; i++)
            Gauges[(int)SelectableType].transform.GetChild(i - 1).gameObject.GetComponent<Image>().color = Color.white;

        //if(Selected[CurrentBulletType, (int)SelectableType] == Indices[LastIndex])
        if (LastIndex[CurrentBulletType, (int)SelectableType] != -1 && Selected[CurrentBulletType, (int)SelectableType] == LastIndex[CurrentBulletType, (int)SelectableType])
            Selected[CurrentBulletType, (int)SelectableType] = -1;

        //실제 총알에 데이터 적용
        switch ((int)SelectableType)
        {
            case 0:
                GameManager.Inst().UpgManager.GetBData(CurrentBulletType).SetAtk(0);
                break;
            case 1:
                GameManager.Inst().UpgManager.GetBData(CurrentBulletType).SetRng(0);
                break;
            case 2:
                GameManager.Inst().UpgManager.GetBData(CurrentBulletType).SetSpd(0);
                break;
        }
    }
}
