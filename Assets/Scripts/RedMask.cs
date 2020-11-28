using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMask : MonoBehaviour
{
    SpriteRenderer Renderer;
    Color Color;
    bool IsAlert;

    float Gyesu;

    public void SetIsAlert(bool b) { IsAlert = b; }

    void Awake()
    {
        Renderer = GetComponent<SpriteRenderer>();
        Color = Renderer.color;
        Color.a = 0.0001f;
        IsAlert = false;
        Gyesu = 1.0f;
    }

    private void Start()
    {
        //gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if(IsAlert)
        {
            Color.a += (1.0f / 6.0f) * Gyesu;

            if (Color.a >= 0.6f || Color.a <= 0.0f)
                Gyesu *= -1.0f;

            Renderer.color = Color;
            if (Color.a <= 0)
            {
                IsAlert = false;
                Color.a = 0.0001f;
                //gameObject.SetActive(false);
            }
        }
    }
}
