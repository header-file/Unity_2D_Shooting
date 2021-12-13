using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddInventory : MonoBehaviour
{
    public Text BeforeText;
    public Text AfterText;
    public Text AmountText;
    public Text PriceText;
    public Button PlusBtn;
    public Button MinusBtn;
    public Button ConfirmBtn;

    int CurAmount;
    int Price;


    void Awake()
    {
        CurAmount = 0;
        Price = 10;
        gameObject.SetActive(false);
    }

    public void Show()
    {
        CurAmount = 0;

        BeforeText.text = GameManager.Inst().Player.MaxInventory.ToString();
        AfterText.text = (GameManager.Inst().Player.MaxInventory + CurAmount).ToString();
        AmountText.text = CurAmount.ToString();
        PriceText.text = (10 * CurAmount).ToString();

        MinusBtn.interactable = false;
        ConfirmBtn.interactable = false;
    }

    public void OnClickPlusBtn()
    {
        CurAmount += 10;

        AfterText.text = (GameManager.Inst().Player.MaxInventory + CurAmount).ToString();
        AmountText.text = CurAmount.ToString();
        PriceText.text = (Price * CurAmount).ToString();

        if (CurAmount > 0)
        {
            MinusBtn.interactable = true;

            if ((Price * CurAmount) <= GameManager.Inst().Jewel)
            {
                PriceText.color = Color.white;
                ConfirmBtn.interactable = true;
            }
            else
            {
                PriceText.color = Color.red;
                ConfirmBtn.interactable = false;
            }
        }

        if (CurAmount >= (Constants.MAXINVENTORY - GameManager.Inst().Player.MaxInventory))
            PlusBtn.interactable = false;
    }

    public void OnClickMinusBtn()
    {
        CurAmount -= 10;

        AfterText.text = (GameManager.Inst().Player.MaxInventory + CurAmount).ToString();
        AmountText.text = CurAmount.ToString();
        PriceText.text = (Price * CurAmount).ToString();

        if (CurAmount <= 0)
        {
            MinusBtn.interactable = false;

            PriceText.color = Color.white;
            ConfirmBtn.interactable = false;
        }
        else if ((Price * CurAmount) <= GameManager.Inst().Jewel)
        {
            PriceText.color = Color.white;
            ConfirmBtn.interactable = true;
        }

        if (CurAmount < (Constants.MAXINVENTORY - GameManager.Inst().Player.MaxInventory))
            PlusBtn.interactable = true;
    }

    public void OnClickConfirmBtn()
    {
        GameManager.Inst().Jewel -= (Price * CurAmount);
        GameManager.Inst().Player.MaxInventory += CurAmount;
        GameManager.Inst().AddInventory(CurAmount);

        Show();
        GameManager.Inst().UiManager.MainUI.Center.Inventory.ShowInventoryCount();
    }

    public void OnClickCancelBtn()
    {
        gameObject.SetActive(false);
    }
}
