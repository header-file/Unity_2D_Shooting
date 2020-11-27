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
            collision.gameObject.SetActive(false);
    }
}
