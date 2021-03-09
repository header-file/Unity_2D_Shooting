﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZzinBottom : MonoBehaviour
{
    public GameObject[] UniverseIcon;
    public GameObject[] WeaponIcon;
    public GameObject[] InventoryIcon;
    public GameObject[] ShopIcon;
    public CanvasGroup HomeIcon;

    void Start()
    {
        UniverseIcon[0].SetActive(true);
        WeaponIcon[0].SetActive(true);
        InventoryIcon[0].SetActive(true);
        ShopIcon[0].SetActive(true);

        UniverseIcon[1].SetActive(false);
        WeaponIcon[1].SetActive(false);
        InventoryIcon[1].SetActive(false);
        ShopIcon[1].SetActive(false);

        HomeIcon.alpha = 0.0f;
    }
}