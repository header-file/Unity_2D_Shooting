using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    public GameObject HPUI;
    public Image HPBar;
    public Text CoolTime;
    public ObjectShake Shaker;

    void Awake()
    {
        HPUI.SetActive(false);
        CoolTime.gameObject.SetActive(false);
    }

    public void SetCoolTime(int time)
    {
        int min = time / 60;
        int sec = time % 60;

        string text = "";
        if (min >= 10)
            text += min.ToString();
        else
            text += ("0" + min.ToString());

        text += ":";

        if (sec >= 10)
            text += sec.ToString();
        else
            text += ("0" + sec.ToString());

        CoolTime.GetComponent<Text>().text = text;
    }
}
