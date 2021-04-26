﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideMenuSlot : MonoBehaviour
{
    public Text PlanetName;
    public Image PlanetImg;
    public Transform ContentTransform;
    public Text Timer;
    public Image ResourceIcon;
    public Text[] Resources;
    public int Index;

    public void OnClickGetBtn()
    {
        GameManager.Inst().ResManager.GetTempResources(Index);
    }
}
