using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData
{
    public int QuestId;
    public string QuestDesc;
    public int GoalCount;
    public int CurrentCount;
    public QuestManager.QuestType Type;
    public int ObjectType;
    public bool IsFinish;

    public QuestData(int id, string desc, int goal, QuestManager.QuestType type, int obj)
    {
        QuestId = id;
        QuestDesc = desc;
        GoalCount = goal;
        CurrentCount = 0;
        Type = type;
        ObjectType = obj;
        IsFinish = false;
    }
}
