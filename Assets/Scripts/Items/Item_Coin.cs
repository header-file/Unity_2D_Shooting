using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Coin : Item
{
    Rigidbody2D Rig;

    bool IsScatter = false;
    bool IsAbsorb = false;
    Vector3 TargetPosition = Vector3.zero;
    float RotateSpeed;

    public void SetIsScatter(bool b) { IsScatter = b; }
    public void SetIsAbsorb(bool b) { IsAbsorb = b; }
    public void SetTargetPosition(Vector3 pos) { TargetPosition = pos; }
    public void InvokeAbsorb() { Invoke("Timeup", 1.0f); }

    void Start()
    {
        Type = ItemType.COIN;
        GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", new Color(0.67f, 0.67f, 0.32f));
        RotateSpeed = 400.0f;
        Rig = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        if (IsAbsorb)
            Absorb();
        else if (IsScatter)
            Scatter();
        else
            Homing();
    }

    void Scatter()
    {
        transform.position = Vector3.Lerp(transform.position, TargetPosition, Time.deltaTime * 10.0f);

        if (Vector3.Distance(transform.position, TargetPosition) <= 0.05f)
        {
            IsScatter = false;
            Invoke("Timeup", 3.5f);
        }
    }

    void Homing()
    {
        Vector2 pointTarget = (Vector2)transform.position - (Vector2)GameManager.Inst().Player.transform.position;
        pointTarget.Normalize();

        float val = Vector3.Cross(pointTarget, transform.up).z;

        Rig.angularVelocity = RotateSpeed * val;

        Rig.velocity = transform.up * 5.0f;
    }

    void Timeup()
    {
        IsAbsorb = true;
        Invoke("Disappear", 5.0f);
    }

    void Absorb()
    {
        transform.position = Vector3.Lerp(transform.position, GameManager.Inst().Player.transform.position, Time.deltaTime * 1.0f);
    }

    void Disappear()
    {
        ResetData();
        gameObject.SetActive(false);
    }

    public void ResetData()
    {
        IsAbsorb = false;
        IsScatter = false;
    }
}
