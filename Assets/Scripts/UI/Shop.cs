using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Toggle[] Toggles;
    public GameObject[] Pages;
    public Text AdLeftText;
    public int AdLeft;

    const int Ads = 5;    


    void Start()
    {
        AdLeft = Ads;
        gameObject.SetActive(false);
    }

    public void ShowPage(int index)
    {
        
    }

    public void MinusAdLeft()
    {
        if (AdLeft <= 0)
            return;

        AdLeft--;
        AdLeftText.text = AdLeft.ToString() + " / " + Ads.ToString();
    }

    public void OnSelectToggle(int index)
    {
        Pages[index].SetActive(Toggles[index].isOn);
        
        if (Toggles[index].isOn)
            ShowPage(index);
    }
}