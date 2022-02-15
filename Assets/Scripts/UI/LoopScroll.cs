using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoopScroll : MonoBehaviour
{
    public RectTransform Panel;     //To hold ScrollPanel
    public RectTransform[] Slots;
    public RectTransform[] Centers;    //Center To Compare the distance for each buttons
    public ActivationTimer ErrorMsg;
    public GameObject ChangeMsg;

    public bool IsOpen;
    public bool IsDragging = false;        //True when drag the panel

    float[] Distances;              //Distance of buttons compare to Center
    float[] DistReposition;
    int SlotDistance;                //Hold the distance between buttons
    int MinBtnNum;                  //Hold the index of Button which is the nearest to Center
    int SlotLength;

    int CurrentCharacter;           //Selected Character
    float Timer;
    const float TickCount = 1.0f / 60.0f;
    bool IsMoving;
    int GoalIndex;

    int[] CurrentNum;
    int[] SelectedNum;

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

        CurrentNum = new int[2];
        for (int i = 0; i < 2; i++)
            CurrentNum[i] = -1;
        SelectedNum = new int[2];
        for (int i = 0; i < 2; i++)
            SelectedNum[i] = -1;
        ChangeMsg.SetActive(false);

        IsOpen = false;
        IsMoving = false;

        GameManager.Inst().UiManager.MainUI.Bottom.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!IsOpen)
            return;

        for (int i = 0; i < Slots.Length; i++)
        {
            DistReposition[i] = Centers[CurrentCharacter].transform.position.x - Slots[i].transform.position.x;
            Distances[i] = Mathf.Abs(DistReposition[i]);

            if(DistReposition[i] > 8.0f)
            {
                float curX = Slots[i].anchoredPosition.x;
                float curY = Slots[i].anchoredPosition.y;

                Vector2 newAnchoredPos = new Vector2(curX + (SlotLength * SlotDistance), curY);
                Slots[i].anchoredPosition = newAnchoredPos;
            }

            if(DistReposition[i] < -8.0f)
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
                GameManager.Inst().UiManager.MainUI.Bottom.ShowEquipBtn(MinBtnNum);
                //GameManager.Inst().UiManager.SelectBullet(i);
            }
        }

        if(IsMoving)
            LerpToBtn(Centers[CurrentCharacter].anchoredPosition.x - Slots[GoalIndex].anchoredPosition.x);
        else if (!IsDragging)
            LerpToBtn(Centers[CurrentCharacter].anchoredPosition.x - Slots[MinBtnNum].anchoredPosition.x);
    }

    public void MoveToSelected(int idx)
    {
        IsMoving = true;
        GoalIndex = idx;
        Timer = 0.0f;
    }

    public void LerpToBtn(float position)
    {
        float newX = Mathf.Lerp(Panel.anchoredPosition.x, position, Timer * 10.0f);
        Vector2 newPosition = new Vector2(newX, Panel.anchoredPosition.y);
        Panel.anchoredPosition = newPosition;
        if (Timer < 1.0f)
            Timer += TickCount;
        else if(IsMoving)
            IsMoving = false;
    }

    public void StartDrag()
    {
        GameManager.Inst().UiManager.MainUI.Bottom.WeaponScroll.IsDragging = true;
    }

    public void EndDrag()
    {
        GameManager.Inst().UiManager.MainUI.Bottom.WeaponScroll.IsDragging = false;
        Timer = 0.0f;
    }

    public int OnClickEquipBtn()
    {
        if(GameManager.Inst().Player.GetBulletType() == MinBtnNum)
        {
            CurrentNum[0] = CurrentCharacter;
            if (CurrentCharacter > 2)
                CurrentNum[1] = GameManager.Inst().GetSubweapons(CurrentCharacter - 1).GetBulletType();
            else
                CurrentNum[1] = GameManager.Inst().GetSubweapons(CurrentCharacter).GetBulletType();
            SelectedNum[0] = 2;
            SelectedNum[1] = GameManager.Inst().Player.GetBulletType();

            ChangeMsg.SetActive(true);
            return -1;
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                if (GameManager.Inst().GetSubweapons(i) == null)
                    continue;

                if(GameManager.Inst().GetSubweapons(i).GetBulletType() == MinBtnNum)
                {
                    CurrentNum[0] = CurrentCharacter;
                    if (CurrentCharacter == 2)
                        CurrentNum[1] = GameManager.Inst().Player.GetBulletType();
                    else
                    {
                        if (CurrentCharacter > 2)
                            CurrentNum[1] = GameManager.Inst().GetSubweapons(CurrentCharacter - 1).GetBulletType();
                        else
                            CurrentNum[1] = GameManager.Inst().GetSubweapons(CurrentCharacter).GetBulletType();
                    }

                    SelectedNum[1] = GameManager.Inst().GetSubweapons(i).GetBulletType();
                    if (i >= 2)
                        SelectedNum[0] = i + 1;
                    else
                        SelectedNum[0] = i;

                    ChangeMsg.SetActive(true);
                    return -1;
                }
            }
        }

        GameManager.Inst().UiManager.MainUI.Bottom.OnClickSelectBullet(MinBtnNum);

        return MinBtnNum;
    }

    public int OnClickYesBtn()
    {
        if(CurrentNum[0] == 2)
        {
            GameManager.Inst().Player.SetBulletType(SelectedNum[1]);

            if (SelectedNum[0] > 2)
                GameManager.Inst().GetSubweapons(SelectedNum[0] - 1).SetBulletType(CurrentNum[1]);
            else
                GameManager.Inst().GetSubweapons(SelectedNum[0]).SetBulletType(CurrentNum[1]);
        }
        else 
        {
            if (CurrentNum[0] > 2)
                GameManager.Inst().GetSubweapons(CurrentNum[0] - 1).SetBulletType(SelectedNum[1]);
            else
                GameManager.Inst().GetSubweapons(CurrentNum[0]).SetBulletType(SelectedNum[1]);

            if (SelectedNum[0] == 2)
                GameManager.Inst().Player.SetBulletType(CurrentNum[1]);
            else if (SelectedNum[0] > 2)
                GameManager.Inst().GetSubweapons(SelectedNum[0] - 1).SetBulletType(CurrentNum[1]);
            else
                GameManager.Inst().GetSubweapons(SelectedNum[0]).SetBulletType(CurrentNum[1]);
        }

        int temp = SelectedNum[1];
        OnClickNoBtn();

        return temp;
    }

    public void OnClickNoBtn()
    {
        for (int i = 0; i < 2; i++)
            CurrentNum[i] = -1;
        SelectedNum = new int[2];
        for (int i = 0; i < 2; i++)
            SelectedNum[i] = -1;

        ChangeMsg.SetActive(false);
    }
}
