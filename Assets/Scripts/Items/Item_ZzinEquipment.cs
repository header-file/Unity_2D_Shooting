using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_ZzinEquipment : Item
{
    public enum EquipType
    {
        MAGNET = 0,
        HOMING = 1,
        HEAL = 2,
        VAMP = 3,
        SHIELD = 4,
        REVIVE = 5,
        REINFORCE = 6,
        KNOCKBACK = 7,
        SLOW = 8,
    }

    public enum Rarity
    {
        WHITE = 0,
        GREEN = 1,
        BLUE = 2,
        PURPLE = 3,
        YELLOW = 4,
    }

    protected Sprite Icon;
    protected float CoolTime;
    protected EquipType EqType;
    protected Rarity Grade;

    GameObject Pop;


    public Sprite GetIcon() { return Icon; }
    public float GetCoolTime() { return CoolTime; }
    public int GetEqType() { return (int)EqType; }
    public int GetRarity() { return (int)Grade; }

    public void SetValues(float time, int value, int uid)
    {
        SetUID(uid);
        SetValue(value);

        CoolTime = time;
    }

    public int SetGrade(int grade)
    {
        int rand = grade * 100;
        if (grade < 0)
            rand = Random.Range(0, 100);

        if (rand <= GameManager.Inst().GetDropRate(GameManager.Inst().StgManager.Stage, "WHITE") || grade == 0)
        {
            Grade = Rarity.WHITE;

            Pop = GameManager.Inst().ObjManager.MakeObj("EquipPopW");
        }
        else if (rand <= GameManager.Inst().GetDropRate(GameManager.Inst().StgManager.Stage, "GREEN") || grade == 1)
        {
            Grade = Rarity.GREEN;

            Pop = GameManager.Inst().ObjManager.MakeObj("EquipPopG");
        }
        else if (rand <= GameManager.Inst().GetDropRate(GameManager.Inst().StgManager.Stage, "BLUE") || grade == 2)
        {
            Grade = Rarity.BLUE;

            Pop = GameManager.Inst().ObjManager.MakeObj("EquipPopB");
        }
        else if (rand <= GameManager.Inst().GetDropRate(GameManager.Inst().StgManager.Stage, "PURPLE") || grade == 3)
        {
            Grade = Rarity.PURPLE;

            Pop = GameManager.Inst().ObjManager.MakeObj("EquipPopP");
        }
        else if (rand <= GameManager.Inst().GetDropRate(GameManager.Inst().StgManager.Stage, "YELLOW") || grade == 4)
        {
            Grade = Rarity.YELLOW;

            Pop = GameManager.Inst().ObjManager.MakeObj("EquipPopY");
        }

        Pop.transform.position = transform.position;
        Pop.GetComponent<ActivationTimer>().IsStart = true;

        return (int)Grade;
    }
}
