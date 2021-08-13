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

    Missile Parent;
    CircleCollider2D Col;

    void Awake()
    {
        Parent = transform.parent.gameObject.GetComponent<Missile>();
        Col = gameObject.GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if(Parent.Target != null && !Parent.Target.activeSelf)
            Parent.Target = null;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(PType == ParentType.PLAYER)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                if (Parent.Target == null || Parent.Target.activeSelf == false)
                    Parent.Target = collision.gameObject;
            }
        }
        else
        {
            if (collision.gameObject.tag == "Player" ||
                collision.gameObject.tag == "SubWeapon")
            {
                if (Parent.Target == null || Parent.Target.activeSelf == false)
                    Parent.Target = collision.gameObject;
            }
        }
    }

    public void SetArea(float Radius)
    {
        Col.radius = Radius;
    }
}
