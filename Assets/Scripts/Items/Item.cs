using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : MonoBehaviour
{
    protected enum ItemType
    {
        COIN = 0,
        EQUIPMENT = 1,
    }

    protected ItemType Type;
    protected int Value;

    Vector3 TargetPos;
    bool IsStart;
    GameObject Player;


    public int GetItemType() { return (int)Type; }
    public int GetValue() { return Value; }

    public void SetValue(int v) { Value = v; }

    void Awake()
    {
        Player = GameObject.Find("Player");

        IsStart = false;
    }

    public void StartAbsorb()
    {
        Invoke("SetStartTrue", 1.0f);
    }

    void SetStartTrue()
    {
        IsStart = true;
    }

    protected void Update()
    {
        if (IsStart)
            Absorb();
    }

    void Absorb()
    {
        transform.position = Vector3.Lerp(transform.position, Player.transform.position, Time.deltaTime * 2.0f);
    }

    private void OnMouseOver()
    {
        SetStartTrue();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            switch (Type)
            {
                case ItemType.COIN:
                    GameManager.Inst().Player.AddCoin(Value);
                    break;

                case ItemType.EQUIPMENT:
                    GameManager.Inst().Player.AddItem(gameObject.GetComponent<Item_Equipment>());
                    GameManager.Inst().Player.Sort();
                    break;
            }

            gameObject.SetActive(false);
        }
    }
}
