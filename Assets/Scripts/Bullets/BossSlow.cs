using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlow : Bullet
{
    int Damage;

    void Awake()
    {
        Type = BulletType.SLOW;
        Damage = 5 * GameManager.Inst().StgManager.Stage;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            HitEffect(collision.gameObject);
            GameManager.Inst().Player.Damage(Damage);

            if (GameManager.Inst().IptManager.SpeedMultiplier >= 1.0f)
                GameManager.Inst().IptManager.SpeedMultiplier = 0.5f;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (GameManager.Inst().IptManager.SpeedMultiplier < 1.0f)
                GameManager.Inst().IptManager.SpeedMultiplier = 1.0f;
        }
    }

    void HitEffect(GameObject obj)
    {
        GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
        hit.transform.position = obj.transform.position;
    }
}
