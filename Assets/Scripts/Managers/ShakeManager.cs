using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ShakeManager : MonoBehaviour
{
    public Light2D GlobalLight;
    public AnimationCurve Curve;
    public ObjectShake BgShaker;
    public float ShakeScale;
    public float ShakeTime = 0.05f;

    bool IsTimelag;
    bool IsLightOn;
    float LightTime;


    void Awake()
    {
        GameManager.Inst().ShkManager = gameObject.GetComponent<ShakeManager>();
        IsTimelag = false;
    }

    void Update()
    {
        if (IsLightOn)
        {
            GlobalLight.intensity = Curve.Evaluate(LightTime);
            LightTime += Time.deltaTime;

            if(LightTime >= 0.5f)
            {
                IsLightOn = false;
                GlobalLight.intensity = 0.9f;
            }
        }
    }

    public void Damage()
    {
        Timelag();

        BgShaker.Shake(ShakeTime, ShakeScale);

        GameManager.Inst().Player.Shaker.Shake(ShakeTime, ShakeScale);
        
        for (int i = 0; i < 4; i++)
        {
            if (GameManager.Inst().GetSubweapons(i) != null)
                GameManager.Inst().GetSubweapons(i).Shaker.Shake(ShakeTime, ShakeScale);
        }

        for (int i = 0; i < 4; i++)
            GameManager.Inst().UiManager.MainUI.Center.Turrets[i].Shaker.Shake(ShakeTime, ShakeScale);
    }

    public void Hit()
    {
        
    }

    public void Timelag()
    {
        if (IsTimelag)
            return;

        Time.timeScale = 0.0f;
        StartCoroutine(Wait(0.05f));
    }

    IEnumerator Wait(float time)
    {
        IsTimelag = true;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1.0f;
        IsTimelag = false;
    }

    public void Light()
    {
        IsLightOn = true;
        LightTime = 0.0f;
        GlobalLight.intensity = Curve.Evaluate(0.0f);
    }
}
