using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour
{
    public GameObject Save;
    public GameObject Load;
    public Animation Anim;

    public void SaveComplete()
    {
        Load.SetActive(false);
        Save.SetActive(true);

        Anim.Play();
    }

    public void LoadComplete()
    {
        Save.SetActive(false);
        Load.SetActive(true);

        Anim.Play();
    }
}
