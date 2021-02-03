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
        RectT.transform.position = Vector3.Lerp(RectT.transform.position, Vector3.zero, Time.deltaTime * 10.0f);

        if (Time.deltaTime > 0.1f)
            IsSummaryOn = false;
    }

    void SummaryOff()
    {
        RectT.transform.position = Vector3.Lerp(RectT.transform.position, OriginalPos, Time.deltaTime * 10.0f);

        if (Time.deltaTime > 0.1f)
            IsSummaryOff = false;
    }

    public void OnClickSummaryOn()
    {
        IsSummaryOn = true;
    }

    public void OnClickSummaryOff()
    {
        IsSummaryOff = true;
    }
}
