using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainArea : MonoBehaviour
{
    public GameObject Target;

    void Awake()
    {
        Target = null;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Target = collision.gameObject;
        }   
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (Target == collision.gameObject)
            Target = null;
    }
}
