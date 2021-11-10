using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeverMode : MonoBehaviour
{
    public Animation Anim;
    

    void ToLoop()
    {
        Anim.Play("UI_Fever_loop");
    }

    public void ToEnd()
    {
        Anim.Play("UI_Fever_end");
    }

    void End()
    {
        //gameObject.SetActive(false);
    }
}
