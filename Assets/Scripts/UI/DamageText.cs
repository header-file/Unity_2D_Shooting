using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    TextMesh DmgText;
    
    public void SetText(float dmg) { DmgText.text = ((int)dmg).ToString(); }

    void Awake()
    {
        DmgText = gameObject.GetComponent<TextMesh>();
        gameObject.GetComponent<MeshRenderer>().sortingOrder = 10;
    }

    void Update()
    {
        
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
