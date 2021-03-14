using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideMenu : MonoBehaviour
{
    public GameObject BackBtn;
    public Button OpenBtn;
    public RectTransform SideBar;
    public SideMenuSlot[] Slots;
    public Transform ContentTransform;

    bool IsSideMenuOpen;
    bool IsSideMenuClose;

    void Start()
    {
        BackBtn.SetActive(false);

        Slots = new SideMenuSlot[Constants.MAXSTAGES];
        MakeSlot();
    }

    void Update()
    {
        if (IsSideMenuOpen)
            Opening();
        else if (IsSideMenuClose)
            Closing();
    }

    public void SideMenuOpen()
    {
        IsSideMenuOpen = true;
        IsSideMenuClose = false;
        OpenBtn.interactable = false;
        BackBtn.SetActive(true);
    }

    void Opening()
    {
        SideBar.anchoredPosition = Vector3.Lerp(SideBar.anchoredPosition, new Vector3(-600.0f, 0.0f, 0.0f), Time.deltaTime * 5.0f);

        if (Time.deltaTime >= 0.2f)
            IsSideMenuOpen = false;
    }

    public void SideMenuClose()
    {
        IsSideMenuOpen = false;
        IsSideMenuClose = true;
        OpenBtn.interactable = true;
        BackBtn.SetActive(false);
    }

    void Closing()
    {
        SideBar.anchoredPosition = Vector3.Lerp(SideBar.anchoredPosition, new Vector3(0.0f, 0.0f, 0.0f), Time.deltaTime * 5.0f);

        if (Time.deltaTime >= 0.2f)
            IsSideMenuClose = false;
    }

    void MakeSlot()
    {
        for (int i = 0; i < Constants.MAXSTAGES; i++)
        {
            if (i == GameManager.Inst().StgManager.Stage - 1)
                Slots[i] = GameManager.Inst().ObjManager.MakeObj("SideNow").GetComponent<SideMenuSlot>();
            else if(i > GameManager.Inst().StgManager.Stage - 1)
                Slots[i] = GameManager.Inst().ObjManager.MakeObj("SideNotYet").GetComponent<SideMenuSlot>();
            else
                Slots[i] = GameManager.Inst().ObjManager.MakeObj("SideCleared").GetComponent<SideMenuSlot>();

            Slots[i].transform.SetParent(ContentTransform, false);
        }
    }
}
