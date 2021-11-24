using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchArea : MonoBehaviour
{
    public enum ParentType
    {
        PLAYER = 0,
        ENEMY = 1
    }

    public ParentType PType;

    BossMissile ParentB;
    Missile ParentP;
    CircleCollider2D Col;

    void Awake()
    {
        ParentP = transform.parent.gameObject.GetComponent<Missile>();
        ParentB = transform.parent.gameObject.GetComponent<BossMissile>();
        Col = gameObject.GetComponent<CircleCollider2D>();

        if (ParentP != null)
            PType = ParentType.PLAYER;
        else if (ParentB != null)
            PType = ParentType.ENEMY;
    }

    void Update()
    {
        if (PType == ParentType.PLAYER)
        {
            if (ParentP.Target != null && !ParentP.Target.activeSelf)
                ParentP.Target = null;
        }
        else
        {
            if (ParentB.Target != null && !ParentB.Target.activeSelf)
                ParentB.Target = null;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(PType == ParentType.PLAYER)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                if (ParentP.Target == null || ParentP.Target.activeSelf == false)
                    ParentP.Target = collision.gameObject;
            }
        }
        else
        {
            if (collision.gameObject.tag == "Player" ||
                collision.gameObject.tag == "SubWeapon")
            {
                if (ParentB.Target == null || ParentB.Target.activeSelf == false)
                    ParentB.Target = collision.gameObject;
            }
        }
    }

    public void SetArea(float Radius)
    {
        Col.radius = Radius;
    }
}
