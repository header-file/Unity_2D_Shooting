using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public int Index;

    RectTransform[] Pos;
    RectTransform RectT;

    void Awake()
    {
        Pos = new RectTransform[Bullet.MAXBULLETS];
        RectT = GetComponent<RectTransform>();

        for (int i = 0; i < Bullet.MAXBULLETS; i++)
            Pos[i] = transform.parent.GetChild(i).GetComponent<RectTransform>();
    }

    void Update()
    {
        
        //if (transform.position.x <= -3.4)
        //{
            

        //    int idx = Index + (Bullet.MAXBULLETS - 1);
        //    if (idx > Bullet.MAXBULLETS)
        //        idx -= Bullet.MAXBULLETS;

        //    Vector3 pos = Pos[idx].position;
        //    pos.x += 1.2f;
        //    //pos.y += 2.8f;
        //    RectT.position = pos;
        //}
    }

    public void SetSlotDetail(int Type)
    {
        
    }

    void OnMouseDrag()
    {
        Debug.Log(RectT.anchoredPosition);
    }
}
