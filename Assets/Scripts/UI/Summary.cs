using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summary : MonoBehaviour
{
    RectTransform RectT;
    Vector3 OriginalPos;
    bool IsSummaryOn;
    bool IsSummaryOff;

    void Start()
    {
        RectT = GetComponent<RectTransform>();
        OriginalPos = new Vector3(720.0f, 0.0f, 0.0f);
        IsSummaryOn = false;
    }

    void Update()
    {
        if (IsSummaryOn)
            SummaryOn();
        if (IsSummaryOff)
            SummaryOff();
    }

    void SummaryOn()
    {
        RectT.anchoredPosition = Vector3.Lerp(RectT.anchoredPosition, Vector3.zero, Time.deltaTime * 10.0f);

        if (Time.deltaTime >= 0.1f)
            IsSummaryOn = false;
    }

    void SummaryOff()
    {
        RectT.anchoredPosition = Vector3.Lerp(RectT.anchoredPosition, OriginalPos, Time.deltaTime * 10.0f);

        if (Time.deltaTime >= 0.1f)
            IsSummaryOff = false;
    }

    public void OnClickSummaryOn()
    {
        IsSummaryOff = false;
        IsSummaryOn = true;
    }

    public void OnClickSummaryOff()
    {
        IsSummaryOn = false;
        IsSummaryOff = true;
    }
}
