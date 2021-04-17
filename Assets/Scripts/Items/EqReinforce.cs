using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqReinforce : Item_ZzinEquipment
{
    void Awake()
    {
        Type = ItemType.ZZINEQUIP;
        EqType = EquipType.REINFORCE;
        Icon = GetComponent<SpriteRenderer>().sprite;
    }
}
