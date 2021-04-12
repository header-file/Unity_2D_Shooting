using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqVamp : Item_ZzinEquipment
{
    void Awake()
    {
        Type = ItemType.ZZINEQUIP;
        EqType = EquipType.VAMP;
        Icon = GetComponent<SpriteRenderer>().sprite;
    }
}
