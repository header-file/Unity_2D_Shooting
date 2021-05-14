using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFlick : MonoBehaviour
{
    public float Speed;

    Text Target;
    Color Color;
    float Gyesu;

    void Start()
    {
        Target = gameObject.GetComponent<Text>();
        Color = Target.color;

        Speed = 1.0f;
        Gyesu = 1.0f;
    }

    void Update()
    {
       Color.a -= 0.01f * Speed * Gyesu;

        if (Color.a > 1.0f || Color.a < 0.0f)
            Gyesu *= -1;

        Target.color = Color;
    }
}
