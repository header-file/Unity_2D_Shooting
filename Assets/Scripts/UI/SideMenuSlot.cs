using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideMenuSlot : MonoBehaviour
{
    public Button GatherBtn;
    public Text PlanetName;
    public Image PlanetImg;
    public Transform ContentTransform;
    public Text Timer;
    public Image ResourceIcon;
    public Text[] Resources;
    public int Index;

    public void ShowGatheringArea(int stage)
    {
        if (GatherBtn == null)
            return;

        if (GameManager.Inst().ResManager.CheckAble(stage))
            GatherBtn.interactable = true;
        else
            GatherBtn.interactable = false;
    }

    public void OnClickGetBtn()
    {
        GameManager.Inst().ResManager.GetTempResources(Index);
    }
}
