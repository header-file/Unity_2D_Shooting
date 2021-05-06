using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqSlower : Item_ZzinEquipment
{
    void Awake()
    {
        Type = ItemType.ZZINEQUIP;
        EqType = EquipType.SLOW;
        Icon = GetComponent<SpriteRenderer>().sprite;
    }
}
