using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.DateTime;

public class ResourceManager : MonoBehaviour
{
    System.DateTime[] StartTimes;
    bool[] IsStartCount;
    int[] NowSec;

    void Start()
    {
        IsStartCount = new bool[Constants.MAXSTAGES];
        NowSec = new int[Constants.MAXSTAGES];
        StartTimes = new System.DateTime[Constants.MAXSTAGES];

        for (int i = 0; i < Constants.MAXSTAGES; i++)
        {
            IsStartCount[i] = false;
            NowSec[i] = 0;
            StartTimes[i] = Now;
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

    public void LoadCount(int stage, System.DateTime time)
    {
        IsStartCount[stage] = true;
        NowSec[stage] = Now.Second;
        StartTimes[stage] = time;
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
        }

        //SideMenuSlot에 표시
        GameManager.Inst().UiManager.GetSideMenuSlot(stage).Timer.text = str;
    }
}
