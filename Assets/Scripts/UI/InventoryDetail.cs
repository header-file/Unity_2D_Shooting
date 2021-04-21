using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDetail : MonoBehaviour
{
    public GameObject Icon;
    public GameObject Type;
    public Text Value;
    public GameObject[] Grades;
    public Image Frame;
    public GameObject Values;
    public GameObject Detail;
    public Text DetailText;


    public void ShowDetail(int index)
    {
        Player.EqData equip = GameManager.Inst().Player.GetItem(index);

        Icon.GetComponent<Image>().sprite = equip.Icon;
        
        int rarity = equip.Rarity;
        SetRarityColor(rarity);

        int type = equip.Type;
        SetTypeName(type);

        if (equip.UID / 100 == 3)
        {
            int val = (int)equip.Value;
            SetValue(val);
        }
        else if(equip.UID / 100 == 6)
            SetDetail(equip.Type, equip.Rarity, equip.Value);
    }    

    void SetRarityColor(int rarity)
    {
        //Item_Equipment.Rarity rare = (Item_Equipment.Rarity)rarity;
        for (int i = 0; i < 5; i++)
            Grades[i].SetActive(false);

        Grades[rarity].SetActive(true);
    }

    void SetTypeName(int type)
    {
        Item_Equipment.EquipmentType t = (Item_Equipment.EquipmentType)type;
        Text text = Type.GetComponent<Text>();
        switch (t)
        {
            case Item_Equipment.EquipmentType.ATTACK:
                text.text = "ATTACK";
                break;

            case Item_Equipment.EquipmentType.HP:
                text.text = "RANGE";
                break;

            case Item_Equipment.EquipmentType.SPEED:
                text.text = "SPEED";
                break;
        }
    }

    void SetValue(int val)
    {
        Values.SetActive(true);
        Detail.SetActive(false);

        Value.text = val.ToString();
    }

    void SetDetail(int type, int rarity, float value)
    {
        Values.SetActive(false);
        Detail.SetActive(true);

        string detail = "";
        if (GameManager.Inst().EquipDatas[type, rarity, 0] > 0)
            detail += GameManager.Inst().EquipDatas[type, rarity, 0].ToString();
        detail += GameManager.Inst().TxtManager.EquipDetailFront[type];
        if (value > 0)
            detail += value.ToString();
        detail += GameManager.Inst().TxtManager.EquipDetailBack[type];

        DetailText.text = detail;
    }
}
