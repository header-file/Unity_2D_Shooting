using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cheat : MonoBehaviour
{
    public Toggle[] Toggles;
    public GameObject[] Pages;


    void Start()
    {
        //Toggles[0].isOn = true;

        gameObject.SetActive(false);
    }

    public void SwitchPage(int index)
    {
        if (Toggles[index].isOn == false)
            return;

        for (int i = 0; i < Pages.Length; i++)
            Pages[i].SetActive(false);

        Toggles[index].Select();
        Pages[index].SetActive(true);
    }
}
