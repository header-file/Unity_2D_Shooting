using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationTimer : MonoBehaviour
{
    public float TargetTime = 0.0f; 
    public bool IsStart = false;

    float CurrentTime = 0.0f;


    void FixedUpdate()
    {
        if(IsStart)
        {
            CurrentTime += Time.deltaTime;

            if (CurrentTime >= TargetTime)
            {
                CurrentTime = 0.0f;
                IsStart = false;
                gameObject.SetActive(false);
            }
        }
    }
}
