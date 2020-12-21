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

    public int GetEqType() { return (int)EqType; }
    public int GetRarity() { return (int)Grade; }
    public float GetEqValue() { return Eq_Value; }
    public Sprite GetIcon() { return Icon; }

    public void SetEqType(int type) { EqType = (EquipmentType)type; } 
    public void SetRarity(int rarity) { Grade = (Rarity)rarity; }
    public void SetEqValue(float value) { Eq_Value = value; }
    public void SetIcon(Sprite img) { Icon = img; }

    public void SetValues()
    {
        Icon = GetComponent<SpriteRenderer>().sprite;

        SetGrade();
    }

    void SetGrade()
    {
        int rand = Random.Range(0, 1);
        switch(rand)
        {
            case 0:
                Grade = Rarity.WHITE;
                SetEqValue(rand);
                break;

            case 1:
                Grade = Rarity.GREEN;
                SetEqValue(rand);
                break;

            case 2:
                Grade = Rarity.BLUE;
                SetEqValue(rand);
                break;

            case 3:
                Grade = Rarity.PURPLE;
                SetEqValue(rand);
                break;

            case 4:
                Grade = Rarity.YELLOW;
                SetEqValue(rand);
                break;
        }
    }

    void SetEqValue(int num)
    {
        int min = num * 20 + 1;
        int max = (num + 1) * 20 + 1;

        int seed = (int)(Time.time * 1000.0f);
        Random.InitState(seed);
        int rand = Random.Range(min, max);
        Eq_Value = rand;
    }
}
