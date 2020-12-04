using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchArea : MonoBehaviour
{
    Missile Parent;
    CircleCollider2D Col;

    void Awake()
    {
        Parent = transform.parent.gameObject.GetComponent<Missile>();
        Col = gameObject.GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        /*if(Parent.Target != null)
            if (Parent.Target.gameObject.transform.position.x > 3.5f || Parent.Target.gameObject.transform.position.x < -3.5f ||
                Parent.Target.gameObject.transform.position.y > 6.0f || Parent.Target.gameObject.transform.position.y < -6.0f)
                Parent.Target = null;*/
         if(Parent.Target != null && !Parent.Target.activeSelf)
            Parent.Target = null;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (Parent.Target == null || Parent.Target.activeSelf == false)
            {
                /*if ((collision.gameObject.transform.position.x <= 3.5f && collision.gameObject.transform.position.x >= -3.5f) &&
                    (collision.gameObject.transform.position.y <= 6.0f && collision.gameObject.transform.position.y >= -6.0f))*/
                {
                    Parent.Target = collision.gameObject;
                }
            }
        }
    }

    public void SetArea(float Radius)
    {
        Col.radius = Radius;
    }
}
