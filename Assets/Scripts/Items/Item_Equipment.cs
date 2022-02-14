using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Equipment : Item
{
    public enum EquipmentType
    {
        ATTACK = 0,
        HP = 1,
        SPEED = 2
    };

    public enum Rarity
    {
        WHITE = 0,
        GREEN = 1,
        BLUE = 2,
        PURPLE = 3,
        YELLOW = 4,
    }

    protected EquipmentType EqType;
    protected Rarity Grade;
    protected float Eq_Value;
    protected Sprite Icon;

    GameObject Pop;

    public int GetEqType() { return (int)EqType; }
    public int GetRarity() { return (int)Grade; }
    public float GetEqValue() { return Eq_Value; }
    public Sprite GetIcon() { return Icon; }

    public void SetEqType(int type) { EqType = (EquipmentType)type; } 
    public void SetRarity(int rarity) { Grade = (Rarity)rarity; }
    //public void SetEqValue(float value) { Eq_Value = value; }
    public void SetIcon(Sprite img) { Icon = img; }

    public void SetValues(int grade, int UID, int type)
    {
        Icon = GetComponent<SpriteRenderer>().sprite;
        EqType = (EquipmentType)type;

        SetUID(UID);
        SetGrade(grade);
        Eq_Value = 1;
    }

    void SetGrade(int grade)
    {
        //SetEqValue(0);
        Eq_Value = 1;
    }

    void SetEqValue(int num)
    {
        switch(num)
        {
            case 0:
                Eq_Value = 1;
                break;

            case 1:
                Eq_Value = 5;
                break;

            case 2:
                Eq_Value = 20;
                break;

            case 3:
                Eq_Value = 75;
                break;

            case 4:
                Eq_Value = 250;
                break;
        }
    }

    protected override void Update()
    {
        base.Update();
    }
}
