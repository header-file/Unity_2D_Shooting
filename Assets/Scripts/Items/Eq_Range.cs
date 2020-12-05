using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_Range : Item_Equipment
{
    void Start()
    {
        Type = ItemType.EQUIPMENT;
        EqType = EquipmentType.RANGE;
        GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", new Color(0.0f, 0.04103366f, 1.670157f));
    }
}
