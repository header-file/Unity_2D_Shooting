using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingBubble : MonoBehaviour
{
    public GameObject Bubble;
    public GameObject ConfirmWindow;
    public Text AmountText;
    public Text TypeText;
    public int[] Amounts;
    public string[] Types;

    bool IsFloating;
    float GoalPosX;
    float Gyesu;
    Vector3 CurPos;
    Vector3 Offset;
    float Amplitude;
    float Frequency;
    float Timer;
    int Type;


    void Awake()
    {
        IsFloating = true;
        GoalPosX = 2.5f;
        Gyesu = 0.5f;
        Offset = Bubble.transform.position;
        Frequency = 0.25f;
        Amplitude = 1.0f;
        Timer = 0.0f;
        ConfirmWindow.SetActive(false);
    }

    void Update()
    {
        if (IsFloating)
        {
            Floating();
            CountDisappear();
        }
        else
            CountAppear();
    }

    void Floating()
    {
        CurPos = Offset;
        float temp = Bubble.transform.position.x;
        temp += Time.deltaTime * Gyesu;
        CurPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * Frequency) * Amplitude;

        CurPos.x = temp;
        Bubble.transform.position = CurPos;

        if (Mathf.Abs(CurPos.x - GoalPosX) <= 0.05f)
            ResetX();
    }

    void ResetX()
    {
        Gyesu *= -1.0f;
        GoalPosX *= -1.0f;
    }

    void CountDisappear()
    {
        Timer += Time.deltaTime;

        if (Timer >= 1000.0f)
        {
            Disappear();
            Timer = 0.0f;
        }
    }

    void Disappear()
    {
        Bubble.SetActive(false);
        IsFloating = false;
    }

    void CountAppear()
    {
        Timer += Time.deltaTime;

        if (Timer >= 15.0f)
        {
            Appear();
            Timer = 0.0f;
        }
    }

    void Appear()
    {
        Bubble.SetActive(true);
        IsFloating = true;
    }

    void ShowWindow()
    {
        Type = Random.Range(0, 4);

        AmountText.text = Amounts[Type].ToString();
        TypeText.text = Types[Type];
    }

    void Make()
    {
        switch(Type)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }

    public void OnClickBubble()
    {
        ConfirmWindow.SetActive(true);
        ShowWindow();
        IsFloating = false;
    }

    public void OnClickYesBtn()
    {
        ConfirmWindow.SetActive(false);
        Bubble.SetActive(false);

        GameObject explosion = GameManager.Inst().ObjManager.MakeObj("Explosion");
        explosion.transform.position = Bubble.transform.position;

        Make();
    }

    public void OnClickNoBtn()
    {
        ConfirmWindow.SetActive(false);
        Bubble.SetActive(false);
    }
}
