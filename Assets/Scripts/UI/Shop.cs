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
        Pages[index].SetActive(Toggles[index].isOn);
        
        if (Toggles[index].isOn)
            ShowPage(index);
    }
}