using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Toggle[] Toggles;
    public GameObject[] Pages;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowPage(int index)
    {
        
    }

    public void OnSelectToggle(int index)
    {
        if (!Toggles[index].isOn)
        {
            Pages[index].SetActive(false);
            return;
        }

        Toggles[index].Select();
        Pages[index].SetActive(true);
        ShowPage(index);
        
    }
}

/*
 for (int i = 0; i < Pages.Length; i++)
        {
            if (i == index)
            {
                Pages[i].SetActive(true);
                Toggles[i].transition
                ShowPage(i);
            }
            else
                Pages[i].SetActive(false);
        }
     */
