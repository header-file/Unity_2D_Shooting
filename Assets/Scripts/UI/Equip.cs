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
    public GameObject TypeText;
    public GameObject BeforeText;
    public GameObject AfterText;
    
    Player Player;
    Sprite OriginalSprite;
    int SelectableType;
    int[] Indices;
    Color[] GaugeColors;
    int[,] Selected;
    int SwitchBuffer;

    
    public int GetSwitchBuffer() { return SwitchBuffer; }
    public int GetSelected(int currentBulletType) { return Selected[currentBulletType, SelectableType]; }

    public void SetSwichBuffer(int i) { SwitchBuffer = i; }

    void Start()
    {
        SelectableType = -1;
        Indices = new int[GameManager.Inst().Player.MAXINVENTORY + 1];
        for (int i = 0; i <= GameManager.Inst().Player.MAXINVENTORY; i++)
            Indices[i] = -1;

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

        gameObject.SetActive(false);
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

        if (Selected[CurrentBulletType, 0] != -1)
        {
            int value = (int)GameManager.Inst().Player.GetItem(Selected[CurrentBulletType, 0]).Value;

            PaintGauge(0, value);
        }
        if (Selected[CurrentBulletType, 1] != -1)
        {
            int value = (int)GameManager.Inst().Player.GetItem(Selected[CurrentBulletType, 1]).Value;

            PaintGauge(1, value);
        }
        if (Selected[CurrentBulletType, 2] != -1)
        {
            int value = (int)GameManager.Inst().Player.GetItem(Selected[CurrentBulletType, 2]).Value;

            PaintGauge(2, value);
        }

        //스페셜 할 예정
    }

    void ShowEquipped(int CurrentBulletType)
    {
        if (Selected[CurrentBulletType, 0] != -1)
        {
            Sprite img = GameManager.Inst().Player.GetItem(Selected[CurrentBulletType, 0]).Icon;

            Buttons.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            Buttons.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = img;
        }
        if (Selected[CurrentBulletType, 1] != -1)
        {
            Sprite img = GameManager.Inst().Player.GetItem(Selected[CurrentBulletType, 1]).Icon;

            Buttons.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            Buttons.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = img;
        }
        if (Selected[CurrentBulletType, 2] != -1)
        {
            Sprite img = GameManager.Inst().Player.GetItem(Selected[CurrentBulletType, 2]).Icon;

            Buttons.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
            Buttons.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = img;
        }
    }

    void ShowInventory()
    {
        SelectableType = -1;

        Player = GameManager.Inst().Player;
        for (int i = 0; i < Player.MAXINVENTORY; i++)
        {
            Player.EqData eq = Player.GetItem(i);
            if (eq.Icon != null)
            {
                Inventories.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
                Inventories.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);

                Sprite icon = eq.Icon;
                Inventories.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().sprite = icon;
            }
        }
    }

    public void ShowSwitch(int index, int CurrentBulletType)
    {
        Text text = TypeText.GetComponent<Text>();
        switch (SelectableType)
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
        text.color = GaugeColors[SelectableType];

        if (Selected[CurrentBulletType, SelectableType] != -1)
            BeforeText.GetComponent<Text>().text = GameManager.Inst().Player.GetItem(Selected[CurrentBulletType, SelectableType]).Value.ToString();
        else
            BeforeText.GetComponent<Text>().text = "0";
        AfterText.GetComponent<Text>().text = GameManager.Inst().Player.GetItem(Indices[index]).Value.ToString();
    }

    public void SortAsType(int type)
    {
        SelectableType = type;

        int slotCount = 1;

        for (int i = 0; i < Player.MAXINVENTORY; i++)
        {
            Inventories.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
            Inventories.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }

        for (int i = 0; i < Player.MAXINVENTORY; i++)
        {
            Player.EqData eq = Player.GetItem(i);
            if(eq.Type == type)
            {
                Inventories.transform.GetChild(slotCount).GetChild(1).gameObject.SetActive(false);
                Inventories.transform.GetChild(slotCount).GetChild(0).gameObject.SetActive(true);

                Inventories.transform.GetChild(slotCount).GetChild(0).GetChild(0).GetComponent<Image>().sprite = eq.Icon;

                Indices[slotCount] = i;
                slotCount++;
            }
        }

        Inventories.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        Inventories.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
    }

    public void Select(int index, int CurrentBulletType)
    {
        if (SelectableType < 0)
            return;

        Player.EqData eq = Player.GetItem(Indices[index]);
        if(eq.Type == SelectableType)
        {
            Buttons.transform.GetChild(SelectableType).GetChild(0).gameObject.SetActive(false);
            Buttons.transform.GetChild(SelectableType).gameObject.GetComponent<Image>().sprite = eq.Icon;

            PaintGauge(SelectableType, eq.Value);

            //Detail에 데이터 적용

            //실제 총알에 데이터 적용
            //eq.SetEquip(CurrentBulletType);
            Selected[CurrentBulletType, SelectableType] = Indices[index];

            switch (SelectableType)
            {
                case 0:
                    GameManager.Inst().UpgManager.GetBData(CurrentBulletType).SetAtk((int)eq.Value);
                    break;
                case 1:
                    GameManager.Inst().UpgManager.GetBData(CurrentBulletType).SetRng((int)eq.Value);
                    break;
                case 2:
                    GameManager.Inst().UpgManager.GetBData(CurrentBulletType).SetSpd((int)eq.Value);
                    break;
            }
        }
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

    public void Switch(int index, int CurrnetBulletType)
    {
        Select(index, CurrnetBulletType);
    }

    public void Unequip(int CurrentBulletType)
    {
        Buttons.transform.GetChild(SelectableType).GetChild(0).gameObject.SetActive(true);
        Buttons.transform.GetChild(SelectableType).gameObject.GetComponent<Image>().sprite = OriginalSprite;

        for (int i = 1; i <= 10; i++)
            Gauges[SelectableType].transform.GetChild(i - 1).gameObject.GetComponent<Image>().color = Color.white;

        //실제 총알에 데이터 적용
        /*for (int i = 0; i < Player.MAXINVENTORY; i++)
        {
            if (Player.GetItem(i).GetEquip() == CurrentBulletType)
                Player.GetItem(i).SetEquip(-1);
        }*/
        if(Selected[CurrentBulletType, SelectableType] == CurrentBulletType)
        {
            Player.GetItem(Selected[CurrentBulletType, SelectableType]).SetEquip(-1);
            Selected[CurrentBulletType, SelectableType] = -1;
        }            

        switch (SelectableType)
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
