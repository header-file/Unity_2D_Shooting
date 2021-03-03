using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideMenuSlot : MonoBehaviour
{
    public GameObject Not;
    public GameObject Now;
    public GameObject Resource;
    public RectTransform NowBg;
    public Text PlanetName;
    public Image PlanetImg;

    public Vector2 Big;
    public Vector2 Small;

    void Awake()
    {
        Not.SetActive(true);
        Resource.SetActive(false);

        SetSmall();
    }

    public void Open()
    {
        Not.SetActive(false);
        Resource.SetActive(true);
    }

    public void SetBig()
    {
        Now.SetActive(true);
        PlanetName.gameObject.SetActive(false);

        GetComponent<RectTransform>().sizeDelta = Big;
        NowBg.sizeDelta = new Vector2(Big.y, Big.x);
    }

    public void SetSmall()
    {
        Now.SetActive(false);
        PlanetName.gameObject.SetActive(true);

        GetComponent<RectTransform>().sizeDelta = Small;
    }
}
