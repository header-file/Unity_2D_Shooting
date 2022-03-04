using System.Collections;
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
            tData.Txt = data[i]["Txt"].ToString().Replace("n", "\n");

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
        GameManager.Inst().UiManager.MainUI.Tutorial.gameObject.SetActive(true);

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
        GameManager.Inst().UiManager.MainUI.Tutorial.Size = TutDatas[Step].Size;
        GameManager.Inst().UiManager.MainUI.Tutorial.Pos = TutDatas[Step].Pos;
        GameManager.Inst().UiManager.MainUI.Tutorial.TxtSize = TutDatas[Step].TxtSize;
        GameManager.Inst().UiManager.MainUI.Tutorial.TxtPos = TutDatas[Step].TxtPos;
        GameManager.Inst().UiManager.MainUI.Tutorial.TutorialText.text = TutDatas[Step].Txt;
        GameManager.Inst().UiManager.MainUI.Tutorial.SetWindow();

        switch (Step)
        {
            case 1:
                GameManager.Inst().UiManager.MainUI.Tutorial.Arrows.SetActive(false);
                GameManager.Inst().IptManager.SetIsAbleControl(false);
                Invoke("SetAbleControl", 2.0f);
                EnemySpawn(0);
                break;
            case 4:
                Invoke("ToggleResUI", 2.0f);
                break;
            case 7:
                GameManager.Inst().UiManager.MainUI.Bottom.OnClickManageCancel();
                GameManager.Inst().UiManager.MainUI.Tutorial.EmpImg.raycastTarget = true;
                Invoke("AddStep", 5.0f);
                break;
            case 8:
                //Emp 이미지 변경
                Invoke("AddStep", 5.0f);
                break;
            case 9:
                //Emp 이미지 원래대로 변경
                Invoke("AddStep", 5.0f);
                break;
            case 10:
                Invoke("AddStep", 5.0f);
                break;
            case 11:
                GameManager.Inst().StgManager.SetBossCount(1, 50);
                Invoke("AddStep", 5.0f);
                break;
            case 12:
                GameManager.Inst().UiManager.MainUI.Tutorial.EmpImg.raycastTarget = false;
                GameManager.Inst().StgManager.SetBossCount(1, 100);
                Invoke("AddStep", 5.0f);
                break;
            case 13:
            case 16:
            case 17:
            case 25:
                Invoke("AddStep", 5.0f);
                break;
            case 14:
                Invoke("EndBoss", 5.0f);
                break;
            case 18:
                GameManager.Inst().UiManager.MainUI.SideMenu.OnClickSideBarBackBtn();
                //하단 메뉴 설명 글씨
                GameManager.Inst().UiManager.MainUI.Tutorial.EmpImg.raycastTarget = true;
                Invoke("AddStep", 5.0f);
                break;
            case 19:
                GameManager.Inst().UiManager.MainUI.Tutorial.EmpImg.raycastTarget = false;
                break;
            case 22:
                GameManager.Inst().UiManager.MainUI.Tutorial.EmpImg.raycastTarget = true;
                Invoke("AddStep", 5.0f);
                break;
            case 23:
                //Emp 이미지 변경
                //메뉴 설명 글씨
                Invoke("AddStep", 5.0f);
                break;
            case 24:
                //Emp 이미지 원래대로 변경
                GameManager.Inst().MakeEquipData(0, 0);
                GameManager.Inst().MakeEquipData(1, 0);
                GameManager.Inst().MakeEquipData(2, 0);
                GameManager.Inst().UiManager.MainUI.Tutorial.EmpImg.raycastTarget = false;
                break;
            case 26:
                GameManager.Inst().UiManager.MainUI.Tutorial.EmpImg.raycastTarget = false;
                break;
            case 29:
                GameManager.Inst().UiManager.MainUI.Tutorial.EmpImg.raycastTarget = true;
                Invoke("AddStep", 5.0f);
                break;
            case 30:
                GameManager.Inst().UiManager.MainUI.Tutorial.EmpImg.raycastTarget = false;
                break;
            case 31:
                Invoke("EndTutorial", 5.0f);
                break;
        }
    }

    public void AddStep()
    {
        Step++;
    }

    public void SetFeverGauge()
    {
        GameManager.Inst().StgManager.SetFever(0, 0, 35.0f, 65.0f);
        GameManager.Inst().StgManager.SetFever(0, 1, 0.0f, 0.0f);
        GameManager.Inst().StgManager.SetFever(0, 2, 0.0f, 0.0f);
        GameManager.Inst().StgManager.SetFeverGauge();
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
        GameManager.Inst().UiManager.MainUI.OnClickResourceToggleBtn();
    }

    void ExitSell()
    {
        GameManager.Inst().UiManager.MainUI.Center.Inventory.InventoryDetail.OnClickNoBtn();
        GameManager.Inst().UiManager.MainUI.Center.Inventory.OnClickInventoryDetailBackBtn();

        AddStep();

        for(int i = 0; i < 3; i++)
            GameManager.Inst().MakeEquipData(0, 0);

        for (int i = 0; i < 2; i++)
            GameManager.Inst().MakeReinforceData(0, 0);

        GameManager.Inst().MakeEquipData(1, 0);
    }

    void CloseAndNext()
    {
        GameManager.Inst().UiManager.MainUI.Center.Weapon.ReinforceArea.OnClickInfoBack();
        GameManager.Inst().UiManager.MainUI.Tutorial.EmpImg.raycastTarget = false;
        AddStep();
    }

    void EndBoss()
    {
        GameManager.Inst().StgManager.BossTimer = 0.1f;
        AddStep();
    }

    public void EndTutorial()
    {
        GameManager.Inst().IsTutorial = false;
        GameManager.Inst().DatManager.GameData.LoadData();
        //GameManager.Inst().DatManager.GameData.IsTutorial = false;
        SceneManager.LoadScene("Stage" + GameManager.Inst().DatManager.GameData.BeforeStage.ToString());
    }
}
