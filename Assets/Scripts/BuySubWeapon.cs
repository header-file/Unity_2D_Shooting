using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuySubWeapon : MonoBehaviour
{
    public GameObject BuyButton;

    Button BuyBtn;

    int SelectedIndex;


    public int GetSelectedIndex() { return SelectedIndex; }
    
    public void SetBuyBtnInteratable(bool b) { BuyBtn.interactable = b; }

    void Awake()
    {
        BuyBtn = BuyButton.GetComponent<Button>();
    }

    public void ShowBuy(int index)
    {
        gameObject.SetActive(true);
        Time.timeScale = 0.0f;
        SelectedIndex = index;
        GameManager.Inst().TxtManager.SetBPrices(index, GameManager.Inst().UpgManager.GetSubWeaponPrice());
        GameManager.Inst().TxtManager.SetSName(index);
        
        if (!BuyBtn.IsInteractable())
            BuyBtn.interactable = true;
    }

    public void Cancel()
    {
        SelectedIndex = -1;
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
    }

    public void Buy()
    {
        
        GameManager.Inst().UpgManager.AddLevel((int)UpgradeManager.UpgradeType.SUBWEAPON);
    }
}
