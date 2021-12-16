using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingBubble : MonoBehaviour
{
    public GameObject Bubble;
    public GameObject ConfirmWindow;
    public Text AmountText;
    public Image TypeImg;
    public int[] Amounts;
    public Sprite[] Types;

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
        TypeImg.sprite = Types[Type];
    }

    public void Make()
    {
        GameObject explosion = GameManager.Inst().ObjManager.MakeObj("Explosion");
        explosion.transform.position = Bubble.transform.position;

        switch (Type)
        {
            case 0:
                for(int i = 0; i < 10; i++)
                {
                    Item_Coin coin = GameManager.Inst().ObjManager.MakeObj("Coin").GetComponent<Item_Coin>();
                    coin.SetValue(100);
                    coin.transform.position = Bubble.transform.position;

                    Vector3 pos = transform.position;
                    pos.x += Mathf.Cos(Mathf.Deg2Rad * Random.Range(0.0f, 180.0f)) * 1.0f;
                    pos.y += Mathf.Sin(Mathf.Deg2Rad * Random.Range(0.0f, 180.0f)) * 1.0f;

                    coin.SetTargetPosition(pos);
                    coin.SetIsScatter(true);
                    coin.SetIsAbsorb(false);
                }
                break;
            case 1:
                for (int i = 0; i < 2; i++)
                {
                    Item_Jewel jewel = GameManager.Inst().ObjManager.MakeObj("Jewel").GetComponent<Item_Jewel>();
                    jewel.SetValue(1);
                    jewel.transform.position = Bubble.transform.position;

                    Vector3 pos = transform.position;
                    pos.x += Mathf.Cos(Mathf.Deg2Rad * Random.Range(0.0f, 180.0f)) * 1.0f;
                    pos.y += Mathf.Sin(Mathf.Deg2Rad * Random.Range(0.0f, 180.0f)) * 1.0f;

                    jewel.SetTargetPosition(pos);
                    jewel.SetIsScatter(true);
                    jewel.SetIsAbsorb(false);
                }
                break;
            case 2:
                for (int i = 0; i < 5; i++)
                {
                    Item_Resource resource = GameManager.Inst().ObjManager.MakeObj("Resource").GetComponent<Item_Resource>();
                    resource.transform.position = transform.position;

                    Vector3 pos = transform.position;
                    pos.x += Mathf.Cos(Mathf.Deg2Rad * Random.Range(0.0f, 180.0f)) * 1.0f;
                    pos.y += Mathf.Sin(Mathf.Deg2Rad * Random.Range(0.0f, 180.0f)) * 1.0f;

                    resource.SetValue(10);
                    resource.SetColor();
                    resource.TargetPosition = pos;
                    resource.IsScatter = true;
                    resource.BeginAbsorb();
                }
                break;
            case 3:
                GameManager.Inst().MakeReinforce(-1, 0, Bubble.transform);
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

        GameManager.Inst().AdsManager.AdvType = AdvertiseManager.AdType.FLOATING;
        GameManager.Inst().AdsManager.PlayAd();
    }

    public void OnClickNoBtn()
    {
        ConfirmWindow.SetActive(false);
        Bubble.SetActive(false);
    }
}
