using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_Speed : Item_Equipment
{
    void Start()
    {
        Type = ItemType.EQUIPMENT;
        EqType = EquipmentType.SPEED;
        GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", new Color(0.0f, 0.670157f, 0.1464125f));
    }
}
