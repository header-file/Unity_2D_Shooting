using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    public GameObject IntroObj;
    public Canvas Canvas;
    public GameObject Glow;

    public void OnClickIntroBtn()
    {
        Canvas.gameObject.SetActive(false);
        Glow.SetActive(false);
        IntroObj.SetActive(true);
    }

    public void EndIntro()
    {
        IntroObj.SetActive(false);
        Glow.SetActive(true);
        Canvas.gameObject.SetActive(true);
    }
}
