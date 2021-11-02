using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGauge : MonoBehaviour
{
    public RectTransform[] FeverZones;

    void Start()
    {
        
    }

    public void SetFeverZones(int index, float pos, float size)
    {
        Vector2 p = Vector2.zero;
        Vector2 s = Vector2.one * 12;
        p.x = (pos - 0.5f) * 600;
        s.x = size * 600;
        s.y = 46.0f;
        FeverZones[index].anchoredPosition = p;
        FeverZones[index].sizeDelta = s;
    }
}
