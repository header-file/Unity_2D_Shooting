using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public int CurrentBuffType;
    public float[] BuffTimers;


    void Awake()
    {
        BuffManager[] objs = FindObjectsOfType<BuffManager>();
        if (objs.Length > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);

        BuffTimers = new float[Constants.MAXBUFFS];
        CurrentBuffType = 0;
    }

    void Update()
    {
        if(!GameManager.Inst().IsFullPrice)
            Count();
    }

    void Count()
    {
        for (int i = 0; i < Constants.MAXBUFFS; i++)
        {
            if (BuffTimers[i] <= 0.0f)
                continue;

            BuffTimers[i] -= Time.deltaTime;
            SetText(i);

            if (BuffTimers[i] <= 0.0f)
                EndBuff(i);
        }
    }

    public void StartBuff(int index)
    {
        switch (index)
        {
            case 0:
                GameManager.Inst().Player.StartAutoShot();
                BuffTimers[index] += 600.0f;

                if (GameManager.Inst().IsFullPrice)
                {
                    GameManager.Inst().UiManager.MainUI.Buff.BuffSlots[index].TimerText.gameObject.SetActive(false);
                    GameManager.Inst().UiManager.MainUI.Buff.BuffSlots[index].Endless.SetActive(true);
                }
                break;
            default:
                break;
        }
    }

    void EndBuff(int index)
    {
        switch (index)
        {
            case 0:
                GameManager.Inst().Player.EndAutoShot();
                GameManager.Inst().UiManager.MainUI.Buff.BuffSlots[index].TimerText.text = "";
                break;
        }
    }

    void SetText(int index)
    {
        string str = "";
        if (BuffTimers[index] / (3600 * 24) >= 1)
        {
            int d = (int)BuffTimers[index] / (3600 * 24);
            if (d < 10)
                str += "0";
            str += d.ToString();
            str += "d ";
        }
        else if (BuffTimers[index] / 3600 >= 1)
        {
            int h = (int)BuffTimers[index] / 3600;
            if (h < 10)
                str += "0";
            str += h.ToString();
            str += "h";
        }
        else
        {
            int min = (int)BuffTimers[index] / 60;
            if (min < 10)
                str += "0";
            str += min.ToString();
            str += "m ";
            int sec = (int)BuffTimers[index] % 60;
            if (sec < 10)
                str += "0";
            str += sec.ToString();
            str += "s";
        }

        GameManager.Inst().UiManager.MainUI.Buff.BuffSlots[index].TimerText.text = str;
    }
}
