using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    public AudioClip MainTheme;
    public AudioSource BGM;

    void Start()
    {
        BGM.clip = MainTheme;
        BGM.loop = true;
        BGM.Play();
    }
}
