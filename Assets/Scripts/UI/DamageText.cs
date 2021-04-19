using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public Color[] Colors;
    public int DefaultSize;

    TextMesh DmgText;
    
    
    public void SetText(float dmg) { DmgText.text = ((int)dmg).ToString(); }
    public void SetColor(int type) { DmgText.color = Colors[type]; }
    public void SetSize(int size) { DmgText.fontSize = size; }

    void Awake()
    {
        DmgText = gameObject.transform.GetChild(0).GetComponent<TextMesh>();
        DefaultSize = DmgText.fontSize;
        gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 10;
    }

    /*void Effect()
    {
        //Alpha = Mathf.Lerp(Alpha, 0.0f, Time.deltaTime * 2.0f);
        Alpha -= Time.deltaTime;
        Color color = DmgText.color;
        color.a = Alpha;
        DmgText.color = color;

        Vector3 pos = gameObject.transform.position;
        pos.y += (Time.deltaTime * 0.2f);
        gameObject.transform.position = pos;

        if (Alpha <= 0.0f)
        {
            Alpha = 1.0f;
            gameObject.SetActive(false);
        }
    }*/
}
