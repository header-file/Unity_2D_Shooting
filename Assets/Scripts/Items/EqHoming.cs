using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqHoming : Item_ZzinEquipment
{
    void Awake()
    {
        Type = ItemType.ZZINEQUIP;
        EqType = EquipType.HOMING;
        Icon = GetComponent<SpriteRenderer>().sprite;
    }
}
