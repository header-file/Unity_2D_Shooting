using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideMenuSlot : MonoBehaviour
{
    public GameObject Not;
    public GameObject Now;
    public GameObject Resource;
    public Text PlanetName;
    public Image PlanetImg;

    float Big = 600.0f;
    float Small = 200.0f;

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

        GetComponent<RectTransform>().sizeDelta = new Vector2(Big, Big);
    }

    public void SetSmall()
    {
        Now.SetActive(false);
        PlanetName.gameObject.SetActive(true);

        GetComponent<RectTransform>().sizeDelta = new Vector2(Big, Small);
    }
}
