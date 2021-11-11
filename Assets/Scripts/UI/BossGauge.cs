using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGauge : MonoBehaviour
{
    public GameObject[] FeverZones;

    void Start()
    {
        //for (int i = 0; i < FeverZones.Length; i++)
        //    FeverZones[i].gameObject.SetActive(false);

        GameManager.Inst().StgManager.SetFeverGauge();
        GameManager.Inst().StgManager.FillGauge();
    }

    public void SetFeverZones(int index, float pos, float size)
    {
        Vector2 p = Vector2.zero;
        Vector2 s = Vector2.one * 12;
        p.x = (pos - 0.5f) * 600;
        s.x = size * 600;
        s.y = 46.0f;

        FeverZones[index].GetComponent<RectTransform>().anchoredPosition = p;
        FeverZones[index].GetComponent<RectTransform>().sizeDelta = s;
    }
}
