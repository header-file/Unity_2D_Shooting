using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Resource : Item
{
    public Color[] Colors;
    public Vector3 TargetPosition;
    public bool IsScatter;

    Rigidbody2D Rig;
    Vector3 InitPos;
    bool IsAbsorb;
    float Speed;
    int stage;

    void Start()
    {
        Type = ItemType.RESOURCE;

        Rig = GetComponent<Rigidbody2D>();
        InitPos = new Vector3(-1.2f, 8.9f, 0.0f);
        Speed = 5.0f;
    }

    protected override void Update()
    {
        if (IsScatter)
            Scatter();
        else if (IsAbsorb)
            Absorb();
    }

    void Scatter()
    {
        transform.position = Vector3.Lerp(transform.position, TargetPosition, Time.deltaTime * 2.0f);

        if (Vector3.Distance(transform.position, TargetPosition) <= 0.001f)
            IsScatter = false;
    }

    void Absorb()
    {
        Vector2 pointTarget = (Vector2)transform.position - (Vector2)InitPos;
        pointTarget.Normalize();

        float val = Vector3.Cross(pointTarget, transform.up).z;

        Rig.angularVelocity = 250.0f * val;

        Rig.velocity = transform.up * Speed;
    }

    public void SetColor()
    {
        if(GameManager.Inst().StgManager.Stage == 0)
            stage = Random.Range(0, 4);
        else
            stage = GameManager.Inst().StgManager.Stage;

        GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", Colors[stage]);
    }

    void OnMouseDrag()
    {
        IsAbsorb = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BlockBullet" || collision.gameObject.tag == "PierceBullet" ||
            collision.gameObject.tag == "Chain" || collision.gameObject.tag == "Laser")
            IsAbsorb = true;
        else if (collision.gameObject.name == "ResourceGoal")
        {
            if (GameManager.Inst().StgManager.Stage == 0)
                stage++;
            GameManager.Inst().AddResource(stage, Value);
            IsAbsorb = false;
            gameObject.SetActive(false);
        }
    }
}
