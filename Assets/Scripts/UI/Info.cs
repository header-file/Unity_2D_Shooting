using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    public GameObject[] ColorSelected;
    public GameObject[] ColorChips;

    public void SetColorSelected(int index)
    {
        for (int i = 0; i < GameManager.Inst().ShtManager.MAXCOLOR; i++)
        {
            ColorSelected[i].SetActive(false);
            ColorChips[i].transform.localScale = Vector3.one;
        }

        ColorSelected[index].SetActive(true);
        ColorChips[index].transform.localScale = Vector3.one * 1.5f;
    }
}
