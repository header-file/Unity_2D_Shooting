using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene : MonoBehaviour
{
    public Animator Anim;
    public int MaxCount;

    int SceneCount;
    bool IsAbleNext;
    bool IsEnd;


    void Start()
    {
        SceneCount = 0;
        gameObject.SetActive(false);
        IsAbleNext = false;
        IsEnd = false;
    }

    void NextScene()
    {
        IsAbleNext = true;
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
        IsEnd = true;
    }

    void OnMouseDown()
    {
        if (!IsAbleNext)
            return;

        IsAbleNext = false;

        if (IsEnd)
            gameObject.SetActive(false);
        else
            ToNext();
    }
}
