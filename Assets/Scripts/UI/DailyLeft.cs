using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyLeft : MonoBehaviour
{
    public Text LeftText;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Show(int left)
    {
        gameObject.SetActive(true);

        LeftText.text = left.ToString();
    }
}
