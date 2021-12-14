using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public Vector3 MousePosition;
    public float SpeedMultiplier = 1.0f;

    bool IsAbleControl;
    bool IsAbleSWControl;

    public bool GetIsAbleControl() { return IsAbleControl; }
    public bool GetIsAbleSWControl() { return IsAbleSWControl; }

    public void SetIsAbleControl(bool b) { IsAbleControl = b; }
    public void SetIsAbleSWControl(bool b) { IsAbleSWControl = b; }

    void Awake()
    {
        GameManager.Inst().IptManager = gameObject.GetComponent<InputManager>();
    }

    void Start()
    {
        IsAbleControl = true;
        IsAbleSWControl = false;
    }

    void OnMouseDrag()
    {
        if (!IsAbleControl && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            return;

        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 MPos = new Vector2(MousePosition.x, MousePosition.y);

        if (!GameManager.Inst().Player.GetIsMovable())
            GameManager.Inst().Player.Rotate(MPos);
        else
        {
            MousePosition.z = 0.0f;

            if (GameManager.Inst().Player.transform.position.y <= 4.2f)
                GameManager.Inst().Player.transform.position = Vector3.MoveTowards(GameManager.Inst().Player.transform.position, MousePosition, Time.deltaTime * 5.0f * SpeedMultiplier);
            else
            {
                if(MousePosition.y > 4.2f)
                    GameManager.Inst().Player.transform.position = Vector3.MoveTowards(GameManager.Inst().Player.transform.position, new Vector3(MousePosition.x, GameManager.Inst().Player.transform.position.y, MousePosition.z), Time.deltaTime * 5.0f * SpeedMultiplier);
                else
                    GameManager.Inst().Player.transform.position = Vector3.MoveTowards(GameManager.Inst().Player.transform.position, MousePosition, Time.deltaTime * 5.0f * SpeedMultiplier);
            }
        }

        GameManager.Inst().Player.Fire();
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
