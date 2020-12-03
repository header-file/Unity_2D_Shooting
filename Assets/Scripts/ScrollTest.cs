using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollTest : MonoBehaviour
{
    public RectTransform Panel;     //To hold ScrollPanel
    public Button[] Btns;
    public RectTransform Center;    //Center To Compare the distance for each buttons

    public float[] Distances;              //Distance of buttons compare to Center
    public float[] DistReposition;         //
    bool IsDragging = false;        //True when drag the panel
    int BtnDistance;                //Hold the distance between buttons
    int MinBtnNum;                  //Hold the index of Button which is the nearest to Center
    int BtnLength;

    void Start()
    {
        BtnLength = Btns.Length;
        Distances = new float[BtnLength];
        DistReposition = new float[BtnLength];

        BtnDistance = (int)Mathf.Abs(Btns[1].GetComponent<RectTransform>().anchoredPosition.x - 
                                    Btns[0].GetComponent<RectTransform>().anchoredPosition.x);
        
    }

    void Update()
    {
        for(int i = 0; i < Btns.Length; i++)
        {
            DistReposition[i] = Center.transform.position.x - Btns[i].GetComponent<RectTransform>().position.x;
            Distances[i] = Mathf.Abs(DistReposition[i]);

            if(DistReposition[i] > 500.0f)
            {
                float curX = Btns[i].GetComponent<RectTransform>().anchoredPosition.x;
                float curY = Btns[i].GetComponent<RectTransform>().anchoredPosition.y;

                Vector2 newAnchoredPos = new Vector2(curX + (BtnLength * BtnDistance), curY);
                Btns[i].GetComponent<RectTransform>().anchoredPosition = newAnchoredPos;
            }

            if(DistReposition[i] < -500)
            {
                float curX = Btns[i].GetComponent<RectTransform>().anchoredPosition.x;
                float curY = Btns[i].GetComponent<RectTransform>().anchoredPosition.y;

                Vector2 newAnchoredPos = new Vector2(curX - (BtnLength * BtnDistance), curY);
                Btns[i].GetComponent<RectTransform>().anchoredPosition = newAnchoredPos;
            }
        }
            
        
        float minDistance = Mathf.Min(Distances);

        for (int i = 0; i < Btns.Length; i++)
        {
            if(minDistance == Distances[i])
            {
                MinBtnNum = i;
            }
        }

        if(!IsDragging)
        {
            //LerpToBtn(MinBtnNum * -BtnDistance);
            LerpToBtn(-Btns[MinBtnNum].GetComponent<RectTransform>().anchoredPosition.x);
        }
    }

    void LerpToBtn(float position)
    {
        float newX = Mathf.Lerp(Panel.anchoredPosition.x, position, Time.deltaTime * 10.0f);
        Vector2 newPosition = new Vector2(newX, Panel.anchoredPosition.y);

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
