using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spread : Bullet
{
    public float Duration;


    public float GetDuration() { return Duration; }

    public void SetDuration(float time) { Duration = time; }


    void Awake()
    {
        Type = BulletType.SPREAD;
        GetComponent<SpriteRenderer>().color = Color.white;
        Duration = GameManager.Inst().UpgManager.BData[(int)Type].GetDuration();
    }

    void Update()
    {
        Duration -= Time.deltaTime;

        if (Duration <= 0.0f)
            Deactivate();
    }

    void OnDisable()
    {
        Duration = 10.0f;
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
