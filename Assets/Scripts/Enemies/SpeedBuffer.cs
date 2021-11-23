using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuffer : MonoBehaviour
{
    public Enemy Enemy;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Right" || collision.gameObject.name == "Left")
        {
            //Enemy.CurveTime = 10.0f;
        }
    }
}
