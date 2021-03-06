using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public enum QuestType
    {
        KILL = 0,
        FORGE = 1,
        RESOURCE = 2,
    }

    public enum ResourceType
    {
        COIN = 0,
        A = 1,
        B = 2,
        C = 3,
        D = 4,
    }

    public Dictionary<int, QuestData> Quests;
    public bool IsOpen;

    List<QuestSlot> QuestSlots;
    int CurStageQuests;
    int FinQuests;

    void Awake()
    {
        CurStageQuests = 0;
        FinQuests = 0;

        Quests = new Dictionary<int, QuestData>();
        GenerateData();
        IsOpen = false;
    }

    void Start()
    {
        MakeQuestSlot();
    }

    void GenerateData()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("Datas/QuestData");

        int id = 0;

        for(int i = 0; i < StageManager.MAXSTAGES * 4; i++)
        {
            int qid = int.Parse(data[i]["ID"].ToString());
            string desc = data[i]["Desc"].ToString();
            int goal = int.Parse(data[i]["Goal"].ToString());
            QuestType type = (QuestType)(qid % 10000 / 1000);
            int objType = qid % 1000 / 100;

            Quests.Add(id++, new QuestData(qid, desc, goal, type, objType));
            CurStageQuests++;
        }
    }

    void MakeQuestSlot()
    {
        QuestSlots = new List<QuestSlot>();

        for(int i = 0; i < Quests.Count; i++)
        {
            if (Quests[i].QuestId / 10000 == GameManager.Inst().StgManager.Stage)
            {
                QuestSlot slot = GameManager.Inst().ObjManager.MakeObj("QuestSlot").GetComponent<QuestSlot>();
                slot.Desc.text = Quests[i].QuestDesc;
                slot.Count.text = "0 / " + Quests[i].GoalCount;
                slot.QuestID = Quests[i].QuestId;
                slot.Check.SetActive(false);
                slot.transform.SetParent(GameManager.Inst().UiManager.GetSideMenuSlot(GameManager.Inst().StgManager.Stage - 1).GetComponent<SideMenuSlot>().ContentTransform, false);

                QuestSlots.Add(slot);
            }
        }
    }

    public void QuestProgress(int qType, int objType, int value)
    {
        for (int i = 0; i < Quests.Count; i++)
        {
            int id = Quests[i].QuestId;
            if (id / 10000 == GameManager.Inst().StgManager.Stage)
            {
                id %= 10000;
                if(id / 1000 == qType)
                {
                    id %= 1000;
                    if(id / 100 == objType)
                    {
                        Quests[i].CurrentCount += value;
                        
                        CheckFinish(i);
                    }
                }
            }
        }
    }

    void CheckFinish(int index)
    {
        int found = -1;
        for(int i = 0; i < QuestSlots.Count; i++)
        {
            if (QuestSlots[i].QuestID == Quests[index].QuestId)
            {
                found = i;
                QuestSlots[i].Count.text = Quests[index].CurrentCount.ToString() + " / " + Quests[index].GoalCount.ToString();
            }
        }

        if (QuestSlots[found] == null)
            return;

        if (Quests[index].CurrentCount >= Quests[index].GoalCount)
        {
            Quests[index].IsFinish = true;
            FinQuests++;
            QuestSlots[found].Check.SetActive(true);
        }

        //Check All Clear
        if(FinQuests >= CurStageQuests)
        {
            int nextStage = GameManager.Inst().StgManager.Stage + 1;

            if (nextStage > StageManager.MAXSTAGES)
                return;

            GameManager.Inst().UiManager.UnlockStage(nextStage);
            GameManager.Inst().StgManager.UnlockBullet(nextStage);
        }
    }
}
