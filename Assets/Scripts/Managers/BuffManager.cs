using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    float[] BuffTimers;

    
    void Awake()
    {
        BuffTimers = new float[Constants.MAXBUFFS];
    }

    void Update()
    {
        Count();
    }

    void Count()
    {
        for(int i = 0; i < Constants.MAXBUFFS; i++)
        {
            if (BuffTimers[i] <= 0.0f)
                continue;

            BuffTimers[i] -= Time.deltaTime;

            if (BuffTimers[i] <= 0.0f)
                EndBuff(i);
        }
    }

    public void StartBuff(int index)
    {
        switch (index)
        {
            case 0:
                GameManager.Inst().Player.StartAutoShot();
                BuffTimers[index] = 300.0f;
                break;
        }
    }

    void EndBuff(int index)
    {
        switch(index)
        {
            case 0:
                GameManager.Inst().Player.EndAutoShot();
                break;
        }
    }
}
