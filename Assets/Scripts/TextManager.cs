using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public GameObject[] BulletLevels;
    public GameObject[] BulletPrices;
    public GameObject SubLevel;
    public GameObject SubPrice;

    Text[] BLevels;
    Text[] BPrices;

    public void SetBLevels(int index, int level)
    {
        if (level < 5)
            BLevels[index].text = level.ToString();
        else
            BLevels[index].text = "MAX";
    }
    public void SetBPrices(int index, int price) { BPrices[index].text = price.ToString(); }

    public void SetSLevel(int level)
    {
        if (level < 4)
            SubLevel.GetComponent<Text>().text = level.ToString();
        else
            SubLevel.GetComponent<Text>().text = "MAX";
    }
    public void SetSPrice(int price) { SubPrice.GetComponent<Text>().text = price.ToString(); }

    void Awake()
    {
        BLevels = new Text[5];
        BPrices = new Text[5];

        for (int i = 0; i < 5; i++)
        {    
            BLevels[i] = BulletLevels[i].GetComponent<Text>();
            BPrices[i] = BulletPrices[i].GetComponent<Text>();
        }
    }
}
