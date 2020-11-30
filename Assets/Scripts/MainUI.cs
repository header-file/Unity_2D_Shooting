using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    public Button UpgradeBtn;
    public Canvas UpgradeUI;
    public Button ManageBtn;
    public GameObject Arrows;

    void Awake()
    {
        for (int i = 0; i < 5; i++)
            Arrows.transform.GetChild(i).gameObject.SetActive(false);
    }

    void Update()
    {
        
    }

    public void OnClickUpgradeBtn()
    {
        UpgradeUI upUI = UpgradeUI.gameObject.GetComponent<UpgradeUI>();
        upUI.AppearStart();
        
        UpgradeBtn.interactable = false;
        Time.timeScale = 0.0f;

        GameManager.Inst().IptManager.SetIsAbleControl(false);
    }

    public void UpgradeBtnInteractable()
    {
        UpgradeBtn.interactable = true;
    }

    public void OnClickManageBtn()
    {
        ManageBtn.gameObject.SetActive(false);
        Time.timeScale = 0.0f;

        GameManager.Inst().IptManager.SetIsAbleControl(false);
    }
}
