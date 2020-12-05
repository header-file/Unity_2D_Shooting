using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eq_Attack : Item_Equipment
{
    void Start()
    {
        Type = ItemType.EQUIPMENT;
        EqType = EquipmentType.ATTACK;
        GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", new Color(0.670157f, 0.0f, 0.2129757f));
    }
}
