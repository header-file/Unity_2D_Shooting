using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqRevive : Item_ZzinEquipment
{
    void Awake()
    {
        Type = ItemType.ZZINEQUIP;
        EqType = EquipType.REVIVE;
        Icon = GetComponent<SpriteRenderer>().sprite;
    }
}
