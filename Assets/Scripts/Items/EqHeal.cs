using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqHeal : Item_ZzinEquipment
{
    void Awake()
    {
        Type = ItemType.ZZINEQUIP;
        EqType = EquipType.HEAL;
        Icon = GetComponent<SpriteRenderer>().sprite;
    }
}
