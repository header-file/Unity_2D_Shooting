using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeDataWindow : MonoBehaviour
{
    public GameObject[] TypeWindows;
    public Text[] AmountText;

    public void SetData(int index, int amount)
    {
        TypeWindows[index].SetActive(true);
        AmountText[index].text = amount.ToString();
    }

    void OffWindows()
    {
        for (int i = 0; i < TypeWindows.Length; i++)
            TypeWindows[i].SetActive(false);
    }
}
