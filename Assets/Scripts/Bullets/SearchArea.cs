using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchArea : MonoBehaviour
{
    public enum ParentType
    {
        PLAYER = 0,
        ENEMY = 1
    }

    public ParentType PType;
    public GameObject Parent;

    BossMissile BM;
    Missile MS;
    Chain CH;
    CircleCollider2D Col;

    void Awake()
    {
        Col = gameObject.GetComponent<CircleCollider2D>();
        Parent = gameObject.transform.parent.gameObject;

        if (PType == ParentType.PLAYER)
        {
            if (Parent.GetComponent<Missile>())
                MS = Parent.GetComponent<Missile>();
            else
                CH = Parent.GetComponent<Chain>();
        }
        else
        {
            if (Parent.GetComponent<BossMissile>())
                BM = Parent.GetComponent<BossMissile>();
        }
    }

    void Update()
    {
        if (MS != null)
        {
            if (MS.Target != null && !MS.Target.activeSelf)
                MS.Target = null;
        }
        else if (CH != null)
        {
            if (CH.Target != null && !CH.Target.activeSelf)
                CH.Target = null;
        }
        else if (BM != null)
        {
            if (BM.Target != null && !BM.Target.activeSelf)
                BM.Target = null;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(PType == ParentType.PLAYER)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                if (MS != null)
                {
                    if(MS.Target == null || MS.Target.activeSelf == false)
                        MS.Target = collision.gameObject;
                }                   
                else if(CH != null)
                {
                    //if (CH.Target == null || CH.Target.activeSelf == false)
                        CH.Target = collision.gameObject;
                }
            }
        }
        else
        {
            if (collision.gameObject.tag == "Player" ||
                collision.gameObject.tag == "SubWeapon")
            {
                if (BM.Target == null || BM.Target.activeSelf == false)
                    BM.Target = collision.gameObject;
            }
        }
    }

    public void SetArea(float Radius)
    {
        Col.radius = Radius;
    }
}
