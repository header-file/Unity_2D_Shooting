using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Player Player;

    bool IsAbleControl;

    public bool GetIsAbleControl() { return IsAbleControl; }

    public void SetIsAbleControl(bool b) { IsAbleControl = b; }


    void Start()
    {
        IsAbleControl = true;
    }

    void OnMouseDrag()
    {
        if (!IsAbleControl)
            return;

        Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 MPos = new Vector2(MousePos.x, MousePos.y);

        Player.Rotate(MPos);
        Player.Fire(MPos);
    }

    void OnMouseOver()
    {
        if (!IsAbleControl)
            return;

        Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 MPos = new Vector2(MousePos.x, MousePos.y);

       // Player.Rotate(MPos);
    }
}
