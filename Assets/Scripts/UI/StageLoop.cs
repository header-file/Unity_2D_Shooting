using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageLoop : MonoBehaviour
{
    public RectTransform Panel;
    public RectTransform[] Slots;
    public RectTransform Center;
    public PlanetSlot[] Planets;

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

        gameObject.SetActive(false);
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
                Planets[i].PlanetImage.transform.localScale = Vector3.one * 1.0f;
            }

            if (DistReposition[i] < -13.0f)
            {
                float curX = Slots[i].anchoredPosition.x;
                float curY = Slots[i].anchoredPosition.y;

                Vector2 newAnchoredPos = new Vector2(curX, curY - (SlotLength * SlotDistance));
                Slots[i].anchoredPosition = newAnchoredPos;
                Planets[i].PlanetImage.transform.localScale = Vector3.one * 2.0f;
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
    }

    public void StartDrag()
    {
        IsDragging = true;
    }

    public void Dragging(int index)
    {
        //행성 가로 이동
        Vector3 newPos = new Vector3((Slots[index].transform.position.y - 4.2f) / 1.5f, Planets[index].PlanetImage.transform.position.y, 0.0f);
        Planets[index].PlanetImage.transform.position = newPos;

        //행성 스케일
        float newScale = (Slots[index].transform.position.y - 4.2f) / 5.5f * -0.5f + 1.5f;
        Planets[index].PlanetImage.transform.localScale = Vector3.one * newScale;

        //행성 색
        float newR = Mathf.Abs((Slots[index].transform.position.y - 4.2f) / 5.5f) * -0.75f + 1.0f;
        Color newColor = new Color(newR, newR, newR);
        Planets[index].PlanetImage.color = newColor;

        //행성 이름
        float dist = Mathf.Abs(Vector3.Distance(Planets[index].transform.position, Center.transform.position));
        if (dist <= 1.0f)
        {
            if(!Planets[index].Name.gameObject.activeSelf)
                Planets[index].Name.gameObject.SetActive(true);
        }            
        else
        {
            if (Planets[index].Name.gameObject.activeSelf)
                Planets[index].Name.gameObject.SetActive(false);
        }

        if(Planets[index].Name.gameObject.activeSelf)
        {
            Color textColor = Planets[index].Name.color;
            textColor.a = 1.0f - dist;
            Planets[index].Name.color = textColor;
        }
    }

    public void EndDrag()
    {
        IsDragging = false;
    }
}
