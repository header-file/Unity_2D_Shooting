using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffSlot : MonoBehaviour
{
    public CanvasGroup CanvasGroup;
    public Text TimerText;
    public Image Frame;
    public GameObject Endless;

    void Start()
    {
        TimerText.text = "";
        Endless.SetActive(false);
        Frame.color = Color.black;
    }
}
