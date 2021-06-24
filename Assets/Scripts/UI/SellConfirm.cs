using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellConfirm : MonoBehaviour
{
    public Text PriceText;
    public Text QuantityText;
    public Button DownBtn;
    public Button UpBtn;


    void Start()
    {
        gameObject.SetActive(false);
    }
}
