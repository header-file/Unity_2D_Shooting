using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : MonoBehaviour
{
    protected enum ItemType
    {
        COIN = 0,
        POWER = 1,
    }

    protected ItemType Type;
    protected int Value;

    Vector3 TargetPos;
    bool IsStart;
    GameObject Player;
    //float Timer;

    public int GetItemType() { return (int)Type; }
    public int GetValue() { return Value; }

    public void SetValue(int v) { Value = v; }

    void Awake()
    {
        Player = GameObject.Find("Player");
        TargetPos = Player.transform.position;

        IsStart = true;
        //Timer = 0.0f;
    }

    void StartAbsorb()
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
        transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * 2.0f);
    }

    private void OnMouseOver()
    {
        StartAbsorb();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player player = Player.GetComponent<Player>();
            switch (Type)
            {
                case ItemType.COIN:
                    player.AddCoin(Value);
                    break;
            }

            gameObject.SetActive(false);
        }
    }
}
