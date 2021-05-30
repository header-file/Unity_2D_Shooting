using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyJewel : MonoBehaviour
{
    public Text AmountText;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Show(int type)
    {
        gameObject.SetActive(true);

        switch (type)
        {
            case 0:
                AmountText.text = "4";
                break;

            case 1:
                AmountText.text = "12";
                break;

            case 2:
                AmountText.text = "16";
                break;
        }
    }

    public void OnClickNextBtn()
    {
        gameObject.SetActive(false);

        GameManager.Inst().DatManager.GameData.ProcessDailyJewel();
    }
}
