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
        InitPos = GameObject.Find("ResourceGoal").transform.position;
        Speed = 8.0f;
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
        transform.position = Vector3.Lerp(transform.position, TargetPosition, Time.deltaTime * 10.0f);

        if (Vector3.Distance(transform.position, TargetPosition) <= 0.001f)
        {
            IsScatter = false;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
        }
            
    }

    void Absorb()
    {
        Vector2 pointTarget = (Vector2)transform.position - (Vector2)InitPos;
        pointTarget.Normalize();

        float val = Vector3.Cross(pointTarget, transform.up).z;

        Rig.angularVelocity = 200.0f * val;

        Rig.velocity = transform.up * Speed;
    }

    public void SetColor()
    {
        if(GameManager.Inst().StgManager.Stage == 0)
            stage = Random.Range(0, 4);
        else
            stage = GameManager.Inst().StgManager.Stage;

        GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", Colors[stage]);
        GetComponent<SpriteRenderer>().material.SetFloat("_Intensity", 1.5f);
    }

    void Add()
    {
        if (!gameObject.activeSelf)
            return;

        if (GameManager.Inst().StgManager.Stage == 0)
            stage++;
        GameManager.Inst().AddResource(stage, Value);
        IsAbsorb = false;
        gameObject.SetActive(false);
    }

    void OnMouseDown()
    {
        Invoke("Add", 5.0f);
        IsAbsorb = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BlockBullet" || collision.gameObject.tag == "PierceBullet" ||
            collision.gameObject.tag == "Chain" || collision.gameObject.tag == "Laser" ||
            collision.gameObject.tag == "Border")
        {
            Invoke("Add", 5.0f);
            IsAbsorb = true;
        }
        else if (collision.gameObject.name == "ResourceGoal")
        {
            Add();

            //퀘스트 처리
            GameManager.Inst().QstManager.QuestProgress((int)QuestManager.QuestType.RESOURCE, GameManager.Inst().StgManager.Stage, Value);
        }
    }
}
