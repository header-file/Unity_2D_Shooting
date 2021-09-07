using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumInput : MonoBehaviour
{
    public Text NumText;
    int Type;
    string BefNum;
    string Number;

    public void Awake()
    {
        Type = -1;
        BefNum = "";
        Number = "";
        gameObject.SetActive(false);
    }

    public void Show(string str, int type)
    {
        BefNum = str;
        NumText.text = str;
        Number = "";
        Type = type;
    }

    public void OnClickExit()
    {
        gameObject.SetActive(false);
    }

    public void OnClickInputNumber(int num)
    {
        if (Number == "0")
            Number = "";
        Number += num.ToString();
        NumText.text = Number;
    }

    public void OnClickEraseNumber()
    {
        Number = Number.Remove(Number.Length - 1);
        if (Number == "")
            Number = "0";
        NumText.text = Number;
    }

    public void OnClickClearNumber()
    {
        Number = "";
        NumText.text = "0";
    }

    public void OnClickAdd()
    {
        int val = int.Parse(BefNum) + int.Parse(Number);
        NumText.text = val.ToString();
        Number = "";

        Calculate();
    }

    public void OnClickSub()
    {
        int val = int.Parse(BefNum) - int.Parse(Number);
        if (val < 0)
            val = 0;
        NumText.text = val.ToString();
        Number = "";

        Calculate();
    }

    public void OnClickEqu()
    {
        int val = int.Parse(Number);
        NumText.text = val.ToString();
        Number = "";

        Calculate();
    }

    void Calculate()
    {
        Cheat cheatUI = GameManager.Inst().UiManager.MainUI.Center.Cheat.GetComponent<Cheat>();
        switch (Type)
        {
            case 0:
                cheatUI.CoinText.text = NumText.text;
                GameManager.Inst().Player.SetCoin(int.Parse(NumText.text));
                break;

            case 1:
                cheatUI.JewelText.text = NumText.text;
                GameManager.Inst().SetJewel(int.Parse(NumText.text));
                break;

            case 2:
                cheatUI.ResourceTexts[0].text = NumText.text;
                GameManager.Inst().SetResource(1, int.Parse(NumText.text));
                break;

            case 3:
                cheatUI.ResourceTexts[1].text = NumText.text;
                GameManager.Inst().SetResource(2, int.Parse(NumText.text));
                break;

            case 4:
                cheatUI.ResourceTexts[2].text = NumText.text;
                GameManager.Inst().SetResource(3, int.Parse(NumText.text));
                break;

            case 5:
                cheatUI.ResourceTexts[3].text = NumText.text;
                GameManager.Inst().SetResource(4, int.Parse(NumText.text));
                break;
        }
    }
}
