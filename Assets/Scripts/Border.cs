using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BlockBullet")
            collision.gameObject.SetActive(false);
        else if(collision.gameObject.tag == "PierceBullet")
        {
            if(collision.gameObject.name != "Boomerang(Clone)")
                collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Chain")
            collision.gameObject.GetComponent<Chain>().Die();
            
    }
}
