using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObj : MonoBehaviour
{
    public int index;
    int Count = 0;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(index == 0 && collision.tag == "BlockBullet")
        {
            Count++;

            if(Count >= 3)
            {
                collision.gameObject.SetActive(false);
                GameManager.Inst().Tutorials.Step++;
                gameObject.SetActive(false);
                Count = 0;
                return;
            }
        }
        else if(index == 1 && collision.tag == "Enemy")
        {
            if(GameManager.Inst().Tutorials.Step == 1)
            {
                collision.gameObject.SetActive(false);

                GameManager.Inst().Tutorials.EnemySpawn(0);
            }
        }
        else if(index == 2 && collision.tag == "Resource")
        {
            Count++;
            Debug.Log(Count);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (index == 2 && collision.tag == "Resource")
        {
            Count--;
            Debug.Log(Count);

            if (Count <= 0 && GameManager.Inst().Tutorials.Step == 2)
            {
                GameManager.Inst().Tutorials.Step++;
                gameObject.SetActive(false);
            }                
        }
    }
}
