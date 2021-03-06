﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public GameObject EnemyFin;
    public GameObject ZzinBottomBlock;
    public GameObject WeaponInventoryBlock;
    public int Step;

    List<TutorialData> TutDatas;


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

        GameManager.Inst().Tutorials = gameObject.GetComponent<Tutorials>();
    }

    void Start()
    {
        StartCoroutine(TutorialStart());
    }

    IEnumerator TutorialStart()
    {
        int goal = Step + 1;

        while(true)
        {
            if (goal > TutDatas.Count)
                break;

            GoStep();
            yield return new WaitWhile(() => Step < goal);
            goal++;
        }
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
            case 1:
                GameManager.Inst().IptManager.SetIsAbleControl(false);
                Invoke("SetAbleControl", 2.0f);
                EnemySpawn(0);
                break;
            case 4:
                Invoke("ToggleResUI", 3.0f);
                GameManager.Inst().IptManager.SetIsAbleControl(false);
                EnemySpawn(0);
                break;
            case 5:
                GameManager.Inst().IptManager.SetIsAbleControl(true);
                break;
            case 6:
            case 10:
            case 19:
            case 20:
            case 21:
            case 24:
            case 28:
            case 33:
            case 37:
            case 42:
            case 50:
            case 55:
            case 60:
            case 61:
            case 62:
            case 63:
            case 65:
            case 68:
            case 70:
            case 71:
                Invoke("AddStep", 5.0f);
                break;
            case 9:
                GameManager.Inst().Player.AddCoin(1000);
                break;
            case 11:
                GameManager.Inst().UiManager.OnClickManageCancel();
                ZzinBottomBlock.SetActive(true);
                break;
            case 12:
            case 13:
            case 14:
            case 15:
            case 16:
            case 17:
                Invoke("AddStep", 2.0f);
                break;
            case 18:
                ZzinBottomBlock.SetActive(false);
                break;
            case 25:
                GameManager.Inst().MakeEquipData(0, 0);
                GameManager.Inst().MakeReinforceData(0, 0);
                Invoke("AddStep", 5.0f);
                break;
            case 29:
                WeaponInventoryBlock.SetActive(true);
                Invoke("AddStep", 5.0f);
                break;
            case 30:
                WeaponInventoryBlock.SetActive(false);
                break;
            case 32:
                GameManager.Inst().Player.AddCoin(20);
                break;
            case 35:
                WeaponInventoryBlock.SetActive(true);
                Invoke("AddStep", 3.0f);
                break;
            case 36:
                WeaponInventoryBlock.SetActive(false);
                break;
            case 39:
                Invoke("ExitWeaponInfo", 5.0f);
                break;
            case 43:
                GameManager.Inst().UiManager.OnClickManageCancel();
                break;
            case 46:
                Invoke("ExitSell", 5.0f);
                break;
            case 64:
                Invoke("FeverMode", 3.0f);
                break;
            case 66:
                GameManager.Inst().StgManager.SetBossCount(1, 31);
                Invoke("AddStep", 5.0f);
                break;
            case 67:
                EnemyFin.SetActive(true);
                EnemySpawn(1);
                break;
            case 69:
                GameManager.Inst().StgManager.SetBossCount(1, 50);
                Invoke("AddStep", 6.0f);
                break;
            case 72:
                GameManager.Inst().StgManager.BossTimer = 0.1f;
                Invoke("AddStep", 5.0f);
                break;
            case 73:
                Invoke("EndTutorial", 5.0f);
                break;
        }
    }

    void AddStep()
    {
        Step++;
    }

    void SetAbleControl()
    {
        GameManager.Inst().IptManager.SetIsAbleControl(true);
    }

    public void EnemySpawn(int type)
    {
        Enemy enemy;
        if(type == 0)
            enemy = GameManager.Inst().ObjManager.MakeObj("EnemyS").gameObject.GetComponent<Enemy>();
        else
            enemy = GameManager.Inst().ObjManager.MakeObj("EnemyL").gameObject.GetComponent<Enemy>();

        Vector3 pos = Vector3.zero;
        pos.y = 11.0f;
        enemy.transform.position = pos;

        Vector3 target = Vector3.zero;
        target.y = -1.0f;
        enemy.SetTargetPosition(target);

        enemy.StartMove();
    }

    void ToggleResUI()
    {
        GameManager.Inst().UiManager.OnClickResourceToggleBtn();
    }

    void ExitWeaponInfo()
    {
        GameManager.Inst().UiManager.EquipArea.GetComponent<SwitchWindow>().OnClickInfoBackBtn();

        AddStep();
    }

    void ExitSell()
    {
        GameManager.Inst().UiManager.InventoryDetail.GetComponent<InventoryDetail>().OnClickNoBtn();
        GameManager.Inst().UiManager.OnClickInventoryDetailBackBtn();

        AddStep();

        for(int i = 0; i < 3; i++)
            GameManager.Inst().MakeEquipData(0, 0);

        for (int i = 0; i < 2; i++)
            GameManager.Inst().MakeReinforceData(0, 0);

        GameManager.Inst().MakeEquipData(1, 0);
    }

    void FeverMode()
    {
        GameManager.Inst().StgManager.SetBossCount(1, 21);
        AddStep();
    }

    public void EndTutorial()
    {
        GameManager.Inst().DatManager.GameData.ResetData();
        GameManager.Inst().DatManager.GameData.LoadData();
        GameManager.Inst().DatManager.GameData.IsTutorial = false;
        SceneManager.LoadScene("Stage1");
    }
}
