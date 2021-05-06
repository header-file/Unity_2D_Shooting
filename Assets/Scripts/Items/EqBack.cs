using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqBack : Item_ZzinEquipment
{
    void Awake()
    {
        Type = ItemType.ZZINEQUIP;
        EqType = EquipType.KNOCKBACK;
        Icon = GetComponent<SpriteRenderer>().sprite;
    }
}
