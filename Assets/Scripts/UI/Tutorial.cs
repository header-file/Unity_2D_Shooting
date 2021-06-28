using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public RectTransform Emp;
    public RectTransform[] Blurs;

    public Vector2 Size;
    public Vector2 Pos;

    float Height = 1280.0f;
    float Width = 720.0f;

    void Start()
    {
        Size = new Vector2(100, 100);
        Pos = Vector2.zero;
    }

    void Update()
    {
        SetEmpSize();

        Blurs[0].sizeDelta = new Vector2(Width, Height / 2.0f - (Emp.anchoredPosition.y + (Emp.sizeDelta.y / 2.0f)));
        Blurs[0].anchoredPosition = new Vector2(0.0f, -Blurs[0].sizeDelta.y / 2.0f);

        Blurs[1].sizeDelta = new Vector2(Width, Height / 2.0f + (Emp.anchoredPosition.y - (Emp.sizeDelta.y / 2.0f)));
        Blurs[1].anchoredPosition = new Vector2(0.0f, Blurs[1].sizeDelta.y / 2.0f);

        Blurs[2].sizeDelta = new Vector2(Width / 2.0f + (Emp.anchoredPosition.x - (Emp.sizeDelta.x / 2.0f)), Emp.sizeDelta.y);
        Blurs[2].anchoredPosition = new Vector2(Blurs[2].sizeDelta.x / 2.0f, Emp.anchoredPosition.y);

        Blurs[3].sizeDelta = new Vector2(Width / 2.0f - (Emp.anchoredPosition.x + (Emp.sizeDelta.x / 2.0f)), Emp.sizeDelta.y);
        Blurs[3].anchoredPosition = new Vector2(-Blurs[3].sizeDelta.x / 2.0f, Emp.anchoredPosition.y);
    }

    void SetEmpSize()
    {
        Emp.anchoredPosition = Pos;
        Emp.sizeDelta = Size;
    }
}
