using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummaryScroll : MonoBehaviour
{
    public RectTransform Panel;
    public RectTransform[] Slots;
    public RectTransform Center;

    float[] Distances;
    float[] DistReposition;
    bool IsDragging = false;
    int SlotDistance;
    int MinBtnNum;
    int SlotLength;


    void Start()
    {
        SlotLength = Slots.Length;
        Distances = new float[SlotLength];
        DistReposition = new float[SlotLength];

        SlotDistance = (int)Mathf.Abs(Slots[1].GetComponent<RectTransform>().anchoredPosition.y -
                                    Slots[0].GetComponent<RectTransform>().anchoredPosition.y);
    }

    void Update()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            DistReposition[i] = Center.transform.position.y - Slots[i].transform.position.y;
            Distances[i] = Mathf.Abs(DistReposition[i]);

            if (DistReposition[i] > 6.0f)
            {
                float curX = Slots[i].anchoredPosition.x;
                float curY = Slots[i].anchoredPosition.y;

                Vector2 newAnchoredPos = new Vector2(curX, curY + (SlotLength * SlotDistance));
                Slots[i].anchoredPosition = newAnchoredPos;
            }

            if (DistReposition[i] < -6.0f)
            {
                float curX = Slots[i].anchoredPosition.x;
                float curY = Slots[i].anchoredPosition.y;

                Vector2 newAnchoredPos = new Vector2(curX, curY - (SlotLength * SlotDistance));
                Slots[i].anchoredPosition = newAnchoredPos;
            }
        }

        float minDistance = Mathf.Min(Distances);

        for (int i = 0; i < Slots.Length; i++)
        {
            if (minDistance == Distances[i])
                MinBtnNum = i;
        }

        if (!IsDragging)
            LerpToBtn(Center.anchoredPosition.y - Slots[MinBtnNum].anchoredPosition.y);
    }

    public void LerpToBtn(float position)
    {
        float newY = Mathf.Lerp(Panel.anchoredPosition.y, position, Time.deltaTime * 10.0f);
        Vector2 newPosition = new Vector2(Panel.anchoredPosition.x, newY);
        Panel.anchoredPosition = newPosition;
    }

    public void StartDrag()
    {
        IsDragging = true;
    }

    public void EndDrag()
    {
        IsDragging = false;
    }
}
