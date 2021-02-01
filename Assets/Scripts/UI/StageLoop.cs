using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageLoop : MonoBehaviour
{
    public RectTransform Panel;
    public RectTransform[] Slots;
    public RectTransform Center;
    public GameObject[] Planets;

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

            if (DistReposition[i] > 13.0f)
            {
                float curX = Slots[i].anchoredPosition.x;
                float curY = Slots[i].anchoredPosition.y;

                Vector2 newAnchoredPos = new Vector2(curX, curY + (SlotLength * SlotDistance));
                Slots[i].anchoredPosition = newAnchoredPos;
                Planets[i].transform.localScale = Vector3.one * 1.0f;
            }

            if (DistReposition[i] < -13.0f)
            {
                float curX = Slots[i].anchoredPosition.x;
                float curY = Slots[i].anchoredPosition.y;

                Vector2 newAnchoredPos = new Vector2(curX, curY - (SlotLength * SlotDistance));
                Slots[i].anchoredPosition = newAnchoredPos;
                Planets[i].transform.localScale = Vector3.one * 2.0f;
            }
            
            Dragging(i);
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

        //Vector3 newScale = Vector3.Lerp()
    }

    public void StartDrag()
    {
        IsDragging = true;
    }

    public void Dragging(int index)
    {
        Vector3 newPos = new Vector3(Slots[index].transform.position.y / 2.0f, Planets[index].transform.position.y, 0.0f);
        Planets[index].transform.position = newPos;

        float newScale = Slots[index].transform.position.y / 5.5f * -0.5f + 1.5f;
        Planets[index].transform.localScale = Vector3.one * newScale;

        float newR = Mathf.Abs(Slots[index].transform.position.y / 5.5f) * -0.75f + 1.0f;
        Color newColor = new Color(newR, newR, newR);
        Planets[index].GetComponent<Image>().color = newColor;
    }

    public void EndDrag()
    {
        IsDragging = false;
    }
}
