using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SideMenu : MonoBehaviour
{
    public GameObject BackBtn;
    public RectTransform SideBar;
    public SideMenuSlot[] Slots;
    public Transform ContentTransform;
    public GameObject BtnArrow;
    public Sprite[] PlanetImgs;

    public bool IsOpen = false;

    Vector3 Reverse = new Vector3(0.0f, 0.0f, 180.0f);
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
        BtnArrow.transform.Rotate(Reverse);
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
        BtnArrow.transform.Rotate(Reverse);
        BackBtn.SetActive(false);
    }

    void Closing()
    {
        SideBar.anchoredPosition = Vector3.Lerp(SideBar.anchoredPosition, new Vector3(0.0f, 0.0f, 0.0f), Time.deltaTime * 5.0f);

        if (Time.deltaTime >= 0.2f)
            IsSideMenuClose = false;
    }

    public void MakeSlot()
    {
        if (Slots.Length != Constants.MAXSTAGES)
            return;

        for (int i = 0; i < Constants.MAXSTAGES; i++)
        {
            if (Slots[i] != null)
            {
                Slots[i].gameObject.transform.SetParent(GameManager.Inst().ObjManager.UIPool.transform, false);
                Slots[i].gameObject.SetActive(false);
            }

            if (i == GameManager.Inst().StgManager.ReachedStage - 1)
                Slots[i] = GameManager.Inst().ObjManager.MakeObj("SideNow").GetComponent<SideMenuSlot>();
            else if(i > GameManager.Inst().StgManager.ReachedStage - 1)
                Slots[i] = GameManager.Inst().ObjManager.MakeObj("SideNotYet").GetComponent<SideMenuSlot>();
            else
                Slots[i] = GameManager.Inst().ObjManager.MakeObj("SideCleared").GetComponent<SideMenuSlot>();

            Slots[i].transform.SetParent(ContentTransform, false);
            Slots[i].Index = i;
        }
    }

    public void OnClickSideBarBtn()
    {
        if (IsOpen)
        {
            OnClickSideBarBackBtn();
            return;
        }

        IsOpen = true;
        SideMenuOpen();

        for (int i = 0; i < GameManager.Inst().StgManager.ReachedStage; i++)
            Slots[i].Show(i);

        GameManager.Inst().IptManager.SetIsAbleControl(false);
        GameManager.Inst().IptManager.SetIsAbleSWControl(false);

        if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 18)
            GameManager.Inst().Tutorials.Step++;
    }

    public void OnClickSideBarBackBtn()
    {
        IsOpen = false;
        SideMenuClose();

        GameManager.Inst().IptManager.SetIsAbleControl(true);
        GameManager.Inst().IptManager.SetIsAbleSWControl(true);

        if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 22)
            GameManager.Inst().Tutorials.Step++;
    }
}
