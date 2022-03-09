using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossExplode : MonoBehaviour
{
    public int Damage;


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SubWeapon")
        {
            HitEffect(collision.ClosestPoint(transform.position));
            collision.gameObject.GetComponent<SubWeapon>().Damage(Damage);
        }
        else if (collision.gameObject.tag == "Player")
        {
            HitEffect(collision.ClosestPoint(transform.position));
            collision.gameObject.GetComponent<Player>().Damage(Damage);
        }
    }

    void HitEffect(Vector2 pos)
    {
        GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
        hit.transform.position = pos;
    }

    void Disappear()
    {
        transform.localScale = Vector3.one;
        gameObject.SetActive(false);
    }
}
