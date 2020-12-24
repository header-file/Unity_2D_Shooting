using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectDetail : MonoBehaviour
{
    public GameObject Type;
    public GameObject Value;

    public void SetDatas(int type, int value, Color color)
    {
        switch (type)
        {
            case 0:
                Type.GetComponent<Text>().text = "ATK";
                break;
            case 1:
                Type.GetComponent<Text>().text = "RNG";
                break;
            case 2:
                Type.GetComponent<Text>().text = "SPD";
                break;
        }
        Type.GetComponent<Text>().color = color;

        Value.GetComponent<Text>().text = value.ToString();
    }
}
