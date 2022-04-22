using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene : MonoBehaviour
{
    public Animator Anim;
    public int MaxCount;
    public bool IsOutro;
    public Intro Intro;
    public bool IsFirstTime;

    int SceneCount;
    bool IsAbleNext;
    bool IsEnd;
    bool IsOutroStart;


    void Start()
    {
        SceneCount = 0;
        gameObject.SetActive(false);
        IsAbleNext = false;
        IsEnd = false;
        IsFirstTime = false;

        if (IsOutro)
            IsOutroStart = true;
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

        if (IsOutro)
        {
            if (!IsEnd)
                ToNext();
        }

        IsAbleNext = false;

        if (IsEnd)
        {
            Intro.EndIntro();

            if (IsFirstTime)
                Intro.LoginManager.NextSecne();
        }
        else
            ToNext();
    }

    IEnumerator OutroStart()
    {
        GameManager.Inst().StgManager.CancelEnemies();
        GameManager.Inst().StgManager.EraseCurEnemies();
        yield return new WaitUntil(Outro_Start);
        Anim.SetTrigger("Start");
        IsOutroStart = false;
    }

    bool Outro_Start()
    {
        return IsOutroStart;
    }

    void OutroEnd()
    {
        GameManager.Inst().UiManager.MainUI.gameObject.SetActive(true);
        GameManager.Inst().StgManager.RestartStage();
        gameObject.SetActive(false);
    }
}
