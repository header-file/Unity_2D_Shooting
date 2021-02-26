using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public enum QuestType
    {
        BOSSKILL = 0,
        ENEMYKILL = 1,
        FORGE = 2,
        RESOURCE = 3,
    }

    public GameObject Content;
    public GameObject Arrow;

    public Dictionary<int, QuestData> Quests;
    public bool IsOpen;

    List<QuestSlot> QuestSlots;

    void Awake()
    {
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
            QuestType type = (QuestType)(qid % 1000 / 100);

            Quests.Add(id++, new QuestData(qid, desc, goal, type));
        }
    }

    void MakeQuestSlot()
    {
        QuestSlots = new List<QuestSlot>();

        for(int i = 0; i < Quests.Count; i++)
        {
            if (Quests[i].QuestId / 1000 == GameManager.Inst().StgManager.Stage)
            {
                QuestSlot slot = GameManager.Inst().ObjManager.MakeObj("QuestSlot").GetComponent<QuestSlot>();
                slot.Desc.text = Quests[i].QuestDesc;
                slot.Count.text = "0 / " + Quests[i].GoalCount;
                slot.transform.SetParent(Content.transform, false);

                QuestSlots.Add(slot);
            }
        }
    }
}
