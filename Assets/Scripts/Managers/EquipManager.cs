using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    void Awake()
    {
        GameManager.Inst().EquManager = gameObject.GetComponent<EquipManager>();
    }

    public void Count(int index, int id)
    {
        float time = GameManager.Inst().Player.GetItem(index).CoolTime -= Time.deltaTime;

        if(time <= 0.0f)
            Activate(index, id);
    }

    void Activate(int index, int id)
    {
        switch(GameManager.Inst().Player.GetItem(index).Type)
        {
            case (int)Item_ZzinEquipment.EquipType.MAGNET:
                Item_Resource[] resources = FindObjectsOfType<Item_Resource>();
                for(int i = 0; i < resources.Length; i++)
                {
                    if (resources[i].gameObject.activeSelf)
                        resources[i].BeginAbsorb();
                }
                break;

            case (int)Item_ZzinEquipment.EquipType.HOMING:
                break;

            case (int)Item_ZzinEquipment.EquipType.HEAL:
                break;

            case (int)Item_ZzinEquipment.EquipType.VAMP:
                break;
        }

        GameManager.Inst().Player.GetItem(index).CoolTime = GameManager.Inst().EquipDatas[GameManager.Inst().Player.GetItem(index).Type, GameManager.Inst().Player.GetItem(index).Rarity, 0];
    }
}
