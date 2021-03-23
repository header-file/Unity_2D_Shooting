using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitArea : MonoBehaviour
{
    public GameObject[] HitObjects;

    int Count;

    void Awake()
    {
        HitObjects = new GameObject[5];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
            return;

        //if (collision.gameObject.tag == "SubWeapon" || collision.gameObject.tag == "Player")
        if(collision.gameObject.tag == "PlayerHitArea")
        {
            for (int i = 0; i < 5; i++)
            {
                if (HitObjects[i] == null)
                {
                    HitObjects[i] = collision.GetComponent<PlayerHitArea>().Object;
                    return;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
            return;

        //if (collision.gameObject.tag == "SubWeapon" || collision.gameObject.tag == "Player")
        if (collision.gameObject.tag == "PlayerHitArea")
        {
            for (int i = 0; i < 5; i++)
            {
                if (HitObjects[i] == collision.GetComponent<PlayerHitArea>().Object)
                {
                    HitObjects[i] = null;
                    //return;
                }
            }
        }
    }
}
