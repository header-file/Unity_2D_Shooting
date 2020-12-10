using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equip : MonoBehaviour
{
    public GameObject Inventories;
    public GameObject Buttons;
    public GameObject[] Gauges;
    
    Player Player;
    Sprite OriginalSprite;
    Color OriginalColor;
    int SelectableType;
    int[] Indices;
    Color[] GaugeColors;


    void Start()
    {
        SelectableType = -1;
        Indices = new int[GameManager.Inst().Player.MAXINVENTORY];
        for (int i = 0; i < GameManager.Inst().Player.MAXINVENTORY; i++)
            Indices[i] = -1;

        OriginalColor = Buttons.transform.GetChild(0).gameObject.GetComponent<Image>().color;
        OriginalSprite = Buttons.transform.GetChild(0).gameObject.GetComponent<Image>().sprite;

        GaugeColors = new Color[3];
        GaugeColors[0] = Color.red;
        GaugeColors[1] = Color.blue;
        GaugeColors[2] = Color.green;
    }

    public void Show(int CurrentBulletType)
    {
        ShowEquipDetail(CurrentBulletType);

        ShowEquipped(CurrentBulletType);

        ShowInventory();
    }

    void ShowEquipDetail(int CurrentBulletType)
    {

    }

    void ShowEquipped(int CurrentBulletType)
    {

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
            Buttons.transform.GetChild(SelectableType).gameObject.GetComponent<Image>().color = Color.white;

            for(int i = 1; i <= 10; i++)
            {
                if (eq.Value >= i * 10)
                    Gauges[SelectableType].transform.GetChild(i - 1).gameObject.GetComponent<Image>().color = GaugeColors[SelectableType];
                else
                    Gauges[SelectableType].transform.GetChild(i - 1).gameObject.GetComponent<Image>().color = Color.white;
            }

            //Detail에 데이터 적용

            //실제 총알에 데이터 적용
        }
    }

    public void Unequip()
    {
        Buttons.transform.GetChild(SelectableType).GetChild(0).gameObject.SetActive(true);
        Buttons.transform.GetChild(SelectableType).gameObject.GetComponent<Image>().sprite = OriginalSprite;
        Buttons.transform.GetChild(SelectableType).gameObject.GetComponent<Image>().color = OriginalColor;

        for (int i = 1; i <= 10; i++)
            Gauges[SelectableType].transform.GetChild(i - 1).gameObject.GetComponent<Image>().color = Color.white;
    }
}
