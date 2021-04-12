using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqMagnet : Item_ZzinEquipment
{
    void Awake()
    {
        Type = ItemType.ZZINEQUIP;
        EqType = EquipType.MAGNET;
        Icon = GetComponent<SpriteRenderer>().sprite;
    }
}
