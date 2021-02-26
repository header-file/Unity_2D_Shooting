using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData : MonoBehaviour
{
    public int QuestId;
    public string QuestDesc;
    public int GoalCount;
    public int CurrentCount;
    public QuestManager.QuestType Type;

    public QuestData(int id, string desc, int goal, QuestManager.QuestType type)
    {
        QuestId = id;
        QuestDesc = desc;
        GoalCount = goal;
        CurrentCount = 0;
        Type = type;
    }
}
