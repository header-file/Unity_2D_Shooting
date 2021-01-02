using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDetail : MonoBehaviour
{
    public GameObject Icon;
    public GameObject Type;
    public GameObject Value;
    public GameObject[] Grades;


    public void ShowDetail(int index)
    {
        Player.EqData equip = GameManager.Inst().Player.GetItem(index);

        Icon.GetComponent<Image>().sprite = equip.Icon;
        
        int rarity = equip.Rarity;
        SetRarityColor(rarity);

        int type = equip.Type;
        SetTypeName(type);

        float val = equip.Value;
        SetEqValue((int)val);
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

            case Item_Equipment.EquipmentType.RANGE:
                text.text = "RANGE";
                break;

            case Item_Equipment.EquipmentType.SPEED:
                text.text = "SPEED";
                break;
        }
    }

    void SetEqValue(int val)
    {
        Text text = Value.GetComponent<Text>();
        text.text = val.ToString();
    }
}
