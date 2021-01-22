﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Player Player;

    public Vector3 MousePosition;

    bool IsAbleControl;
    bool IsAbleSWControl;

    public bool GetIsAbleControl() { return IsAbleControl; }
    public bool GetIsAbleSWControl() { return IsAbleSWControl; }

    public void SetIsAbleControl(bool b) { IsAbleControl = b; }
    public void SetIsAbleSWControl(bool b) { IsAbleSWControl = b; }


    void Start()
    {
        IsAbleControl = true;
        IsAbleSWControl = false;
    }

    void OnMouseDrag()
    {
        if (!IsAbleControl)
            return;

        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 MPos = new Vector2(MousePosition.x, MousePosition.y);

        if (!GameManager.Inst().Player.GetIsMovable())
            Player.Rotate(MPos);
        else
        {
            MousePosition.z = 0.0f;
            GameManager.Inst().Player.transform.position = Vector3.MoveTowards(GameManager.Inst().Player.transform.position, MousePosition, Time.deltaTime * 5.0f);
            //GameManager.Inst().Player.transform.position = MousePosition;
        }

        Player.Fire();
    }

    /*void OnMouseOver()
    {
        if (!IsAbleControl)
            return;

        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 MPos = new Vector2(MousePosition.x, MousePosition.y);

       // Player.Rotate(MPos);
    }*/
}
