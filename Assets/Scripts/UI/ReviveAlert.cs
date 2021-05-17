using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviveAlert : MonoBehaviour
{
    public Button YesBtn;
    public Text TimeText;
    public int Index;

    void Start()
    {
        Index = 0;

        gameObject.SetActive(false);
    }

    public void Show(int index)
    {
        Index = index;

        int time = GameManager.Inst().GetSubweapons(index).CoolTime;
        int min = time / 60;
        int sec = time % 60;
        TimeText.text = min.ToString() + "분 ";
        if (sec / 10 == 0)
            TimeText.text += "0";
        TimeText.text += sec.ToString() + "초 후에 자동으로 부활합니다.";

        if (GameManager.Inst().Jewel <= 0)
            YesBtn.interactable = false;
        else
            YesBtn.interactable = true;
    }

    public void OnClickNoBtn()
    {
        gameObject.SetActive(false);
    }
}
