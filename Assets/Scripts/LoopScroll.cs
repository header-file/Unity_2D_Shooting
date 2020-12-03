using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoopScroll : MonoBehaviour
{
    public RectTransform Panel;     //To hold ScrollPanel
    public RectTransform[] Slots;
    public RectTransform[] Centers;    //Center To Compare the distance for each buttons

    float[] Distances;              //Distance of buttons compare to Center
    float[] DistReposition;
    bool IsDragging = false;        //True when drag the panel
    int SlotDistance;                //Hold the distance between buttons
    int MinBtnNum;                  //Hold the index of Button which is the nearest to Center
    int SlotLength;

    int CurrentCharacter;           //Selected Character
    float Timer;
    const float TickCount = 1.0f / 60.0f;

    public void SetCurrentCharacter(int i) { CurrentCharacter = i; }


    void Start()
    {
        SlotLength = Slots.Length;
        Distances = new float[SlotLength];
        DistReposition = new float[SlotLength];

        SlotDistance = (int)Mathf.Abs(Slots[1].GetComponent<RectTransform>().anchoredPosition.x -
                                    Slots[0].GetComponent<RectTransform>().anchoredPosition.x);

        CurrentCharacter = 2;
        Timer = 0.0f;
    }

    void Update()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            DistReposition[i] = Centers[CurrentCharacter].transform.position.x - Slots[i].transform.position.x;
            Distances[i] = Mathf.Abs(DistReposition[i]);

            if(DistReposition[i] > 3.8f)
            {
                float curX = Slots[i].anchoredPosition.x;
                float curY = Slots[i].anchoredPosition.y;

                Vector2 newAnchoredPos = new Vector2(curX + (SlotLength * SlotDistance), curY);
                Slots[i].anchoredPosition = newAnchoredPos;
            }

            if(DistReposition[i] < -3.8f)
            {
                float curX = Slots[i].anchoredPosition.x;
                float curY = Slots[i].anchoredPosition.y;

                Vector2 newAnchoredPos = new Vector2(curX - (SlotLength * SlotDistance), curY);
                Slots[i].anchoredPosition = newAnchoredPos;
            }
        }
            
        float minDistance = Mathf.Min(Distances);
        
        for (int i = 0; i < Slots.Length; i++)
        {
            if (minDistance == Distances[i])
            {
                MinBtnNum = i;
                GameManager.Inst().UiManager.SelectBullet(i);
            }
        }

        if (!IsDragging)
            LerpToBtn(Centers[CurrentCharacter].anchoredPosition.x - Slots[MinBtnNum].anchoredPosition.x);
    }

    public void MoveToSelected(int idx)
    {
        LerpToBtn(Centers[CurrentCharacter].anchoredPosition.x - Slots[idx].anchoredPosition.x);
    }

    public void LerpToBtn(float position)
    {
        float newX = Mathf.Lerp(Panel.anchoredPosition.x, position, Timer * 10.0f);
        Vector2 newPosition = new Vector2(newX, Panel.anchoredPosition.y);
        Panel.anchoredPosition = newPosition;
        if (Timer < 1.0f)
            Timer += TickCount;
    }

    public void StartDrag()
    {
        IsDragging = true;
    }

    public void EndDrag()
    {
        IsDragging = false;
        Timer = 0.0f;
    }
}
