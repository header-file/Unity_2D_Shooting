using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectDetail : MonoBehaviour
{
    public Text Type;
    public Text Value;

    Color[] Colors;

    void Start()
    {
        Colors = new Color[3];
        Colors[0] = Color.red;
        Colors[1] = new Color(0.5f, 0.0f, 1.0f);
        Colors[2] = new Color(0.1882353f, 0.8862746f, 0.7372549f);
    }

    public void SetDatas(int type, int value)
    {
        switch (type)
        {
            case 0:
                Type.text = "ATK";
                break;
            case 1:
                Type.text = "RNG";
                break;
            case 2:
                Type.text = "SPD";
                break;
        }
        Type.color = Colors[type];

        Value.text = value.ToString();
    }
}
