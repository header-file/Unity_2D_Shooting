﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShake : MonoBehaviour
{
    public float Speed = 1.0f;
    public float Scale = 0.15f;

    float ShakeTime;
    Vector3 BasePos;
    bool IsShake;

    void Awake()
    {
        BasePos = transform.position;
    }

    void Update()
    {
        if (!IsShake)
            return;

        if(ShakeTime > 0.0f)
        {
            transform.position = transform.position + Random.insideUnitSphere * Scale;

            ShakeTime -= Time.deltaTime;
        }
        else
        {
            ShakeTime = 0.0f;
            transform.position = BasePos;
            IsShake = false;
        }
    }

    public void Shake(float time)
    {
        if (ShakeTime > 0.0f)
            return;

        IsShake = true;
        ShakeTime = time;
        BasePos = transform.position;
    }

    public void Shake(float time, float scale)
    {
        if (ShakeTime > 0.0f)
            return;

        IsShake = true;
        ShakeTime = time;
        Scale = scale;
        BasePos = transform.position;
    }
}
