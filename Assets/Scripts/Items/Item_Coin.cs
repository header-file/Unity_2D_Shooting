using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Coin : Item
{
   Rigidbody2D Rig;

    bool IsScatter = false;
    Vector3 TargetPosition = Vector3.zero;
    float RotateSpeed;

    public void SetIsScatter(bool b) { IsScatter = b; }
    public void SetTargetPosition(Vector3 pos) { TargetPosition = pos; }
    public void InvokeAbsorb() { Invoke("Absorb", 0.5f); }

    void Start()
    {
        Type = ItemType.COIN;
        GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", new Color(0.67f, 0.67f, 0.32f));
        RotateSpeed = 400.0f;
        Rig = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        //base.Update();

        if (IsScatter)
            Scatter();
        else
        {
            Vector2 pointTarget = (Vector2)transform.position - (Vector2)GameManager.Inst().Player.transform.position;
            pointTarget.Normalize();

            float val = Vector3.Cross(pointTarget, transform.up).z;

            Rig.angularVelocity = RotateSpeed * val;

            Rig.velocity = transform.up * 5.0f;
        }
    }

    void Scatter()
    {
        transform.position = Vector3.Lerp(transform.position, TargetPosition, Time.deltaTime * 10.0f);

        if (Vector3.Distance(transform.position, TargetPosition) <= 0.05f)
        {
            IsScatter = false;
            //Absorb();
        }
    }

    void Absorb()
    {
        IsScatter = false;
        base.IsStart = true;
    }
}
