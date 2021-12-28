using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BuffSlot : MonoBehaviour
{
    public CanvasGroup CanvasGroup;
    public Text TimerText;
    public GameObject Endless;
    public Jun_TweenRuntime TweenImg;

    void Start()
    {
        TimerText.text = "";
        Endless.SetActive(false);
        TweenImg.enabled = false;
        Jun_TweenRuntime[] imgs = TweenImg.GetComponents<Jun_TweenRuntime>();
        for (int i = 0; i < imgs.Length; i++)
            imgs[i].enabled = false;
    }
}
