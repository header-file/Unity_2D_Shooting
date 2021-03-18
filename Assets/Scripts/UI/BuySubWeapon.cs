using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuySubWeapon : MonoBehaviour
{
    public GameObject BuyButton;
    public Text PriceText;

    Button BuyBtn;

    int SelectedIndex;


    public int GetSelectedIndex() { return SelectedIndex; }
    
    public void SetBuyBtnInteratable(bool b) { BuyBtn.interactable = b; }

    void Awake()
    {
        BuyBtn = BuyButton.GetComponent<Button>();

        if (!BuyBtn.IsInteractable())
            BuyBtn.interactable = true;

        gameObject.SetActive(false);
    }

    public void ShowBuy(int index)
    {
        gameObject.SetActive(true);
        PriceText.text = GameManager.Inst().UpgManager.GetSubWeaponBuyPrice().ToString();

        if (!BuyBtn.IsInteractable())
            BuyBtn.interactable = true;

        SelectedIndex = index;
        GameManager.Inst().UpgManager.SetCurrentSubWeaponIndex(index);

        //GameManager.Inst().TxtManager.SetSPrice(GameManager.Inst().UpgManager.GetSubWeaponPrice(index));
        GameManager.Inst().TxtManager.SetSName(index);
    }

    public void Cancel()
    {
        SelectedIndex = -1;
        //Time.timeScale = 1.0f;
        gameObject.SetActive(false);
    }

    public void Buy()
    {
        GameManager.Inst().UpgManager.AddLevel((int)UpgradeManager.UpgradeType.SUBWEAPON);
        BuyBtn.interactable = false;
        PriceText.text = GameManager.Inst().UpgManager.GetSubWeaponBuyPrice().ToString();
    }
}
