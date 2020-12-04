using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public int Index;

    RectTransform Parent;
    RectTransform RectT;
    Vector3 ParentInitPos;

    void Awake()
    {
        RectT = GetComponent<RectTransform>();
        Parent = transform.GetComponentInParent<RectTransform>();
        ParentInitPos = Parent.position;
    }

    void Update()
    {
        //Debug.Log(RectT.transform.position.normalized);
    }

    void OnMouseDown()
    {
        
    }
}
