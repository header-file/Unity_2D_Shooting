using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.DateTime;

public class ResourceManager : MonoBehaviour
{
    public System.DateTime[] StartTimes;

    bool[] IsStartCount;
    int[] NowSec;
    int[,] TempResources;


    void Start()
    {
        IsStartCount = new bool[Constants.MAXSTAGES];
        NowSec = new int[Constants.MAXSTAGES];
        StartTimes = new System.DateTime[Constants.MAXSTAGES];
        TempResources = new int[Constants.MAXSTAGES, 2];

        for (int i = 0; i < Constants.MAXSTAGES; i++)
        {
            IsStartCount[i] = false;
            NowSec[i] = 0;
            StartTimes[i] = Now;

            TempResources[i, 1] = 0;
            TempResources[i, 0] = 0;
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
                //Debug.Log(Now);
                Count(i);
                NowSec[i] = Now.Second;
            }
        }
    }

    public void StartCount(int stage)
    {
        IsStartCount[stage] = true;
        NowSec[stage] = Now.Second;
        StartTimes[stage] = Now;
    }

    public void LoadCount(int stage)
    {
        if (!IsStartCount[stage])
            return;

        System.TimeSpan comp = Now - StartTimes[stage];

        if (comp.Hours > 0)
        {
            TempResources[stage, 0] += 100 * (stage + 1) * 6 * comp.Hours;
            TempResources[stage, 1] += 5 * (stage + 1) * 6 * comp.Hours;
        }
        if(comp.Minutes > 10)
        {
            TempResources[stage, 0] += 100 * (stage + 1)  * (comp.Minutes / 10);
            TempResources[stage, 1] += 5 * (stage + 1) * (comp.Minutes / 10);
        }

        //SideMenuSlot에 표시
        if (GameManager.Inst().UiManager.GetSideMenuSlot(stage).Resources.Length == 2)
        {
            GameManager.Inst().UiManager.GetSideMenuSlot(stage).Resources[0].text = TempResources[stage, 0].ToString();
            GameManager.Inst().UiManager.GetSideMenuSlot(stage).Resources[1].text = TempResources[stage, 1].ToString();
        }
    }

    void Count(int stage)
    {
        string str = "";
        System.TimeSpan comp = Now - StartTimes[stage];

        if (comp.Hours < 10)
            str += "0";
        str += comp.Hours.ToString() + " : ";
        if (comp.Minutes < 10)
            str += "0";
        str += comp.Minutes.ToString() + " : ";
        if (comp.Seconds < 10)
            str += "0";
        str += comp.Seconds.ToString();

        if(comp.Minutes % 10 == 0 && comp.Seconds == 0)
        {
            //자원 상승
            TempResources[stage, 0] += 100 * (stage + 1);
            TempResources[stage, 1] += 5 * (stage + 1);
        }

        ShowData(stage, str);
    }

    void ShowData(int stage, string str)
    {
        //SideMenuSlot에 표시
        if (GameManager.Inst().UiManager.GetSideMenuSlot(stage).Resources.Length == 2)
        {
            GameManager.Inst().UiManager.GetSideMenuSlot(stage).Resources[0].text = TempResources[stage, 0].ToString();
            GameManager.Inst().UiManager.GetSideMenuSlot(stage).Resources[1].text = TempResources[stage, 1].ToString();
        }

        if (GameManager.Inst().UiManager.GetSideMenuSlot(stage).Timer != null)
            GameManager.Inst().UiManager.GetSideMenuSlot(stage).Timer.text = str;
    }
}
