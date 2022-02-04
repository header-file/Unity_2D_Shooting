using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public RectTransform Emp;
    public RectTransform[] Blurs;
    public RectTransform TextBg;
    public Text TutorialText;
    public GameObject SkipWindow;
    public Image EmpImg;

    public Vector2 Size;
    public Vector2 Pos;
    public Vector2 TxtSize;
    public Vector2 TxtPos;
    

    public enum TutorialTypes
    {
        MAIN = 0,
        UNIVERSE = 1,
        WEAPON = 2,
        INVENTORY = 3,
        SYNTHESIS = 4,
        BOTTOM = 5,
    };

    float Height = 1280.0f;
    float Width = 720.0f;

    void Awake()
    {
        Size = new Vector2(100, 100);
        Pos = Vector2.zero;

        SkipWindow.SetActive(false);

        gameObject.SetActive(false);
    }

    void Update()
    {
        SetWindow();
    }

    void SetEmpSize()
    {
        Emp.anchoredPosition = Pos;
        Emp.sizeDelta = Size;

        TextBg.anchoredPosition = TxtPos;
        TextBg.sizeDelta = TxtSize;
    }

    public void SetWindow()
    {
        SetEmpSize();

        Blurs[0].sizeDelta = new Vector2(Width, Height / 2.0f - (Emp.anchoredPosition.y + Emp.sizeDelta.y / 2.0f));
        Blurs[0].anchoredPosition = Vector2.zero;

        Blurs[1].sizeDelta = new Vector2(Width, Height / 2.0f + (Emp.anchoredPosition.y - Emp.sizeDelta.y / 2.0f));
        Blurs[1].anchoredPosition = Vector2.zero;

        Blurs[2].sizeDelta = new Vector2(Width / 2.0f + (Emp.anchoredPosition.x - Emp.sizeDelta.x / 2.0f), Emp.sizeDelta.y);
        Blurs[2].anchoredPosition = new Vector2(0.0f, Pos.y);

        Blurs[3].sizeDelta = new Vector2(Width / 2.0f - (Emp.anchoredPosition.x + Emp.sizeDelta.x / 2.0f), Emp.sizeDelta.y);
        Blurs[3].anchoredPosition = new Vector2(0.0f, Pos.y);
    }

    public void OnClickSkip()
    {
        SkipWindow.SetActive(true);
    }

    public void OnClickYes()
    {
        GameManager.Inst().Tutorials.EndTutorial();
    }

    public void OnClickNo()
    {
        SkipWindow.SetActive(false);
    }
}
