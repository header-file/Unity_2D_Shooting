using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Equipment : Item
{
    public enum EquipmentType
    {
        ATTACK = 0,
        RANGE = 1,
        SPEED = 2
    };

    public enum Rarity
    {
        Normal = 0,
        Rare = 1,
        Epic = 2,
        Unique = 3,
    }

    protected EquipmentType EqType;
    protected Rarity Grade;
    protected float Eq_Value;
    protected Sprite Icon;

    public int GetEqType() { return (int)EqType; }
    public int GetRarity() { return (int)Grade; }
    public float GetEqValue() { return Eq_Value; }
    public Sprite GetIcon() { return Icon; }

    public void SetRarity(int rarity) { Grade = (Rarity)rarity; }
    public void SetEqValue(float value) { Eq_Value = value; }
}
