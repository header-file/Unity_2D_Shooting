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
    public Text[] ResourceTexts;
    public GameObject[] Resources;
    public int Index;
    public GameObject Clear;

    public void Show(int stage)
    {
        PlanetName.text = GameManager.Inst().TxtManager.PlanetNames[stage];
        PlanetImg.sprite = GameManager.Inst().UiManager.MainUI.SideMenu.PlanetImgs[stage];

        if(ResourceIcon != null)
            ResourceIcon.sprite = GameManager.Inst().UiManager.MainUI.SideMenu.ResourceImgs[stage];

        if (stage == GameManager.Inst().StgManager.ReachedStage - 1)
            return;

        if (GatherBtn == null)
            return;
    }

    public void OnClickGetBtn()
    {
        GameManager.Inst().ResManager.GetTempResources(Index);
    }

    public void OnClickClearBtn()
    {
        GameManager.Inst().QstManager.Clear();
    }
}
