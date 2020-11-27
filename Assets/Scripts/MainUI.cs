using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    public Button UpgradeBtn;
    public Canvas UpgradeUI;

    void Start()
    {
        
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
}
