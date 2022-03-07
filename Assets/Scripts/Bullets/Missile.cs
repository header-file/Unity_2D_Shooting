using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Bullet
{
    public GameObject Target;
    public GameObject SearchArea;
    public TrailRenderer Trail;
    
    Vector3 InitPos;
    Rigidbody2D Rig;

    float RotateSpeed;
    float Speed;
    bool IsDisableTrace;
    

    void Awake()
    {
        InitPos = new Vector3(0.0f, 12.0f, 0.0f);
        Rig = GetComponent<Rigidbody2D>();
        IsDisableTrace = false;

        //Damage = 1.0f;
        Type = BulletType.MISSILE;
        GetComponent<SpriteRenderer>().color = Color.white;
        //Damage *= (float)GameManager.Inst().UpgManager.GetBData((int)Type).GetPowerLevel();
        Speed = GameManager.Inst().UpgManager.BData[(int)Type].GetSpeed();
        RotateSpeed = 200.0f;
    }

    void Update()
    {
        if(IsDisableTrace)
            Rig.velocity = transform.up * Speed;
        else
        {
            if (Target == null)
            {
                Rig.velocity = transform.up * Speed;
                Rig.angularVelocity = 0.0f;
            }
            else if (Target != null)
            {
                Vector2 pointTarget = (Vector2)transform.position - (Vector2)Target.transform.position;
                pointTarget.Normalize();

                float val = Vector3.Cross(pointTarget, transform.up).z;

                Rig.angularVelocity = RotateSpeed * val;

                Rig.velocity = transform.up * Speed;
            }
        }
    }

    public void ResetTarget()
    {
        Target = null;
        IsDisableTrace = false;
    }

    public void StartTraceTimer()
    {
        Invoke("DisableTrace", 12.0f);
    }

    void DisableTrace()
    {
        IsDisableTrace = true;
    }

    void OnDisable()
    {
        Trail.Clear();
    }
}
