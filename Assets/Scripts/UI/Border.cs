using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BlockBullet")
            collision.gameObject.SetActive(false);
        else if (collision.gameObject.tag == "PierceBullet")
        {
            if (collision.GetComponent<Boomerang>() == null)
                collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Chain")
            collision.gameObject.GetComponent<Chain>().Die();
        else if(collision.gameObject.tag == "EquipBullet")
            collision.gameObject.SetActive(false);
        else if (collision.gameObject.tag == "BossBullet")
            collision.gameObject.SetActive(false);
    }
}
