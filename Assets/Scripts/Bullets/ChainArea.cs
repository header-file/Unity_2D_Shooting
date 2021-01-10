using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainArea : MonoBehaviour
{
    public GameObject[] Targets;
    public int MaxEnemy = 0;


    void Awake()
    {
        Targets = new GameObject[5];
        for(int i = 0; i < 5; i++)
            Targets[i] = null;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            for (int i = 0; i < MaxEnemy; i++)
            {
                if (Targets[i] == collision.gameObject)
                    return;
                else if (Targets[i] == null)
                {
                    Targets[i] = collision.gameObject;
                    return;
                } 
            }
                
        }   
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        for (int i = 0; i < MaxEnemy; i++)
        {
            if (Targets[i] == collision.gameObject)
                Targets[i] = null;
        }
    }

    public void ResetTargets()
    {
        for (int i = 0; i < MaxEnemy; i++)
            Targets[i] = null;
    }
}
