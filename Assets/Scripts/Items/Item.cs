using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : MonoBehaviour
{
    protected enum ItemType
    {
        COIN = 0,
        EQUIPMENT = 1,
        SHIELD = 2,
        BOMB = 3,
        RESOURCE = 10,
    }

    protected ItemType Type;
    protected int Value;
    protected bool IsStart;

    Vector3 TargetPos;
    GameObject Player;
    int UID;


    public int GetItemType() { return (int)Type; }
    public int GetValue() { return Value; }
    public int GetUID() { return UID; }

    public void SetValue(int v) { Value = v; }
    public void SetUID(int id) { UID = id; }

    void Awake()
    {
        Player = GameObject.Find("Player");

        IsStart = false;
    }

    public void StartAbsorb(float time)
    {
        Invoke("SetStartTrue", time);
    }

    void SetStartTrue()
    {
        IsStart = true;
    }

    protected virtual void Update()
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
                    gameObject.GetComponent<Item_Coin>().ResetData();
                    break;

                case ItemType.EQUIPMENT:
                    GameManager.Inst().Player.AddItem(gameObject.GetComponent<Item_Equipment>());
                    break;
                case ItemType.SHIELD:
                    GameManager.Inst().Player.Shield();
                    break;
                case ItemType.BOMB:
                    gameObject.GetComponent<Item_Bomb>().Bomb();
                    break;
            }

            IsStart = false;
            gameObject.SetActive(false);
        }
    }
}
