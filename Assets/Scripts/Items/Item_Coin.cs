using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Coin : Item
{
    void Start()
    {
        Type = ItemType.COIN;
        GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", new Color(0.67f, 0.67f, 0.32f));
    }
}
