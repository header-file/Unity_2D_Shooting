using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPart : MonoBehaviour
{
    public GameObject Center;

    void Update()
    {
        transform.RotateAround(Center.transform.position, Center.transform.forward, Time.deltaTime * -30.0f);
    }
}
