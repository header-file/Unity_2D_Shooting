using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        GameManager.Inst().QstManager = gameObject.GetComponent<QuestManager>();

        CurStageQuests = 0;
        FinQuests = 0;

        Quests = new Dictionary<int, QuestData>();
        GenerateData();
        IsOpen = false;
    }

    void Start()
    {
        //LoadQuestData();
        MakeQuestSlot();
        GameManager.Inst().DatManager.GameData.LoadQuests();

        GameManager.Inst().Player.UISetting();

        //if(SceneManager.GetActiveScene().name != "Stage0")
        //    GameManager.Inst().StgManager.BeginStage();
    }

    void GenerateData()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("Datas/QuestData");

        int id = 0;

        for(int i = 0; i < Constants.MAXSTAGES * Constants.MAXQUESTS; i++)
        {
            int qid = int.Parse(data[i]["ID"].ToString());
            string desc = data[i]["Desc"].ToString();
            int goal = int.Parse(data[i]["Goal"].ToString());
            QuestType type = (QuestType)(qid % 10000 / 1000);
            int objType = qid % 1000 / 100;

            Quests.Add(id++, new QuestData(qid, desc, goal, type, objType));
        }
    }

    void LoadQuestData()
    {
        if (GameManager.Inst().DatManager.GameData.Quests == null ||
            GameManager.Inst().DatManager.GameData.Quests.Length == 0)
            return;

        for(int i = 0; i < Constants.MAXSTAGES * Constants.MAXQUESTS; i++)
        {
            if(Quests[i].QuestId == GameManager.Inst().DatManager.GameData.Quests[Constants.QSTDATASIZE * i + 0])
                Quests[i].CurrentCount = GameManager.Inst().DatManager.GameData.Quests[Constants.QSTDATASIZE * i + 1];
        }
    }

    void MakeQuestSlot()
    {
        if(QuestSlots != null && QuestSlots.Count > 0)
        {
            for (int i = 0; i < QuestSlots.Count; i++)
            {
                if (QuestSlots[i] == null)
                    continue;

                QuestSlots[i].transform.SetParent(GameManager.Inst().ObjManager.UIPool.transform, false);
                QuestSlots[i].gameObject.SetActive(false);
            }

            QuestSlots.Clear();
        }

        QuestSlots = new List<QuestSlot>();

        for(int i = 0; i < Quests.Count; i++)
        {
            if (Quests[i].QuestId / 10000 == GameManager.Inst().StgManager.ReachedStage)
            {
                QuestSlot slot = GameManager.Inst().ObjManager.MakeObj("QuestSlot").GetComponent<QuestSlot>();
                slot.Desc.text = Quests[i].QuestDesc;
                slot.Count.text = "0 / " + Quests[i].GoalCount;
                slot.QuestID = Quests[i].QuestId;
                slot.ProgressBar.fillAmount = 0.0f;
                slot.Check.SetActive(false);
                slot.transform.SetParent(GameManager.Inst().UiManager.MainUI.GetSideMenuSlot(GameManager.Inst().StgManager.ReachedStage - 1).ContentTransform, false);

                QuestSlots.Add(slot);
                CurStageQuests++;
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
                        if (Quests[i].CurrentCount <= 0 && value < 0)
                            return;

                        Quests[i].CurrentCount += value;

                        if (Quests[i].CurrentCount > Quests[i].GoalCount)
                            Quests[i].CurrentCount = Quests[i].GoalCount;

                        CheckFinish(i);
                    }
                }
            }
        }
    }

    public void CheckFinish(int index)
    {
        int found = -1;
        for(int i = 0; i < QuestSlots.Count; i++)
        {
            if (QuestSlots[i].QuestID == Quests[index].QuestId)
            {
                found = i;
                QuestSlots[i].Count.text = Quests[index].CurrentCount.ToString() + " / " + Quests[index].GoalCount.ToString();
                QuestSlots[i].ProgressBar.fillAmount = (float)Quests[index].CurrentCount / Quests[index].GoalCount;
            }
        }

        if (found == -1 || QuestSlots[found] == null)
            return;

        if (Quests[index].CurrentCount >= Quests[index].GoalCount)
        {
            if (Quests[index].IsFinish)
                return;

            Quests[index].IsFinish = true;
            FinQuests++;
            QuestSlots[found].Check.SetActive(true);
        }

        //Check All Clear
        if(FinQuests >= CurStageQuests)
        {
            int nextStage = GameManager.Inst().StgManager.Stage + 1;

            OpenNextStage(nextStage);
        }
    }

    public void OpenNextStage(int stage)
    {
        if (stage > Constants.MAXSTAGES)
            return;

        GameManager.Inst().StgManager.ReachedStage = stage;
        GameManager.Inst().StgManager.UnlockStages(stage);
        MakeQuestSlot();
        GameManager.Inst().ResManager.StartCount(stage - 2);
    }
}
