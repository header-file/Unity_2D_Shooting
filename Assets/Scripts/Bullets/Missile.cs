using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Bullet
{
    public GameObject Target;
    public GameObject SearchArea;
    
    Vector3 InitPos;
    Rigidbody2D Rig;

    float RotateSpeed;
    float Speed;
    

    void Awake()
    {
        InitPos = new Vector3(0.0f, 12.0f, 0.0f);
        Rig = GetComponent<Rigidbody2D>();

        Damage = 1.0f;
        Type = BulletType.MISSILE;
        GetComponent<SpriteRenderer>().color = Color.white;
        //Damage *= (float)GameManager.Inst().UpgManager.GetBData((int)Type).GetPowerLevel();
        Speed = GameManager.Inst().UpgManager.GetBData((int)Type).GetSpeed();
        RotateSpeed = 200.0f;
    }

    void Update()
    {
        if(Target != null)
        {
            Vector2 pointTarget = (Vector2)transform.position - (Vector2)Target.transform.position;
            pointTarget.Normalize();

            float val = Vector3.Cross(pointTarget, transform.up).z;

            Rig.angularVelocity = RotateSpeed * val;

            Rig.velocity = transform.up * Speed;
        }
        else if (Target == null)
        {
            Vector2 pointTarget = (Vector2)transform.position - (Vector2)InitPos;
            pointTarget.Normalize();

            float val = Vector3.Cross(pointTarget, transform.up).z;

            //Rig.angularVelocity = RotateSpeed * val;

            Rig.velocity = transform.up * Speed;
            Rig.angularVelocity = 0.0f;
        }
    }

    public void ResetTarget()
    {
        Target = null;
    }
}
