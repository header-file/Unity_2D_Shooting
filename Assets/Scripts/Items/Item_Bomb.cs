using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Bomb : Item
{
    void Start()
    {
        Type = ItemType.BOMB;
    }

    protected override void Update()
    {
        base.Update();
    }

    public void Bomb()
    {
        GameObject bomb = GameManager.Inst().ObjManager.MakeObj("Bomb");
        bomb.transform.position = transform.position;

        bomb.GetComponent<Bomb>().BombStart();

        GameManager.Inst().SodManager.PlayEffect("Eq_Explosive");

        gameObject.SetActive(false);
    }
}
