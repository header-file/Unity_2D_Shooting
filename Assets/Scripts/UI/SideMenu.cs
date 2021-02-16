using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideMenu : MonoBehaviour
{
    public GameObject BackBtn;
    public Button OpenBtn;
    public RectTransform SideBar;

    bool IsSideMenuOpen;
    bool IsSideMenuClose;

    void Start()
    {
        BackBtn.SetActive(false);
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
        SideBar.anchoredPosition = Vector3.Lerp(SideBar.anchoredPosition, new Vector3(-500.0f, 0.0f, 0.0f), Time.deltaTime * 5.0f);

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
}
