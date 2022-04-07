using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.DateTime;

public class ResourceManager : MonoBehaviour
{
    public System.DateTime[] GoalTimes;
    public bool[] IsStartCount;
    public bool IsFinish;

    struct ResourceData
    {
        public int hour;
        public int min;
        public int resource;
        public int coin;
        public int jewel;
    }

    ResourceData[] ResourceDatas;
    int[] NowSec;
    string time;


    void Awake()
    {
        ResourceDatas = new ResourceData[Constants.MAXSTAGES];
        List<Dictionary<string, object>> data = CSVReader.Read("Datas/SideMenuData");
        for(int i = 0; i < Constants.MAXSTAGES; i++)
        {
            ResourceDatas[i].hour = int.Parse(data[i]["Hour"].ToString());
            ResourceDatas[i].min = int.Parse(data[i]["Minute"].ToString());
            ResourceDatas[i].resource = int.Parse(data[i]["Resource"].ToString());
            ResourceDatas[i].coin = int.Parse(data[i]["Coin"].ToString());
            ResourceDatas[i].jewel = int.Parse(data[i]["Jewel"].ToString());
        }
        IsFinish = false;
    }

    void Start()
    {
        IsStartCount = new bool[Constants.MAXSTAGES];
        NowSec = new int[Constants.MAXSTAGES];
        GoalTimes = new System.DateTime[Constants.MAXSTAGES];

        for (int i = 0; i < Constants.MAXSTAGES; i++)
        {
            IsStartCount[i] = false;
            NowSec[i] = 0;
            GoalTimes[i] = Now;
        }
    }

    void Update()
    {
        for (int i = 0; i < Constants.MAXSTAGES; i++)
        {
            if (!IsStartCount[i])
                continue;

            if (NowSec[i] != Now.Second)
            {
                Count(i);
                NowSec[i] = Now.Second;
            }
        }
    }

    public void StartCount(int stage)
    {
        IsStartCount[stage] = true;
        NowSec[stage] = Now.Second;
        GoalTimes[stage] = Now.AddHours(ResourceDatas[stage].hour);
        GoalTimes[stage] = GoalTimes[stage].AddMinutes(ResourceDatas[stage].min);
    }

    public void LoadCount(int stage)
    {
        if (!IsStartCount[stage])
            return;

        Count(stage);
    }

    void Count(int stage)
    {
        time = "";
        System.TimeSpan comp = GoalTimes[stage] - Now;

        if(comp.Hours <= 0 && comp.Minutes <= 0 && comp.Seconds <= 0)
        {
            Finish(stage);
            return;
        }

        if (comp.Hours < 10)
            time += "0";
        time += comp.Hours.ToString() + " : ";
        if (comp.Minutes < 10)
            time += "0";
        time += comp.Minutes.ToString() + " : ";
        if (comp.Seconds < 10)
            time += "0";
        time += comp.Seconds.ToString();

        ShowData(stage, time);
    }

    void ShowData(int stage, string str)
    {
        if (!IsStartCount[stage])
            return;

        //SideMenuSlot에 표시
        if (ResourceDatas[stage].coin > 0)
        {
            GameManager.Inst().UiManager.MainUI.GetSideMenuSlot(stage).Resources[0].gameObject.SetActive(true);
            GameManager.Inst().UiManager.MainUI.GetSideMenuSlot(stage).ResourceTexts[0].text = ResourceDatas[stage].coin.ToString();
        }
        else
            GameManager.Inst().UiManager.MainUI.GetSideMenuSlot(stage).Resources[0].gameObject.SetActive(false);

        if (ResourceDatas[stage].resource > 0)
        {
            GameManager.Inst().UiManager.MainUI.GetSideMenuSlot(stage).Resources[1].gameObject.SetActive(true);
            GameManager.Inst().UiManager.MainUI.GetSideMenuSlot(stage).ResourceTexts[1].text = ResourceDatas[stage].resource.ToString();
        }
        else
            GameManager.Inst().UiManager.MainUI.GetSideMenuSlot(stage).Resources[1].gameObject.SetActive(false);

        if (ResourceDatas[stage].jewel > 0)
        {
            GameManager.Inst().UiManager.MainUI.GetSideMenuSlot(stage).Resources[2].gameObject.SetActive(true);
            GameManager.Inst().UiManager.MainUI.GetSideMenuSlot(stage).ResourceTexts[2].text = ResourceDatas[stage].jewel.ToString();
        }
        else
            GameManager.Inst().UiManager.MainUI.GetSideMenuSlot(stage).Resources[2].gameObject.SetActive(false);

        if (GameManager.Inst().UiManager.MainUI.GetSideMenuSlot(stage).Timer != null)
            GameManager.Inst().UiManager.MainUI.GetSideMenuSlot(stage).Timer.text = str;

        if (str == "00 : 00 : 00")
            GameManager.Inst().UiManager.MainUI.GetSideMenuSlot(stage).GatherBtn.gameObject.SetActive(true);
        else
            GameManager.Inst().UiManager.MainUI.GetSideMenuSlot(stage).GatherBtn.gameObject.SetActive(false);
    }

    void Finish(int stage)
    {
        string str = "00 : 00 : 00";

        ShowData(stage, str);

        IsFinish = true;
        GameManager.Inst().UiManager.MainUI.SideMenu.Eff_Notice.SetActive(true);
    }

    public void GetTempResources(int stage)
    {
        GameManager.Inst().UiManager.MainUI.PopupReward.gameObject.SetActive(true);
        if (ResourceDatas[stage].coin > 0)
            GameManager.Inst().UiManager.MainUI.PopupReward.Show((int)PopupReward.RewardType.COIN, ResourceDatas[stage].coin);
        if (ResourceDatas[stage].jewel > 0)
            GameManager.Inst().UiManager.MainUI.PopupReward.Show((int)PopupReward.RewardType.CRYSTAL, ResourceDatas[stage].jewel);
        if (ResourceDatas[stage].resource > 0)
            GameManager.Inst().UiManager.MainUI.PopupReward.Show(stage + 3, ResourceDatas[stage].resource);

        GameManager.Inst().AddResource(stage + 1, ResourceDatas[stage].resource);
        GameManager.Inst().Player.AddCoin(ResourceDatas[stage].coin);
        GameManager.Inst().AddJewel(ResourceDatas[stage].jewel);

        GameManager.Inst().UiManager.MainUI.GetSideMenuSlot(stage).GatherBtn.gameObject.SetActive(false);

        IsFinish = false;
        if(!GameManager.Inst().QstManager.IsClear)
            GameManager.Inst().UiManager.MainUI.SideMenu.Eff_Notice.SetActive(false);

        StartCount(stage);
    }
}
