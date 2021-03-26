using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeManager : MonoBehaviour
{
    public ObjectShake BgShaker;
    public float ShakeScale;
    public float ShakeTime = 0.05f;


    public void Damage()
    {
        BgShaker.Shake(ShakeTime, ShakeScale);

        GameManager.Inst().Player.Shaker.Shake(ShakeTime, ShakeScale);
        
        for (int i = 0; i < 4; i++)
        {
            if (GameManager.Inst().GetSubweapons(i) != null)
                GameManager.Inst().GetSubweapons(i).Shaker.Shake(ShakeTime, ShakeScale);
        }

        for (int i = 0; i < 4; i++)
            GameManager.Inst().Turrets[i].Shaker.Shake(ShakeTime, ShakeScale);
    }
}
