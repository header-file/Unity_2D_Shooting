using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    CameraShake Camerashake;

    public float ShakeAmount;
    float ShakeTime;
    Vector3 InitialPos;

    void Start()
    {
        //Camerashake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        InitialPos = new Vector3(0.0f, 4.2f, -10.0f);
    }

    void Update()
    {
        if(ShakeTime > 0)
        {
            transform.position = Random.insideUnitSphere * ShakeAmount + InitialPos;
            ShakeTime -= Time.deltaTime;
        }
        else
        {
            ShakeTime = 0;
            transform.position = InitialPos;
        }
    }

    public void Vibrate(float STime)
    {
        ShakeTime = STime;
    }
}
