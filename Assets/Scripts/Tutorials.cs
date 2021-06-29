using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorials : MonoBehaviour
{
    public struct TutorialData
    {
        public int Step;
        public Vector2 Size;
        public Vector2 Pos;
        public Vector2 TxtSize;
        public Vector2 TxtPos;
        public string Txt;
    }

    List<TutorialData> TutDatas;
    int Step;


    void Awake()
    {
        Step = 0;
        TutDatas = new List<TutorialData>();
        List<Dictionary<string, object>> data = CSVReader.Read("Datas/TutorialData");
        for(int i = 0; i < data.Count; i++)
        {
            TutorialData tData = new TutorialData();
            tData.Step = int.Parse(data[i]["Step"].ToString());
            tData.Size = new Vector2(int.Parse(data[i]["SizeX"].ToString()), int.Parse(data[i]["SizeY"].ToString()));
            tData.Pos = new Vector2(int.Parse(data[i]["PosX"].ToString()), int.Parse(data[i]["PosY"].ToString()));
            tData.TxtSize = new Vector2(int.Parse(data[i]["TxtSizeX"].ToString()), int.Parse(data[i]["TxtSizeY"].ToString()));
            tData.TxtPos = new Vector2(int.Parse(data[i]["TxtPosX"].ToString()), int.Parse(data[i]["TxtPosY"].ToString()));
            tData.Txt = data[i]["Txt"].ToString();

            TutDatas.Add(tData);
        }
    }

    void Start()
    {
        StartCoroutine(TutorialStart());
    }

    IEnumerator TutorialStart()
    {
        GoStep();
        yield return new WaitWhile(() => Step < 1);

        GoStep();
        yield return new WaitWhile(() => Step < 2);
    }

    void GoStep()
    {
        GameManager.Inst().UiManager.Tutorial.Size = TutDatas[Step].Size;
        GameManager.Inst().UiManager.Tutorial.Pos = TutDatas[Step].Pos;
        GameManager.Inst().UiManager.Tutorial.TxtSize = TutDatas[Step].TxtSize;
        GameManager.Inst().UiManager.Tutorial.TxtPos = TutDatas[Step].TxtPos;
        GameManager.Inst().UiManager.Tutorial.TutorialText.text = TutDatas[Step].Txt;
        GameManager.Inst().UiManager.Tutorial.SetWindow();

        switch (Step)
        {
            case 0:
                break;
            case 1:
                break;
        }
    }

    void AddStep()
    {
        Step++;
    }
}
