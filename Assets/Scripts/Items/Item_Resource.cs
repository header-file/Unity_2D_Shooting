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
    bool IsDeathCount;
    float DeathCount;
   

    void Awake()
    {
        Speed = 8.0f;
        IsDeathCount = false;
    }

    void Start()
    {
        Type = ItemType.RESOURCE;

        Rig = GetComponent<Rigidbody2D>();
        InitPos = GameObject.Find("ResourceGoal").transform.position;
    }

    protected override void Update()
    {
        if (IsScatter)
            Scatter();
        else if (IsAbsorb)
            Absorb();

        if (IsDeathCount)
            Count();
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
            stage = Random.Range(0, Constants.MAXSTAGES + 1);
        else if (GameManager.Inst().StgManager.Stage == 5)
            stage = Random.Range(1, 5);
        else
            stage = GameManager.Inst().StgManager.Stage;        

        GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", Colors[stage]);
        GetComponent<SpriteRenderer>().material.SetFloat("_Intensity", 1.5f);
    }

    public void SetColor(int type)
    {
        stage = type;

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
        BeginAbsorb();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BlockBullet" || collision.gameObject.tag == "PierceBullet" ||
            collision.gameObject.tag == "Chain" || collision.gameObject.tag == "Laser" ||
            collision.gameObject.tag == "Explosion" || collision.gameObject.tag == "Dot" ||
            collision.gameObject.tag == "Border")
        {
            BeginAbsorb();
        }
        else if (collision.gameObject.name == "ResourceGoal")
        {
            Add();
            GameManager.Inst().UiManager.MainUI.ResourceAnim.Play();
            GameManager.Inst().SodManager.PlayEffect("Resource get");

            //퀘스트 처리
            GameManager.Inst().QstManager.QuestProgress((int)QuestManager.QuestType.RESOURCE, GameManager.Inst().StgManager.Stage, Value);
        }
    }

    public void BeginAbsorb()
    {
        Invoke("Add", 5.0f);
        IsAbsorb = true;
    }

    public void InvokeDisappear()
    {
        //Invoke("Disappear", 2.0f);
        IsDeathCount = true;
        DeathCount = 10.0f;
    }

    void Count()
    {
        DeathCount -= Time.deltaTime;

        if (DeathCount <= 0.0f)
            Disappear();
    }

    void Disappear()
    {
        if (IsAbsorb || !gameObject.activeSelf)
            return;
        GameObject resourceDie = GameManager.Inst().ObjManager.MakeObj("ResourceDie");
        resourceDie.transform.position = transform.position;
        
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        IsScatter = false;
        IsAbsorb = false;
        IsDeathCount = false;
        DeathCount = 2.0f;
    }

    void OnDisable()
    {
        IsScatter = false;
        IsAbsorb = false;
        IsDeathCount = false;
        DeathCount = 2.0f;
    }
}
