using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShake : MonoBehaviour
{
    public float Speed = 1.0f;
    public float Scale = 0.15f;

    float ShakeTime;
    Vector3 BasePos;
    bool IsShake;

    public void SetBasePos(Vector3 pos) { BasePos = pos; }

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
            Scale *= 0.7f;

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
    }

    public void Shake(float time, float scale)
    {
        if (ShakeTime > 0.0f)
            return;

        IsShake = true;
        ShakeTime = time;
        Scale = scale;
    }
}
