using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene : MonoBehaviour
{
    public Animator Anim;
    public int MaxCount;

    int SceneCount;


    void Start()
    {
        SceneCount = 0;
        gameObject.SetActive(false);
    }

    void NextScene()
    {
        Invoke("ToNext", 1.0f);
    }

    public void ToNext()
    {
        SceneCount++;

        if (SceneCount <= MaxCount)
            Anim.SetTrigger("Scene" + SceneCount.ToString());
        else
            Anim.SetTrigger("End");
    }

    void End()
    {
        gameObject.SetActive(false);
    }
}
