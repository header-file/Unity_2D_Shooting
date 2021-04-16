using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqShield : Item_ZzinEquipment
{
    void Awake()
    {
        Type = ItemType.ZZINEQUIP;
        EqType = EquipType.SHIELD;
        Icon = GetComponent<SpriteRenderer>().sprite;
    }
}
