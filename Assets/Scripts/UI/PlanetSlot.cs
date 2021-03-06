﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetSlot : MonoBehaviour
{
    public Image PlanetImage;
    public Text Name;
    public GameObject Lock;

    void Awake()
    {
        Name.gameObject.SetActive(false);
        Lock.SetActive(true);
    }

    void Update()
    {
        if(Name.gameObject.activeSelf)
        {
            Vector3 pos = PlanetImage.gameObject.GetComponent<RectTransform>().anchoredPosition;
            pos.y -= 200.0f * PlanetImage.transform.localScale.x;
            Name.gameObject.GetComponent<RectTransform>().anchoredPosition = pos;
        }
            
    }
}
